using Fosol.Common.Extensions.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.CSharp
{
    /// <summary>
    /// A CSharpCodeFactory generates c# code objects for the specified data Model.
    /// </summary>
    public sealed class CSharpCodeFactory
        : CodeFactory
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public CSharpCodeFactory(ModelFactory factory)
            : base(factory)
        {
        }
        #endregion

        #region Methods

        public override void Generate()
        {
            // Create the in-memory whole data Model from the database.
            var model = this.ModelFactory.Generate();

            foreach (var table in model.Tables)
            {
                SaveToFile(table.Name + ".cs", GenerateTable(table));
            }

            SaveToFile("Context.cs", GenerateContext(model));
        }

        private string GenerateTable(Fosol.Data.Models.Table table)
        {
            var code = new StringBuilder();

            code.AppendLine("using System;");
            code.AppendLine("using System.ComponentModel;");
            code.AppendLine("using System.ComponentModel.DataNotations;");
            code.AppendLine("using System.ComponentModel.DataNotations.Schema;");
            code.AppendLine("using System.Data.Entity;");
            code.AppendLine("");

            code.AppendLine("namespace " + this.ModelFactory.Configuration.Namespace);
            code.AppendLine("{");

            // Only apply Data Annotation if configured to do so.
            if (!this.ModelFactory.Configuration.UseFluentApi)
                code.AppendLine(("[Table(\"" + table.Name + "\")]").Indent(1));

            code.AppendLine(("public partial class " + table.Name).Indent(1));
            code.AppendLine("{".Indent(1));

            code.AppendLine("#region Variables".Indent(2));
            code.AppendLine("#endregion".Indent(2));
            code.AppendLine("");

            code.AppendLine("#region Properties".Indent(2));
            foreach (var column in table.Columns)
            {
                foreach (var line in GenerateProperty(column).Split(new[] { "\r\n" }, StringSplitOptions.None))
                {
                    code.AppendLine(line.Indent(2));
                }
            }

            code.AppendLine("#endregion".Indent(2));
            code.AppendLine("");

            code.AppendLine("#region Constructors".Indent(2));
            code.AppendLine("#endregion".Indent(2));
            code.AppendLine("");

            code.AppendLine("#region Methods".Indent(2));
            code.AppendLine("#endregion".Indent(2));
            code.AppendLine("");

            code.AppendLine("#region Events".Indent(2));
            code.AppendLine("#endregion".Indent(2));

            code.AppendLine("}".Indent(2));
            code.AppendLine("}");

            return code.ToString();
        }

        private string GenerateContext(Fosol.Data.Models.Model model)
        {
            var code = new StringBuilder();
            code.AppendLine("using System.Data.Entity;");
            if (!string.IsNullOrEmpty(this.Namespace))
            {
                code.AppendLine("namespace " + this.ModelFactory.Configuration.Namespace + "{");
            }

            code.AppendLine("public partial class Context : DbContext {");

            code.AppendLine("#region Variables");
            code.AppendLine("#endregion");
            code.AppendLine("#region Properties");
            foreach (var table in model.Tables)
            {
                code.AppendLine("public DbSet<" + table.Name + "> " + table.Name + " { get; set; }");
            }
            code.AppendLine("#endregion");
            code.AppendLine("#region Constructors");
            code.AppendLine("#endregion");
            code.AppendLine("#region Methods");
            code.AppendLine("#endregion");
            code.AppendLine("#region Events");
            code.AppendLine("#endregion");

            code.AppendLine("}");

            if (!string.IsNullOrEmpty(this.Namespace))
                code.AppendLine("}");

            return code.ToString();
        }

        //[System.ComponentModel.DataAnnotations.Schema.Table("name")]
        //[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        //[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true, ErrorMessage = "")]
        //[System.ComponentModel.DefaultValue("value")]
        //[System.ComponentModel.DataAnnotations.Schema.Column("name")]
        //[System.ComponentModel.DataAnnotations.MaxLength(1, ErrorMessage = "")]
        //[System.ComponentModel.DataAnnotations.Key()]
        //[System.ComponentModel.DataAnnotations.Timestamp()]
        private string GenerateProperty(Models.Column column)
        {
            var prop = new StringBuilder();

            if (!this.ModelFactory.Configuration.UseFluentApi)
                prop.AppendLine("[Column(\"" + column.Name + "\")]");

            // This column is a primary key.
            if (column.IsPrimaryKey)
                prop.AppendLine("[Key]");

            // A column can only be computed OR identity if the database generates the value.
            if (column.IsComputed)
                prop.AppendLine("[DatabaseGenerated(DatabaseGeneratedOption.Computed)]");
            else if (column.IsIdentity)
                prop.AppendLine("[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");

            // This column is a timestamp.
            if (((SqlClient.SqlColumn)column).SqlDbType == System.Data.SqlDbType.Timestamp)
                prop.AppendLine("[Timestamp]");
            // Check if the column is required.
            // By default a Timestamp will be generated by the database, althouth it is not considered 'computed' or an 'identity'.
            else if (!column.IsNullable
                && !column.IsComputed
                && !column.IsIdentity)
            {
                if (column.NativeType == typeof(String))
                    prop.AppendLine("[Required(AllowEmptyStrings = true, ErrorMessage = \"" + column.Name + " cannot be null.\")]");
                else
                    prop.AppendLine("[Required(ErrorMessage = \"" + column.Name + " cannot be null.\")]");
            }

            // Provides a way to override the default behaviour of code first when auto-generating the database.
            if (column.IsPrimaryKey && !column.IsComputed && !column.IsIdentity)
                prop.AppendLine("[DatabaseGenerated(DatabaseGeneratedOption.None)]");

            // This column has a default value.
            var default_value = column.GetDefaultValue();
            if (default_value != null)
            {
                if (column.NativeType == typeof(String))
                    prop.AppendLine("[DefaultValue(\"" + default_value + "\")]");
                else
                    prop.AppendLine("[DefaultValue(" + default_value + ")]");
            }

            if (column.MaximumLength > 0)
                prop.AppendLine("[MaxLength(" + column.MaximumLength + ", ErrorMessage = \"" + column.Name + " cannot be longer than " + column.MaximumLength + " characters.\")]");

            prop.AppendLine("public " + column.NativeType.Name + " " + column.Name + " { get; set; }");

            return prop.ToString();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
