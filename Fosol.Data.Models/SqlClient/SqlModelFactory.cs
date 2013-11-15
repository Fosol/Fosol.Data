using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.SqlClient
{
    /// <summary>
    /// An SqlModelFactory provides a way to build a data Model based on a Sql Server database.
    /// </summary>
    public sealed class SqlModelFactory
        : ModelFactory
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a SqlModelFactory object.
        /// </summary>
        /// <param name="connectionString">Connection string to the database.</param>
        public SqlModelFactory(string connectionString)
            : base("System.Data.SqlClient", connectionString)
        {
        }

        /// <summary>
        /// Creates a new instance of a SqlModelFactory object.
        /// </summary>
        /// <param name="connection">SqlConnection object.</param>
        public SqlModelFactory(SqlConnection connection)
            : base(connection)
        {
        }

        /// <summary>
        /// Creates a new instance of a SqlModelFactory object.
        /// </summary>
        /// <param name="config">DataModelElement object with configuration details.</param>
        public SqlModelFactory(Configuration.DataModelElement config)
            : base(config)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Extract the tables from the database and add them to the model.
        /// </summary>
        protected override EntityCollection BuildTables()
        {
            return BuildEntities(EntityType.Table);
        }

        /// <summary>
        /// Extract the views from the database and add them to the model.
        /// </summary>
        protected override EntityCollection BuildViews()
        {
            return BuildEntities(EntityType.View);
        }

        /// <summary>
        /// Extract the routines (stored procedures) from the database and add them to the model.
        /// </summary>
        protected override EntityCollection BuildRoutines()
        {
            return null;
        }

        /// <summary>
        /// Since Sql Server keeps both tables and views in the same way this method can perform both builds.
        /// </summary>
        /// <param name="entityType">The only relevant EntityType are Table and View.</param>
        private EntityCollection BuildEntities(EntityType entityType)
        {
            var connection = (SqlConnection)this.Connection;
            var entities = new EntityCollection();

            // Create a model for each table in the database.
            using (var cmd = entityType == EntityType.Table ? GetTables(connection) : GetViews(connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = (string)reader["TABLE_NAME"];
                        var entity = new Entity(name, (string)reader["TABLE_TYPE"]);
                        entity.Catalog = (string)reader["TABLE_CATALOG"];
                        entity.Schema = (string)reader["TABLE_SCHEMA"];
                        entities.Add(entity);
                    }
                }
            }

            // Foreach entity attach its columns.
            foreach (var entity in entities)
            {
                // Add the columns to the model.
                using (var cmd = GetColumns(entity.Name, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var column = new SqlColumn((string)reader["COLUMN_NAME"], (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), (string)reader["DATA_TYPE"], true), (int)reader["ORDINAL_POSITION"]);
                            //column.IsNullable = (bool)reader["IS_NULLABLE"];
                            if (!Convert.IsDBNull(reader["COLUMN_DEFAULT"]))
                                column.Default = (string)reader["COLUMN_DEFAULT"];
                            if (!Convert.IsDBNull(reader["CHARACTER_MAXIMUM_LENGTH"]))
                                column.MaximumLength = (int)reader["CHARACTER_MAXIMUM_LENGTH"];
                            if (!Convert.IsDBNull(reader["NUMERIC_PRECISION"]))
                                column.Precision = (byte)reader["NUMERIC_PRECISION"];
                            if (!Convert.IsDBNull(reader["NUMERIC_SCALE"]))
                                column.Scale = (int)reader["NUMERIC_SCALE"];

                            column.IsNullable = (int)reader["AllowsNull"] == 1;
                            column.IsComputed = (int)reader["IsComputed"] == 1;
                            if (!Convert.IsDBNull(reader["IsDeterministic"]))
                                column.IsDeterministic = (int)reader["IsDeterministic"] == 1;
                            column.IsIdentity = (int)reader["IsIdentity"] == 1;

                            entity.Columns.Add(column);
                        }
                    }
                }

                // Add the constraints to the model.
                using (var cmd = GetConstraints(entity.Name, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var column_name = (string)reader["COLUMN_NAME"];

                            // If the column doens't exist it means there was a failure importing it.
                            if (!entity.ContainsColumn(column_name))
                                throw new Exceptions.ModelFactoryException(string.Format("Invalid constraint: Column '{0}' wasn't imported.", column_name));

                            var column = entity.Columns[column_name];

                            var constraint_name = (string)reader["CONSTRAINT_NAME"];
                            var constraint_type = ModelFactory.ParseConstraintType((string)reader["CONSTRAINT_TYPE"]);

                            Constraint constraint;

                            // Check if the constraint already exists.  If it does it means it has multiple columns.
                            if (entity.ContainsConstraint(constraint_name))
                                constraint = entity.Constraints[constraint_name];
                            else
                            {
                                // The constraint doesn't exist yet.  Create a new constraint and add it to the model.
                                // If it's a foreign key it means there will be a record in the referential constraints.
                                constraint = constraint_type == ConstraintType.ForeignKey ? new ReferentialConstraint(constraint_name, constraint_type) : new Constraint(constraint_name, constraint_type);
                                entity.Constraints.Add(constraint);
                            }

                            // Some constraints use multiple columns.
                            constraint.Columns.Add(new ConstraintColumn((int)reader["ORDINAL_POSITION"], column));

                            // Reference the constraint in the column.
                            column.Constraints.Add(constraint);
                        }
                    }
                }

                foreach (ReferentialConstraint constraint in entity.Constraints.Where(c => c is ReferentialConstraint))
                {
                    // Update the referential constraints to include their relationship partner table.
                    using (var cmd = GetReferential(constraint.Name, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                constraint.SetParentConstraintName((string)reader["UNIQUE_CONSTRAINT_NAME"]);
                                constraint.UpdateRule = (string)reader["UPDATE_RULE"];
                                constraint.DeleteRule = (string)reader["DELETE_RULE"];
                            }
                        }
                    }

                    // Update the referential constraint to include the parent table name for the foreign relationship.
                    using (var cmd = GetConstraint(constraint.ParentConstraintName, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                constraint.ParentName = (string)reader["TABLE_NAME"];
                            }
                        }
                    }
                }
            }

            return entities;
        }

        /// <summary>
        /// Get a SqlCommand that will return all the entities within the database.
        /// </summary>
        /// <param name="connection">SqlConnection object.</param>
        /// <returns>A new instance of a SqlCommand.</returns>
        public static SqlCommand GetEntities(SqlConnection connection = null)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.Tables";
            if (connection == null)
                return new SqlCommand(sql);
            else
                return new SqlCommand(sql, connection);
        }

        /// <summary>
        /// Get a SqlCommand that will return all the tables within the database.
        /// </summary>
        /// <param name="connection">SqlConnection object.</param>
        /// <returns>A new instance of a SqlCommand.</returns>
        public static SqlCommand GetTables(SqlConnection connection = null)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.Tables WHERE Table_Type='BASE TABLE'";
            if (connection == null)
                return new SqlCommand(sql);
            else
                return new SqlCommand(sql, connection);
        }

        /// <summary>
        /// Get a SqlCommand that will return all the views within the database.
        /// </summary>
        /// <param name="connection">SqlConnection object.</param>
        /// <returns>A new instance of a SqlCommand.</returns>
        public static SqlCommand GetViews(SqlConnection connection = null)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.Tables WHERE Table_Type='VIEW'";
            if (connection == null)
                return new SqlCommand(sql);
            else
                return new SqlCommand(sql, connection);
        }

        /// <summary>
        /// Get a SqlCommand that will return all the routines (stored procedures) within the database.
        /// </summary>
        /// <param name="connection">SqlConnection object.</param>
        /// <returns>A new instance of a SqlCommand.</returns>
        public static SqlCommand GetRoutines(SqlConnection connection = null)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE'";
            if (connection == null)
                return new SqlCommand(sql);
            else
                return new SqlCommand(sql, connection);
        }

        /// <summary>
        /// Get a SqlCommand that will return all the columsn for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table you want the columns for.</param>
        /// <param name="connection">SqlConnection object.</param>
        /// <returns>A new instance of a SqlCommand.</returns>
        public static SqlCommand GetColumns(string tableName, SqlConnection connection = null)
        {
            var sql = "SELECT c.*, "
                + "COLUMNPROPERTY(OBJECT_ID('{0}'), c.COLUMN_NAME, 'AllowsNull') AS AllowsNull, "
                + "COLUMNPROPERTY(OBJECT_ID('{0}'), c.COLUMN_NAME, 'IsComputed') AS IsComputed, "
                + "COLUMNPROPERTY(OBJECT_ID('{0}'), c.COLUMN_NAME, 'IsDeterministic') AS IsDeterministic, "
                + "COLUMNPROPERTY(OBJECT_ID('{0}'), c.COLUMN_NAME, 'IsIdentity') AS IsIdentity "
                + "FROM INFORMATION_SCHEMA.Columns c "
                + "WHERE c.TABLE_NAME='{0}' "
                + "ORDER BY ORDINAL_POSITION ASC ";

            if (connection == null)
                return new SqlCommand(string.Format(sql, tableName));
            else
                return new SqlCommand(string.Format(sql, tableName), connection);
        }

        /// <summary>
        /// Get a SqlCommand that will return the constraint specified.
        /// </summary>
        /// <param name="constraintName">Name of the constraint to retrieve.</param>
        /// <param name="connection">SqlConnection object.</param>
        /// <returns>A new instance of a SqlCommand.</returns>
        public static SqlCommand GetConstraint(string constraintName, SqlConnection connection = null)
        {
            var sql = "SELECT kcu.COLUMN_NAME, kcu.CONSTRAINT_NAME, kcu.ORDINAL_POSITION, tc.* "
                + "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu "
                + "LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc ON kcu.TABLE_NAME = tc.TABLE_NAME AND kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME "
                + "WHERE kcu.CONSTRAINT_NAME = '{0}' ";
            if (connection == null)
                return new SqlCommand(string.Format(sql, constraintName));
            else
                return new SqlCommand(string.Format(sql, constraintName), connection);
        }

        /// <summary>
        /// Get a SqlCommand that will return all the constraints for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table to retrieve constraints for.</param>
        /// <param name="connection">SqlConnection object.</param>
        /// <returns>A new instance of a SqlCommand.</returns>
        public static SqlCommand GetConstraints(string tableName, SqlConnection connection)
        {
            return SqlModelFactory.GetConstraints(tableName, null, connection);
        }

        /// <summary>
        /// Get a SqlCommand that will return all the constraints for the specified table and column.
        /// </summary>
        /// <param name="tableName">Name of the table to retrieve constraints for.</param>
        /// <param name="columnName">Name of the column to retrieve constraints for.</param>
        /// <param name="connection">SqlConnection object.</param>
        /// <returns>A new instance of a SqlCommand.</returns>
        public static SqlCommand GetConstraints(string tableName, string columnName = null, SqlConnection connection = null)
        {
            string sql;
            
            if (columnName == null)
                sql = "SELECT kcu.COLUMN_NAME, kcu.CONSTRAINT_NAME, kcu.ORDINAL_POSITION, tc.* "
                    + "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu "
                    + "LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc ON kcu.TABLE_NAME = tc.TABLE_NAME AND kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME "
                    + "WHERE kcu.TABLE_NAME = '{0}' "
                    + "ORDER BY kcu.ORDINAL_POSITION ASC ";
            else
                sql = "SELECT kcu.COLUMN_NAME, kcu.CONSTRAINT_NAME, kcu.ORDINAL_POSITION, tc.* "
                    + "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu "
                    + "LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc ON kcu.TABLE_NAME = tc.TABLE_NAME AND kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME "
                    + "WHERE kcu.TABLE_NAME = '{0}' "
                    + " AND kcu.COLUMN_NAME='{1}' "
                    + "ORDER BY kcu.ORDINAL_POSITION ASC ";

            if (connection == null)
                return new SqlCommand(string.Format(sql, tableName, columnName));
            else
                return new SqlCommand(string.Format(sql, tableName, columnName), connection);
        }

        /// <summary>
        /// Get a SqlCommand that will return the referential constraint with the name specified.
        /// </summary>
        /// <param name="constraintName">Name of the constraint to identify the referential constraint.</param>
        /// <param name="connection">SqlConnection object.</param>
        /// <returns>A new instance of a SqlCommand.</returns>
        public static SqlCommand GetReferential(string constraintName, SqlConnection connection = null)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = '{0}'";
            if (connection == null)
                return new SqlCommand(string.Format(sql, constraintName));
            else
                return new SqlCommand(string.Format(sql, constraintName), connection);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
