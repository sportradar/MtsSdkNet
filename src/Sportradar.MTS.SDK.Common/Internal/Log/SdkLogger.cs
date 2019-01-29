/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Sportradar.MTS.SDK.Common.Log;

namespace Sportradar.MTS.SDK.Common.Internal.Log
{
    /// <summary>
    /// A logger used to log different events in sdk
    /// </summary>
    public class SdkLogger : ILog
    {
        private readonly ILog _log;

        /// <summary>Get the implementation behind this wrapper object</summary>
        /// <value>
        /// The <see cref="T:log4net.Core.ILogger" /> object that in implementing this object.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <see cref="T:log4net.Core.ILogger" /> object that in implementing this
        /// object. The <c>Logger</c> object may not
        /// be the same object as this object because of logger decorators.
        /// This gets the actual underlying objects that is used to process
        /// the log events.
        /// </para>
        /// </remarks>
        public ILogger Logger { get; }

        /// <summary>
        /// Gets the name of the logger
        /// </summary>
        public string Name => _log?.Logger.Name;

        /// <summary>
        /// Checks if the logger is enabled for Debug level
        /// </summary>
        public bool IsDebugEnabled => _log.IsDebugEnabled;

        /// <summary>
        /// Checks if the logger is enabled for Info level
        /// </summary>
        public bool IsInfoEnabled => _log.IsInfoEnabled;

        /// <summary>
        /// Checks if the logger is enabled for Warn level
        /// </summary>
        public bool IsWarnEnabled => _log.IsWarnEnabled;

        /// <summary>
        /// Checks if the logger is enabled for Error level
        /// </summary>
        public bool IsErrorEnabled => _log.IsErrorEnabled;

        /// <summary>
        /// Checks if the logger is enabled for Fatal level
        /// </summary>
        public bool IsFatalEnabled => _log.IsFatalEnabled;

        internal SdkLogger(LoggerType loggerType, string className, string repositoryName = null)
        {
            Hierarchy hierarchy;
            try
            {
                if (string.IsNullOrEmpty(repositoryName))
                {
                    repositoryName = SdkLoggerFactory.SdkLogRepositoryName;
                }
                hierarchy = (Hierarchy) LogManager.GetRepository(repositoryName);
            }
            catch (Exception)
            {
                //make empty logger
                hierarchy = (Hierarchy) LogManager.GetRepository();
                var emptyLogger = hierarchy.LoggerFactory.CreateLogger(hierarchy, className);
                emptyLogger.Hierarchy = hierarchy;
                emptyLogger.RemoveAllAppenders();
                emptyLogger.Level = Level.Off;
                Logger = emptyLogger;
                _log = new LogImpl(emptyLogger);
                return;
            }
            var typeLogger = hierarchy.GetLogger(SdkLoggerFactory.SdkLogRepositoryName + "." + loggerType);
            var logger = hierarchy.LoggerFactory.CreateLogger(hierarchy, className);
            logger.Hierarchy = hierarchy;
            logger.Level = ((Logger)typeLogger).Level ?? ((Logger)typeLogger).EffectiveLevel;
            var appenders = ((Logger)typeLogger).Appenders.ToArray();
            if (appenders.Length == 0)
            {
                appenders = hierarchy.Root.Appenders.ToArray();
            }
            logger.RemoveAllAppenders();
            foreach (var a in appenders)
            {
                logger.AddAppender(a);
            }

            Logger = logger;
            _log = new LogImpl(logger);
        }

        /// <summary>
        /// Creates new SdkLogger for specific <see cref="LoggerType"/>
        /// </summary>
        /// <param name="loggerType">A <see cref="LoggerType"/> to be used when creating new SdkLogger</param>
        /// <param name="classType">A type to be used when creating new SdkLogger</param>
        /// <param name="repositoryName">Repository containing the logger</param>
        public SdkLogger(LoggerType loggerType, Type classType, string repositoryName = null)
            :this(loggerType, classType.FullName, repositoryName)
        {

        }

        /// <summary>
        /// Logs a message with Info level
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            //_log.Info(_logKeyword + message);
            _log.Info(message);
        }

