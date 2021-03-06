﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A CodeFactory is an abstract class which provides a way to generate code for a specified data Model.
    /// Create a specific CodeFactory for each language.
    /// </summary>
    public abstract class CodeFactory
    {
        #region Variables
        private ModelFactory _Factory;
        #endregion

        #region Properties
        /// <summary>
        /// get - A reference to a ModelFactory object that will be used to generate the code.
        /// </summary>
        protected ModelFactory ModelFactory
        {
            get { return _Factory; }
        }
        #endregion

        #region Constructors
        public CodeFactory(ModelFactory factory)
        {
            Fosol.Common.Validation.Assert.IsNotNull(factory, "factory");
            _Factory = factory;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generate the code files for the data Model.
        /// </summary>
        /// <param name="pathToFolder">Path to the folder you want to save the code files in.</param>
        public abstract void Generate(string pathToFolder);

        /// <summary>
        /// Save the code into the specified file.
        /// </summary>
        /// <param name="fullPath">Path to directory and the name of the file.</param>
        /// <param name="code">Code to save into the file.</param>
        protected void SaveToFile(string fullPath, string code)
        {
            var path = System.IO.Path.GetDirectoryName(fullPath);

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            var file = System.IO.File.CreateText(fullPath);
            file.Write(code);
            file.Close();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
