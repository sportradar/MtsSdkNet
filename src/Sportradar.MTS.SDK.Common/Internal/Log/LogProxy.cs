﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Threading.Tasks;
using log4net;
using Sportradar.MTS.SDK.Common.Log;

namespace Sportradar.MTS.SDK.Common.Internal.Log
{
    /// <summary>
    /// A log proxy used to log input and output parameters of a method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogProxy<T> : RealProxy
    {
        private Predicate<MethodInfo> _filter;
        private readonly LoggerType _defaultLoggerType;
        private readonly bool _canOverrideLoggerType;

        private readonly T _decorated;

        private struct LogProxyPerm
        {
            public bool LogEnabled;
            public IMethodMessage MethodCall;
            public MethodInfo MethodInfo;
            public object Result;
            public ILog Logger;
            public Stopwatch Watch;
        }

        private readonly IDictionary<int, LogProxyPerm> _proxyPerms;

        /// <summary>
        /// Initializes new instance of the <see cref="LogProxy{T}"/>
        /// </summary>
        /// <param name="decorated">A base class to be decorated</param>
        /// <param name="loggerType">A <see cref="LoggerType"/> to be used within the proxy</param>
        /// <param name="canOverrideLoggerType">A value indicating if the <see cref="LoggerType"/> can be overridden with <see cref="LogAttribute"/> on a method or class</param>
        public LogProxy(T decorated, LoggerType loggerType = LoggerType.Execution, bool canOverrideLoggerType = true)
            : base(typeof(T))
        {
            _decorated = decorated;
            _defaultLoggerType = loggerType;
            _canOverrideLoggerType = canOverrideLoggerType;

            _proxyPerms = new ConcurrentDictionary<int, LogProxyPerm>();
        }

        /// <summary>
        /// A Predicate used to filter which class methods may be logged
        /// </summary>
        public Predicate<MethodInfo> Filter
        {
            get { return _filter; }
            set
            {
                if (value == null)
                {
                    _filter = m => true;
                }
                else
                {
                    _filter = value;
                }
            }
        }

        /// <summary>When overridden in a derived class, invokes the method that is specified in the provided <see cref="IMessage" /> on the remote object that is represented by the current instance</summary>
        /// <returns>The message returned by the invoked method, containing the return value and any out or ref parameters</returns>
        /// <param name="msg">A <see cref="IMessage" /> that contains a <see cref="IDictionary" /> of information about the method call</param>
        public override IMessage Invoke(IMessage msg)
        {
            bool logEnabled = false;

            var methodCall = msg as IMethodCallMessage;
            if (methodCall == null)
            {
                throw new ArgumentException("Input parameter 'msg' not valid IMethodCallMessage.");
            }
            var methodInfo = methodCall.MethodBase as MethodInfo;
            if (methodInfo == null)
            {
                throw new ArgumentException("Input parameter 'msg' does not have MethodBase as MethodInfo.");
            }

            ILog logger = SdkLoggerFactory.GetLogger(methodCall.MethodBase.ReflectedType, SdkLoggerFactory.SdkLogRepositoryName, _defaultLoggerType);

            if (_filter != null && _filter(methodInfo))
            {
                logEnabled = true;
            }

            if (!logEnabled || _canOverrideLoggerType)
            {
                var attributes = methodInfo.GetCustomAttributes(true).ToList();
                if (methodInfo.DeclaringType != null)
                {
                    attributes.AddRange(methodInfo.DeclaringType.GetCustomAttributes(true));
                }

                if (attributes.Count > 0)
                {
                    foreach (object t in attributes)
                    {
                        if (!(t is LogAttribute))
                        {
                            continue;
                        }
                        logEnabled = true;
                        if (_canOverrideLoggerType)
                        {
                            logger = SdkLoggerFactory.GetLogger(methodCall.MethodBase.ReflectedType, SdkLoggerFactory.SdkLogRepositoryName, ((LogAttribute)t).LoggerType);
                        }
                        break;
                    }
                }
            }

            var watch = new Stopwatch();
            watch.Start();

            try
            {
                if (logEnabled)
                {
                    logger.Info($"Starting executing '{methodCall.MethodName}' ...");
                }
                if (logEnabled && methodCall.InArgCount > 0)
                {
                    logger.Debug($"{methodCall.MethodName} arguments:");
                    for (int i = 0; i < methodCall.InArgCount; i++)
                    {
                        logger.Debug($"\t{((Type[])methodCall.MethodSignature)[i].Name}={methodCall.InArgs[i]}");
                    }
                }

                var result = methodInfo.Invoke(_decorated, methodCall.InArgs); // MAIN EXECUTION

                var task = result as Task;
                if (task != null)
                {
                    var perm = new LogProxyPerm()
                    {
                        LogEnabled = logEnabled,
                        Logger = logger,
                        MethodCall = methodCall,
                        MethodInfo = methodInfo,
                        Result = result,
                        Watch = watch
                    };
                    _proxyPerms.Add(task.Id, perm);
                    if (logEnabled)
                    {
                        logger.Debug($"TaskId:{task.Id} is executing and we wait to finish ...");
                    }
                    task.ContinueWith(TaskExecutionFinished);
                    return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
                }

                FinishExecution(logEnabled, methodCall, methodInfo, result?.GetType().Name, result, logger, watch);
                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                watch.Stop();
                if (logEnabled)
                {
                    logger.Error($"Exception during executing '{methodCall.MethodName}': {Environment.NewLine}", e);
                }
                return new ReturnMessage(e, methodCall);
            }
        }

        private void TaskExecutionFinished(Task task)
        {
            LogProxyPerm perm;
            if (!_proxyPerms.TryGetValue(task.Id, out perm))
            {
                Console.WriteLine($"No perm for task. Id: {task.Id}");
                return;
            }
            var underlyingResultType = "Task->" + perm.Result.GetType().GetProperty("Result")?.PropertyType.Name;

            if (task.IsFaulted)
            {
                string exceptionMsg = "EXCEPTION: ";
                if (task.Exception != null)
                {
                    exceptionMsg += task.Exception?.InnerExceptions[0].ToString();
                }
                FinishExecution(perm.LogEnabled, perm.MethodCall, perm.MethodInfo, underlyingResultType, exceptionMsg, perm.Logger, perm.Watch, taskId: $"TaskId:{task.Id}, ");
                return;
            }
            var value = perm.Result.GetType().GetProperty("Result")?.GetValue(task);

            FinishExecution(perm.LogEnabled, perm.MethodCall, perm.MethodInfo, underlyingResultType, value, perm.Logger, perm.Watch, taskId: $"TaskId:{task.Id}, ");
            _proxyPerms.Remove(task.Id);
        }

        private static void FinishExecution(bool logEnabled, IMethodMessage methodCall, MethodInfo methodInfo, string resultTypeName, object result, ILog logger, Stopwatch watch, string taskId = null)
        {
            watch.Stop();

            if (logEnabled)
            {
                logger.Info($"{taskId}Finished executing '{methodCall.MethodName}'. Time: {watch.ElapsedMilliseconds} ms.");
            }

            if (logEnabled && !string.Equals(methodInfo.ReturnType.FullName, "System.Void"))
            {
                logger.Debug($"{taskId}{methodCall.MethodName} result: {resultTypeName}={result};");
            }
        }
    }
}