        /// <summary>
        /// Logs a message with Debug level
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            _log.Debug(message);
        }

        /// <summary>
        /// Logs a message with Warn level
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            _log.Warn(message);
        }

        /// <summary>
        /// Logs a message with Error level
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            _log.Error(message);
        }

        /// <summary>
        /// Logs a message with Fatal level
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(string message)
        {
            _log.Fatal(message);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" /> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.FatalFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" /> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.DebugFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message with Info level
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            _log.Info(message);
        }

        /// <summary>
        /// Logs a message object with the <c>INFO</c> level including
        /// the stack trace of the <see cref="T:System.Exception" /> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <param name="exception">The exception to log, including its stack trace</param>
        /// <seealso cref="M:Info(object)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        /// <remarks>See the <see cref="M:Info(object)" /> form for more detailed information</remarks>
        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Info" /> level</overloads>
        /// <seealso cref="M:Info(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object)" />
        /// methods instead.
        /// </para></remarks>
        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <seealso cref="M:Info(object)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void InfoFormat(string format, object arg0)
        {
            _log.InfoFormat(format, arg0);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <seealso cref="M:Info(object)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void InfoFormat(string format, object arg0, object arg1)
        {
            _log.InfoFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <seealso cref="M:Info(object)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.InfoFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" /> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <seealso cref="M:Info(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object)" />
        /// methods instead.
        /// </para></remarks>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.InfoFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message with Debug level
        /// </summary>
        /// <param name="message"></param>
        public void Debug(object message)
        {
            _log.Debug(message);
        }

        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Debug" /> level including
        /// the stack trace of the <see cref="T:System.Exception" /> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <param name="exception">The exception to log, including its stack trace</param>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <remarks>See the <see cref="M:Debug(object)" /> form for more detailed information</remarks>
        public void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted string with the <see cref="F:log4net.Core.Level.Debug" /> level</overloads>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void DebugFormat(string format, object arg0)
        {
            _log.DebugFormat(format, arg0);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void DebugFormat(string format, object arg0, object arg1)
        {
            _log.DebugFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.DebugFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Logs a message with Warn level
        /// </summary>
        /// <param name="message"></param>
        public void Warn(object message)
        {
            _log.Warn(message);
        }

        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Warn" /> level including
        /// the stack trace of the <see cref="T:System.Exception" /> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <param name="exception">The exception to log, including its stack trace</param>
        /// <seealso cref="M:Warn(object)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        /// <remarks>See the <see cref="M:Warn(object)" /> form for more detailed information</remarks>
        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Warn" /> level</overloads>
        /// <seealso cref="M:Warn(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object)" />
        /// methods instead.
        /// </para></remarks>
        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <seealso cref="M:Warn(object)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void WarnFormat(string format, object arg0)
        {
            _log.WarnFormat(format, arg0);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <seealso cref="M:Warn(object)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void WarnFormat(string format, object arg0, object arg1)
        {
            _log.WarnFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <seealso cref="M:Warn(object)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.WarnFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" /> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <seealso cref="M:Warn(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object)" />
        /// methods instead.
        /// </para></remarks>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.WarnFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message with Error level
        /// </summary>
        /// <param name="message"></param>
        public void Error(object message)
        {
            _log.Error(message);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" /> level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <seealso cref="M:Error(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object)" />
        /// methods instead.
        /// </para></remarks>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.ErrorFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message with Fatal level
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Fatal" /> level including
        /// the stack trace of the <see cref="T:System.Exception" /> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log</param>
        /// <param name="exception">The exception to log, including its stack trace</param>
        /// <seealso cref="M:Fatal(object)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        /// <remarks>See the <see cref="M:Fatal(object)" /> form for more detailed information</remarks>
        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" /> level</overloads>
        /// <seealso cref="M:Fatal(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object)" />
        /// methods instead.
        /// </para></remarks>
        public void FatalFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <seealso cref="M:Fatal(object)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void FatalFormat(string format, object arg0)
        {
            _log.WarnFormat(format, arg0);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <seealso cref="M:Fatal(object)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void FatalFormat(string format, object arg0, object arg1)
        {
            _log.WarnFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <seealso cref="M:Fatal(object)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.WarnFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Logs a message with Error level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Error(object message, Exception ex)
        {
            _log.Error(message, ex);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Error" /> level</overloads>
        /// <seealso cref="M:Error(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object)" />
        /// methods instead.
        /// </para></remarks>
        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <seealso cref="M:Error(object)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void ErrorFormat(string format, object arg0)
        {
            _log.ErrorFormat(format, arg0);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <seealso cref="M:Error(object)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            _log.ErrorFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" /> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <seealso cref="M:Error(object)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        /// <remarks><para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object,Exception)" />
        /// methods instead.
        /// </para></remarks>
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.ErrorFormat(format, arg0, arg1, arg2);
        }
    }
}
