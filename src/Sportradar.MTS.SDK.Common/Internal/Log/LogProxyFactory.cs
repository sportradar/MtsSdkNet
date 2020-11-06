/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Reflection;
using Sportradar.MTS.SDK.Common.Log;

namespace Sportradar.MTS.SDK.Common.Internal.Log
{
    /// <summary>
    /// Factory for the <see cref="LogProxy{T}"/>
    /// </summary>
    public class LogProxyFactory
    {
        public static T Create<T>(object[] args, LoggerType loggerType = LoggerType.Execution, bool canOverrideLoggerType = true)
        {
            var tmp = (T)Activator.CreateInstance(typeof(T), args);
            var logProxy = new LogProxy<T>(tmp, loggerType, canOverrideLoggerType);
            return (T)logProxy.GetTransparentProxy();
        }

        public static T Create<T>(object[] args, Predicate<MethodInfo> filter, LoggerType loggerType = LoggerType.Execution, bool canOverrideLoggerType = true)
        {
            var tmp = (T)Activator.CreateInstance(typeof(T), args);
            var logProxy = new LogProxy<T>(tmp, loggerType, canOverrideLoggerType) { Filter = filter };
            return (T)logProxy.GetTransparentProxy();
        }

        public static T Create<T>(LoggerType loggerType, bool canOverrideLoggerType, params object[] args)
        {
            var tmp = (T)Activator.CreateInstance(typeof(T), args);
            var logProxy = new LogProxy<T>(tmp, loggerType, canOverrideLoggerType);
            return (T)logProxy.GetTransparentProxy();
        }

        public static T Create<T>(Predicate<MethodInfo> filter, LoggerType loggerType, bool canOverrideLoggerType, params object[] args)
        {
            var tmp = (T)Activator.CreateInstance(typeof(T), args);
            var logProxy = new LogProxy<T>(tmp, loggerType, canOverrideLoggerType) { Filter = filter };
            return (T)logProxy.GetTransparentProxy();
        }
    }
}
