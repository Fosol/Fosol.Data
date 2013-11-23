using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fosol.Data.Models.Configuration
{
    public sealed class ControlElement
        : ConfigurationElement
    {
        #region Variables
        public const string[] InvalidCharacters = new[] { ".", " " };
        #endregion

        #region Properties
        [ConfigurationProperty("aliases", IsRequired = false)]
        public AliasElementCollection Aliases
        {
            get { return (AliasElementCollection)this["invalidCharacters"]; }
            set { this["invalidCharacters"] = value; }
        }

        [ConfigurationProperty("foreignKeys", IsRequired = false)]
        public ForeignKeyElement ForeignKeys
        {
            get { return (ForeignKeyElement)this["foreignKeys"]; }
            set { this["foreignKeys"] = value; }
        }
        #endregion

        #region Constructors
        public ControlElement()
        {
            this.Aliases = new AliasElementCollection();
            this.ForeignKeys = new ForeignKeyElement();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates an alias for the given value by replacing invalid characters with new values.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string CreateAlias(string name)
        {
            // Aggregate a collection of invalid characters and their replacement values.
            var replace = (
                from a in this.Aliases
                join ic in ControlElement.InvalidCharacters
                    on a.Find equals ic
                select new { Find = ic ?? a.Find, Replace = a.Replace ?? this.Aliases.Default, UserCamelCase = a.UseCamelCase }
                ).Distinct();

            // Create a regex statement and escape the values you are attempting to find.
            var regex = new Regex(String.Join("|", replace.Select(c => Regex.Escape(c.Find))));
            return regex.Replace(name, this.Aliases.Default);
        }
        #endregion

        #region Events
        #endregion

        #region Operators
        #endregion
    }
}
