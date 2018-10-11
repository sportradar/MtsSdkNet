/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Reflection;

namespace Sportradar.MTS.SDK.Common.Internal
{
    /// <summary>
    /// Class provides information about current executing assembly
    /// </summary>
    public static class SdkInfo
    {
        /// <summary>
        /// Gets the version number of the executing assembly
        /// </summary>
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Gets the assembly version number
        /// </summary>
        public static string GetVersion(Assembly assembly)
        {
            return assembly.GetName().Version.ToString();
        }
    }
}
