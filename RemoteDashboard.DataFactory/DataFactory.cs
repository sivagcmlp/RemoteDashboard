using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace RemoteDashboard.DataFactory
{
	/// <summary>
	/// DataFactory: Create SQL statements based on the table mappings
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// 	<listheader><term>Change History</term><description>Description</description></listheader>
	///		<item><term>05/27/2005 SE</term><description>Created</description></item>
	/// </list>
	/// </remarks>
	public class DataFactory
	{
		/// <summary>
		/// Finds a record in the database given the data object type
		/// </summary>
		/// <param name="type">The type of the dataobject to generate SQL statements from</param>
		/// <param name="filterClause">A SQL where clause without the WHERE</param>
		/// <param name="parameters">The parameter values to populate the WHERE clause with</param>
		/// <returns></returns>
		public static object Find(Type type, string filterClause, params object[] parameters)
		{
			using (Db db = new Db())
			{
				TableMap table = DataMap.Instance[type];

				db.CommandText = table.SelectSQL + " WHERE " + filterClause;

				AddParameters(db, filterClause, parameters);

				DataRow dr = db.GetDataRow();

				if (dr.RowState == DataRowState.Detached)
				{
					return null;
				}

				object data = Assembly.GetAssembly(type).CreateInstance(type.FullName);
				foreach (ColumnMap col in table.Columns)
				{
					col.Target.SetValue(data, dr[col.Name], null);
				}
				return data;
			}
		}

		/// <summary>
		/// Finds all of the records in the database given the data object type
		/// </summary>
		/// <param name="type">The type of the dataobject to generate SQL statements from</param>
		/// <param name="filterClause">A SQL where clause without the WHERE</param>
		/// <param name="orderClause">The order clause without the ORDER BY</param>
		/// <param name="parameters">The parameter values to populate the WHERE clause with</param>
		/// <returns></returns>
		public static object FindAll(Type type, string filterClause, string orderClause, params object[] parameters)
		{
			using (Db db = new Db())
			{
				TableMap table = DataMap.Instance[type];

				db.CommandText = table.SelectSQL + " WHERE " + filterClause + " ORDER BY " + orderClause;

				AddParameters(db, filterClause, parameters);

				DataTable dt = db.GetDataTable();
				ArrayList list = new ArrayList();
				
				foreach (DataRow dr in dt.Rows)
				{
					object data = Assembly.GetAssembly(type).CreateInstance(type.FullName);
					foreach (ColumnMap col in table.Columns)
					{
						col.Target.SetValue(data, dr[col.Name], null);
					}
					list.Add(data);
				}

				return list;
			}
		}

		/// <summary>
		/// Adds the parameters to the command object
		/// </summary>
		/// <param name="db">DB Connection object</param>
		/// <param name="filterClause">A SQL where clause without the WHERE</param>
		/// <param name="parameters">The parameter values to populate the WHERE clause with</param>
		private static void AddParameters(Db db, string filterClause, object[] parameters)
		{
			StringCollection parameterNames = GetParameterNames(filterClause);
			Queue parms = new Queue(parameters);
			foreach (string name in parameterNames)
			{
				db.AddParameter(name, parms.Dequeue());
			}
		}

		/// <summary>
		/// Gets a list of parameters names from the where / filter clause
		/// </summary>
		/// <param name="filterClause">A SQL where clause without the WHERE</param>
		/// <returns></returns>
		private static StringCollection GetParameterNames(string filterClause)
		{
			StringCollection names = new StringCollection();
			foreach (string piece in Regex.Split(filterClause, @"( |\t|\n|\r)+", RegexOptions.Compiled))
			{
				if (piece.StartsWith("@") == true)
				{
					names.Add(piece);
				}
			}
			return names;
		}

		/// <summary>
		/// Gets a record in the database given the data object type, and primary key values
		/// </summary>
		/// <param name="type">The type of the dataobject to generate SQL statements from</param>
		/// <param name="primaryKeys">The primary key values to populate the WHERE clause with</param>
		/// <returns></returns>
		public static object Get(Type type, params object[] primaryKeys)
		{
			using (Db db = new Db())
			{
				TableMap table = DataMap.Instance[type];

				db.CommandText = table.DetailsSQL;

				Queue parms = new Queue(primaryKeys);
				foreach (ColumnMap col in table.Columns)
				{
					if (col.IsPrimaryKey == true)
					{
						db.AddParameter(col.Name, parms.Dequeue());
					}
				}

				DataRow dr = db.GetDataRow();

				if (dr.RowState == DataRowState.Detached)
				{
					return null;
				}

				object data = Assembly.GetAssembly(type).CreateInstance(type.FullName);
				foreach (ColumnMap col in table.Columns)
				{
					col.Target.SetValue(data, dr[col.Name], null);
				}
				return data;
			}
		}


		/// <summary>
		/// Saves the data object to the database
		/// </summary>
		/// <param name="Save">Data Object to be saved</param>
		/// <returns></returns>
		public static void Save(object data)
		{
			TableMap table = DataMap.Instance[data.GetType()];
			if (Update(table, data) == 0)
			{
				Insert(table, data);
			}
		}

		/// <summary>
		/// Inserts a new record into the databsae
		/// </summary>
		/// <param name="table">Table map to map the object with</param>
		/// <param name="data">The Data Object to save</param>
		private static void Insert(TableMap table, object data)
		{
			using (Db db = new Db())
			{
				db.CommandText = table.InsertSQL;

				foreach (ColumnMap col in table.Columns)
				{
					if (col.IsIdentity == false)
					{
						db.AddParameter(
							col.Name,
							col.Target.GetValue(data, null)
							);
					}
				}

				db.ExecuteNonQuery();

				if (table.PrimaryIdentityColumn != null)
				{
					db.CommandText = table.IdentitySQL;

					table.PrimaryIdentityColumn.Target.SetValue(data, Convert.ToInt32(db.GetScalar()), null);
				}
			}
		}

		/// <summary>
		/// Updates a record in the databsae
		/// </summary>
		/// <param name="table">Table map to map the object with</param>
		/// <param name="data">The Data Object to save</param>
		/// <returns>Number of rows affected</returns>
		private static int Update(TableMap table, object data)
		{
			using (Db db = new Db())
			{
				db.CommandText = table.UpdateSQL;

				foreach (ColumnMap col in table.Columns)
				{
					db.AddParameter(
						col.Name,
						col.Target.GetValue(data, null)
						);
				}

				return db.ExecuteNonQuery();
			}
		}


		/// <summary>
		/// Deletes a record in the databsae
		/// </summary>
		/// <param name="data">The Data Object to delete</param>
		public static void Delete(object data)
		{
			TableMap table = DataMap.Instance[data.GetType()];
			using (Db db = new Db())
			{
				db.CommandText = table.DeleteSQL;

				foreach (ColumnMap col in table.Columns)
				{
					if (col.IsPrimaryKey == true)
					{
						db.AddParameter(
							col.Name,
							col.Target.GetValue(data, null)
							);
					}
				}

				db.ExecuteNonQuery();
			}
		}
	}
}
