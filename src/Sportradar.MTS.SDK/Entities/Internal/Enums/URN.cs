﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Dawn;

namespace Sportradar.MTS.SDK.Entities.Internal.Enums
{
    /// <summary>
    /// Represents a Uniform Resource Name
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal class URN
    {
        /// <summary>
        /// A regex pattern used for parsing of URN strings
        /// </summary>
        private static readonly string RegexPattern = $@"\A(?<{PrefixGroupName}>sr):(?<{TypeGroupName}>[a-zA-Z_2]+):(?<{IdGroupName}>\d+)\z";

        /// <summary>
        /// The name of the regex group used to store the prefix
        /// </summary>
        private const string PrefixGroupName = "prefix";

        /// <summary>
        /// The name of the regex group used to store the type
        /// </summary>
        private const string TypeGroupName = "type";

        /// <summary>
        /// The name of the regex group used to store the id
        /// </summary>
        private const string IdGroupName = "id";

        /// <summary>
        /// Defines supported resource types
        /// </summary>
        private static readonly Tuple<string, ResourceTypeGroup>[] Types = {
            new Tuple<string, ResourceTypeGroup>("sport_event", ResourceTypeGroup.Match),
            new Tuple<string, ResourceTypeGroup>("race_event", ResourceTypeGroup.Race),
            new Tuple<string, ResourceTypeGroup>("season", ResourceTypeGroup.Tournament),
            new Tuple<string, ResourceTypeGroup>("tournament", ResourceTypeGroup.Tournament),
            new Tuple<string, ResourceTypeGroup>("race_tournament", ResourceTypeGroup.Tournament),
            new Tuple<string, ResourceTypeGroup>("simple_tournament", ResourceTypeGroup.Tournament),
            new Tuple<string, ResourceTypeGroup>("h2h_tournament", ResourceTypeGroup.Tournament),
            new Tuple<string, ResourceTypeGroup>("sport", ResourceTypeGroup.Other),
            new Tuple<string, ResourceTypeGroup>("category", ResourceTypeGroup.Other),
            new Tuple<string, ResourceTypeGroup>("match", ResourceTypeGroup.Match),
            new Tuple<string, ResourceTypeGroup>("team", ResourceTypeGroup.Other),
            new Tuple<string, ResourceTypeGroup>("competitor", ResourceTypeGroup.Other),
            new Tuple<string, ResourceTypeGroup>("simpleteam", ResourceTypeGroup.Other),
            new Tuple<string, ResourceTypeGroup>("venue", ResourceTypeGroup.Other),
            new Tuple<string, ResourceTypeGroup>("player", ResourceTypeGroup.Other),
            new Tuple<string, ResourceTypeGroup>("referee", ResourceTypeGroup.Other),
            new Tuple<string, ResourceTypeGroup>("market", ResourceTypeGroup.Other)
        };

        /// <summary>
        /// Gets the prefix of the current instance
        /// </summary>
        /// <value>The prefix</value>
        public string Prefix { get; }

        /// <summary>
        /// Gets a string specifying the type of the resource associated with the current instance
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets a <see cref="ResourceTypeGroup"/> enum member describing the group of the resource
        /// </summary>
        /// <seealso cref="ResourceTypeGroup"/>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public ResourceTypeGroup TypeGroup { get; }

        /// <summary>
        /// Gets the numerical part of the identifier associated with the current instance
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="URN"/> class
        /// </summary>
        /// <param name="prefix">The prefix of the URN</param>
        /// <param name="type">The type of the resource associated with the URN</param>
        /// <param name="id">The numerical identifier of the resource associated with the URN</param>
        public URN(string prefix, string type, long id)
        {
            Guard.Argument(prefix, nameof(prefix)).NotNull().NotEmpty();
            Guard.Argument(type, nameof(type)).NotNull().NotEmpty();
            Guard.Argument(id, nameof(id)).Positive();

            var tuple = Types.FirstOrDefault(t => t.Item1 == type);
            if (tuple == null)
            {
                throw new ArgumentException($"Value '{type}' is not a valid type", nameof(type));
            }

            TypeGroup = tuple.Item2;
            Prefix = prefix;
            Type = type;
            Id = id;
        }

        /// <summary>
        /// Constructs a <see cref="URN"/> instance by parsing the provided string
        /// </summary>
        /// <param name="urnString">The string representation of the URN</param>
        /// <returns>A <see cref="URN"/> constructed by parsing the provided string representation</returns>
        /// <exception cref="FormatException">The format of the provided representation is not correct</exception>
        public static URN Parse(string urnString)
        {
            Guard.Argument(urnString, nameof(urnString)).NotNull().NotEmpty();

            var match = Regex.Match(urnString, RegexPattern);
            if (!match.Success)
            {
                throw new FormatException($"Value '{urnString}' is not a valid string representation of the URN");
            }

            var type = match.Groups[TypeGroupName].Value;
            if (Types.All(t => t.Item1 != type))
            {
                throw new FormatException($"Resource type name: '{type}' is not supported");
            }

            return new URN(
                match.Groups[PrefixGroupName].Value,
                match.Groups[TypeGroupName].Value,
                long.Parse(match.Groups[IdGroupName].Value));
        }

        /// <summary>
        /// Tries to construct a <see cref="URN"/> instance by parsing the provided string
        /// </summary>
        /// <param name="urnString">The string representation of the URN</param>
        /// <param name="urn">When the method returns it contains the <see cref="URN"/> constructed by parsing the provided string if the parsing was successful, otherwise null</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool TryParse(string urnString, out URN urn)
        {
            Guard.Argument(urnString, nameof(urnString)).NotNull().NotEmpty();

            var success = false;

            try
            {
                urn = Parse(urnString);
                success = true;
            }
            catch (FormatException)
            {
                urn = null;
            }
            return success;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance</returns>
        public override string ToString()
        {
            return $"{Prefix}:{Type}:{Id}";
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as URN;
            if (other == null)
            {
                return false;
            }

            return Prefix == other.Prefix && Type == other.Type && Id == other.Id;
        }

        /// <summary>
        /// Returns a hash code for this instance
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}