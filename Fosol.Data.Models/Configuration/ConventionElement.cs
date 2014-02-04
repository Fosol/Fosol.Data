﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fosol.Data.Models.Configuration
{
    /// <summary>
    /// Provides a way to configure how properties and foreign keys will be mapped and named.
    /// </summary>
    public sealed class ConventionElement
        : ConfigurationElement
    {
        #region Variables
        private bool _IsInitialized = false;

        // Default invalid characters that cannot be used in data model naming.
        // By default these characters will be replaced by the AliasElementCollection.Default value.
        public static readonly string[] InvalidCharacters = new[] { ".", " ", "!", "@", "#", "%", "^", "&", "*", "(", ")", "[", "]", "{", "}", "|", "\"", ",", "/", "?", "`", "~", ":", ";", "'", "<", ">", "-", "=", "+" };
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A collection of AliasElemnt which are used to find and replace invalid characters.
        /// </summary>
        [ConfigurationProperty("aliases", IsRequired = false)]
        public AliasElementCollection Aliases
        {
            get { return (AliasElementCollection)this["aliases"]; }
            set { this["aliases"] = value; }
        }

        /// <summary>
        /// get/set - Controls how to rename foreign keys.
        /// </summary>
        [ConfigurationProperty("foreignKeys", IsRequired = false)]
        public ForeignKeyElement ForeignKeys
        {
            get { return (ForeignKeyElement)this["foreignKeys"]; }
            set { this["foreignKeys"] = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ControlElement.
        /// </summary>
        public ConventionElement()
        {
            this.Aliases = new AliasElementCollection();
            this.ForeignKeys = new ForeignKeyElement();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates an alias for the given value by replacing invalid characters and applying camel case if required.
        /// </summary>
        /// <param name="name">The original name from the datasource.</param>
        /// <returns>A valid alias to use instead of the original name value from the datasource.</returns>
        public string CreateAlias(string name)
        {
            var new_name = new StringBuilder(name.Trim());

            // Aggregate a collection of invalid characters and their replacement values.
            var aliases = (
                from a in this.Aliases
                select a).Distinct();

            // Group aliases that have the same ReplaceWith values.
            var group = (
                from a in aliases
                group a by a.ReplaceWith into g
                select new { ReplaceWith = g.Key, Aliases = g.ToList() });

            // Replace invalid characters that have the same ReplaceWith value.
            foreach (var g in group)
            {
                string exp;
                // Create a regex statement and escape the values you are attempting to find.
                if (g.Aliases.FirstOrDefault(a => a.IsRegex) == null)
                    exp = String.Join("|", g.Aliases.Where(a => !a.IsRegex).Select(c => Regex.Escape(c.Find)));
                else
                    exp = "(" + String.Join("|", g.Aliases.Where(a => !a.IsRegex).Select(c => Regex.Escape(c.Find))) + ")|(" + String.Join(")|(", g.Aliases.Where(a => a.IsRegex).Select(c => c.Find)) + ")";
                var regex = new Regex(exp);

                if (this.Aliases.UseCamelCase)
                {
                    // Loop through all the matches and if they require camel case update them accordingly.
                    var result = new StringBuilder(new_name.ToString());
                    var match = regex.Match(new_name.ToString());
                    while (match.Success)
                    {
                        var alias = aliases.FirstOrDefault(a => a.Find.Equals(match.Value));
                        var len = match.Value.Length;
                        result[match.Index + len] = Char.ToUpper(name[match.Index + len]);
                        match = match.NextMatch();
                    }
                    new_name = new StringBuilder(regex.Replace(result.ToString(), g.ReplaceWith));
                }
                else
                    new_name = new StringBuilder(regex.Replace(new_name.ToString(), g.ReplaceWith));
            }

            // Uppercase the first word.
            if (this.Aliases.UseCamelCase)
                new_name[0] = Char.ToUpper(new_name[0]);

            return new_name.ToString();
        }

        /// <summary>
        /// Initialize a default configuration with aliases for invalid characters.
        /// </summary>
        protected override void Init()
        {
            // Only perform this the first time.
            if (!_IsInitialized)
            {
                _IsInitialized = true;
                var aliases = (
                    from ic in ConventionElement.InvalidCharacters
                    select new AliasElement()
                    {
                        Find = ic,
                        ReplaceWith = this.Aliases.DefaultReplaceWith,
                        IsRegex = false
                    });

                foreach (var alias in aliases)
                {
                    this.Aliases.Add(alias);
                }
            }
            base.Init();
        }
        #endregion

        #region Events
        #endregion

        #region Operators
        #endregion
    }
}