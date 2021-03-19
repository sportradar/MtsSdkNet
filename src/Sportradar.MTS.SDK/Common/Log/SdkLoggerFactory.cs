/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.IO;
using System.Linq;
using log4net;
using log4net.Repository.Hierarchy;
using Sportradar.MTS.SDK.Common.Internal.Log;

namespace Sportradar.MTS.SDK.Common.Log
{
    /// <summary>
    /// Provides methods to get logger for specific <see cref="LoggerType"/>
    /// </summary>
    public static class SdkLoggerFactory
    {
        /// <summary>
        /// Default repository name for the SDK
        /// </summary>
        internal const string SdkLogRepositoryName = "Sportradar.MTS.SDK";

        /// <summary>
        /// Method for getting log4net.ILog for feed traffic
        /// </summary>
        /// <param name="type">A type to be used for creating new ILog</param>
        /// <param name="repositoryName">Repository containing the logger</param>
        /// <returns>Returns default log4net.ILog with specified settings</returns>
        public static ILog GetLoggerForFeedTraffic(Type type, string repositoryName = SdkLogRepositoryName)
        {
            return GetLogger(type, repositoryName, LoggerType.FeedTraffic);
        }

        /// <summary>
        /// Method for getting log4net.ILog for rest traffic
        /// </summary>
        /// <param name="type">A type to be used for creating new ILog</param>
        /// <param name="repositoryName">Repository containing the logger</param>
        /// <returns>Returns default <see cref="ILog"/> with specified settings</returns>
        public static ILog GetLoggerForRestTraffic(Type type, string repositoryName = SdkLogRepositoryName)
        {
            return GetLogger(type, repositoryName, LoggerType.RestTraffic);
        }

        /// <summary>
        /// Method for getting log4net.ILog for client interaction
        /// </summary>
        /// <param name="type">A type to be used for creating new ILog</param>
        /// <param name="repositoryName">Repository containing the logger</param>
        /// <returns>Returns default log4net.ILog with specified settings</returns>
        public static ILog GetLoggerForClientInteraction(Type type, string repositoryName = SdkLogRepositoryName)
        {
            return GetLogger(type, repositoryName, LoggerType.ClientInteraction);
        }

        /// <summary>
        /// Method for getting log4net.ILog for cache
        /// </summary>
        /// <param name="type">A type to be used for creating new ILog</param>
        /// <param name="repositoryName">Repository containing the logger</param>
        /// <returns>Returns default <see cref="ILog"/> with specified settings</returns>
        public static ILog GetLoggerForCache(Type type, string repositoryName = SdkLogRepositoryName)
        {
            return GetLogger(type, repositoryName, LoggerType.Cache);
        }

        /// <summary>
        /// Method for getting log4net.ILog for statistics
        /// </summary>
        /// <param name="type">A type to be used for creating new ILog</param>
        /// <param name="repositoryName">Repository containing the logger</param>
        /// <returns>Returns default log4net.ILog with specified settings</returns>
        public static ILog GetLoggerForStats(Type type, string repositoryName = SdkLogRepositoryName)
        {
            return GetLogger(type, repositoryName, LoggerType.Stats);
        }

        /// <summary>
        /// Method for getting default execution log4net.ILog
        /// </summary>
        /// <param name="type">A type to be used for creating new ILog</param>
        /// <param name="repositoryName">Repository containing the logger</param>
        /// <returns>Returns default log4net.ILog with specified settings</returns>
        public static ILog GetLoggerForExecution(Type type, string repositoryName = SdkLogRepositoryName)
        {
            return GetLogger(type, repositoryName);
        }

        /// <summary>
        /// Method for getting correct log4net.ILog for specific logger type
        /// </summary>
        /// <param name="type">A type to be used for creating new ILog</param>
        /// <param name="repositoryName">Repository containing the logger</param>
        /// <param name="loggerType">A value of <see cref="LoggerType"/> to be used to get log4net.ILog</param>
        /// <returns>Returns log4net.ILog with specific settings for this <see cref="LoggerType"/></returns>
        public static ILog GetLogger(Type type, string repositoryName = SdkLogRepositoryName, LoggerType loggerType = LoggerType.Execution)
        {
            return new SdkLogger(loggerType, type, repositoryName);
        }

        /// <summary>
        /// Method for getting correct log4net.ILog for specific logger type
        /// </summary>
        /// <param name="typeName">The name of the type to be used for creating new ILog</param>
        /// <param name="repositoryName">Repository containing the logger</param>
        /// <param name="loggerType">A value of <see cref="LoggerType"/> to be used to get log4net.ILog</param>
        /// <returns>Returns log4net.ILog with specific settings for this <see cref="LoggerType"/></returns>
        public static ILog GetLogger(string typeName, string repositoryName = SdkLogRepositoryName, LoggerType loggerType = LoggerType.Execution)
        {
            return new SdkLogger(loggerType, typeName, repositoryName);
        }

        /// <summary>
        /// Configures the LogManager for SDK
        /// </summary>
        /// <param name="configFile">The configuration file</param>
        /// <param name="repositoryName">Repository name to be created containing the SDK loggers</param>
        public static void Configure(FileInfo configFile, string repositoryName = SdkLogRepositoryName)
        {
            var reps = LogManager.GetAllRepositories();
            if (reps.Any(r => r.Name == repositoryName))
            {
                return;
            }
            if (string.IsNullOrEmpty(repositoryName))
            {
                repositoryName = SdkLogRepositoryName;
            }
            var sdkRep = LogManager.CreateRepository(repositoryName);
            log4net.Config.XmlConfigurator.Configure(sdkRep, configFile);
        }

        /// <summary>
        /// Method checks if all loggers are created for each <see cref="LoggerType"/>
        /// </summary>
        /// <param name="repositoryName">Repository containing the loggers</param>
        /// <returns>Returns value indicating if all sdk defined loggers exists</returns>
        public static bool CheckAllLoggersExists(string repositoryName = SdkLogRepositoryName)
        {
            Hierarchy hierarchy;
            try
            {
                if (string.IsNullOrEmpty(repositoryName))
                {
                    repositoryName = SdkLogRepositoryName;
                }
                hierarchy = (Hierarchy)LogManager.GetRepository(repositoryName);
            }
            catch (Exception)
            {
                return false;
            }
            var loggers = hierarchy.GetCurrentLoggers().ToList(); //LogManager.GetCurrentLoggers().ToList();
            var enums = Enum.GetNames(typeof(LoggerType));

            foreach (var e in enums)
            {
                if (loggers.Find(l => l.Name == SdkLogRepositoryName + "." + e) == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
