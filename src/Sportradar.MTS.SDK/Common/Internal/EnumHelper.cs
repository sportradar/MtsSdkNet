﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Globalization;

namespace Sportradar.MTS.SDK.Common.Internal
{
    /// <summary>
    /// Defines static methods used for manipulate with enums
    /// </summary>
    internal static class EnumHelper
    {
        /// <summary>
        /// Determines whether the provided value is member of the specified enumeration
        /// </summary>
        /// <typeparam name="TEnum">The enumeration whose members are to be checked</typeparam>
        /// <param name="value">The value to check</param>
        /// <returns>True if the provided value is member of the specified enumeration. Otherwise false</returns>
        public static bool IsEnumMember<TEnum>(object value) where TEnum : struct
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException($"Type specified by generic parameter T must be an enum. Provided type was:{typeof(TEnum).FullName}");
            }
            return Enum.IsDefined(enumType, value);
        }

        /// <summary>
        /// Converts the provided <code>value</code> to the member of the specified enum
        /// </summary>
        /// <typeparam name="T">The type of enum to which to convert the <code>value</code></typeparam>
        /// <param name="value">The value to be converted</param>
        /// <returns>The member of the specified enum</returns>
        public static T GetEnumValue<T>(int value) where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enum");
            }
            if (!Enum.IsDefined(type, value))
            {
                throw new ArgumentException($"Value:{value} is not a member of enum{type.Name}", nameof(value));
            }
            return (T)(object)value;
        }

        /// <summary>
        /// Converts the provided <code>value</code> to the member of the specified enum, or returns <code>defaultValue</code>
        /// if value of <code>specified</code> is false
        /// </summary>
        /// <typeparam name="T">The type of enum to which to convert the <code>value</code></typeparam>
        /// <param name="specified">Value indicating whether the value field was specified in the feed message</param>
        /// <param name="value">The value in the feed message</param>
        /// <param name="defaultValue">A member of enum T to be returned if <code>specified</code> is false</param>
        /// <returns>The <code>value</code> converted to enum member T</returns>
        public static T GetEnumValue<T>(bool specified, int value, T defaultValue) where T : struct, IConvertible
        {
            return !specified ? defaultValue : GetEnumValue<T>(value);
        }

        /// <summary>
        /// Converts the provided <code>value</code> to the member of the specified enum, or returns <code>defaultValue</code>
        /// </summary>
        /// <typeparam name="T">The type of enum to which to convert the <code>value</code></typeparam>
        /// <param name="value">The value in the feed message</param>
        /// <param name="defaultValue">A T member to be returned if unknown <code>value</code></param>
        /// <returns>The <code>value</code> converted to T enum member</returns>
        public static T GetEnumValue<T>(int value, T defaultValue) where T : struct, IConvertible
        {
            try
            {
                return GetEnumValue<T>(value);
            }
            catch
            {
                // ignored
            }
            return defaultValue;
        }

        /// <summary>
        /// Converts the provided <code>value</code> (enum value name) to the member of the specified enum
        /// </summary>
        /// <typeparam name="T">The type of enum to which to convert the <code>value</code></typeparam>
        /// <param name="value">The value name to be converted</param>
        /// <returns>The member of the specified enum</returns>
        public static T GetEnumValue<T>(string value) where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enum");
            }
            var enumValues = Enum.GetValues(type);
            foreach (int v in enumValues)
            {
                var enumChoice = (T)(object)v;
                if (string.Equals(enumChoice.ToString(CultureInfo.InvariantCulture), value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return enumChoice;
                }
            }
            throw new ArgumentException($"Value:{value} is not a member of enum{type.Name}", nameof(value));
        }

        /// <summary>
        /// Converts the provided <code>value</code> (enum value name) to the member of the specified enum
        /// </summary>
        /// <typeparam name="T">The type of enum to which to convert the <code>value</code></typeparam>
        /// <param name="value">The value name to be converted</param>
        /// <param name="defaultValue">A member of enum T to be returned if <code>value</code> is not member of enum</param>
        /// <returns>The member of the specified enum</returns>
        public static T GetEnumValue<T>(string value, T defaultValue) where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enum");
            }
            var enumValues = Enum.GetValues(type);
            foreach (int v in enumValues)
            {
                var enumChoice = (T)(object)v;
                if (string.Equals(enumChoice.ToString(CultureInfo.InvariantCulture), value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return enumChoice;
                }
            }
            return defaultValue;
        }
    }
}
