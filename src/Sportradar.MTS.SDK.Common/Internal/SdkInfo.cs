﻿/*
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

        /// <summary>
        /// Multiplies the specified value
        /// </summary>
        /// <param name="value">The initial value</param>
        /// <param name="factor">The factor</param>
        /// <param name="maxValue">The maximum value</param>
        /// <returns>The multiplied value, up to max value</returns>
        public static int Multiply(int value, double factor = 2, int maxValue = 64000)
        {
            value = (int) (value * factor);
            if (value >= maxValue)
            {
                value = maxValue;
            }
            return value;
        }


        /// <summary>
        /// Increase the specified value
        /// </summary>
        /// <param name="value">The initial value</param>
        /// <param name="factor">The factor (if 0 is *2)</param>
        /// <param name="maxValue">The maximum value</param>
        /// <returns>The increased value, up to max value</returns>
        public static int Increase(int value, int factor = 0, int maxValue = 64000)
        {
            if (factor == 0)
            {
                value = value * 2;
            }
            else
            {
                value = value + factor;
            }
            if (value >= maxValue)
            {
                value = maxValue;
            }
            return value;
        }
    }
}
