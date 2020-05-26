using System;
using System.Reflection;
using System.Text;

namespace RemoteDashboard.DataFactory
{
	/// <summary>
	/// TableMap: Maps a table from the database
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// 	<listheader><term>Change History</term><description>Description</description></listheader>
	///		<item><term>05/27/2005 SE</term><description>Created</description></item>
	/// </list>
	/// </remarks>
	public class TableMap
	{
		#region Methods

		/// <summary>
		/// Creates a SQL Select Statement
		/// </summary>
		/// <returns></returns>
		private string GetSelectSQL()
		{
			StringBuilder sql = new StringBuilder();

			sql.Append("SELECT ");

			bool needsSeperator = false;

			foreach (ColumnMap map in Columns)
			{
				if (needsSeperator == true)
				{
					sql.Append(", ");
				}
				sql.Append(map.Name);

				needsSeperator = true;
			}

			sql.Append(" FROM " + Name);

			return sql.ToString();
		}

		/// <summary>
		/// Creates a SQL Select Statement for getting one row based on the primary keys
		/// </summary>
		/// <returns></returns>
		private string GetDetailsSQL()
		{
			StringBuilder sql = new StringBuilder();

			sql.Append("SELECT ");

			bool needsSeperator = false;

			foreach (ColumnMap map in Columns)
			{
				if (needsSeperator == true)
				{
					sql.Append(", ");
				}
				sql.Append(map.Name);

				needsSeperator = true;
			}

			sql.Append(" FROM " + Name);

			sql.Append(" WHERE ");

			needsSeperator = false;

			foreach (ColumnMap map in Columns)
			{
				if (map.IsPrimaryKey == true)
				{
					if (needsSeperator == true)
					{
						sql.Append(" AND ");
					}
					sql.AppendFormat("{0} = @{0}", map.Name);

					needsSeperator = true;
				}
			}

			return sql.ToString();
		}

		/// <summary>
		/// Creates a SQL Delete Statement to delete one row based on the primary keys
		/// </summary>
		/// <returns></returns>
		private string GetDeleteSQL()
		{
			StringBuilder sql = new StringBuilder();

			sql.Append("DELETE FROM " + Name);

			sql.Append(" WHERE ");

			bool needsSeperator = false;

			foreach (ColumnMap map in Columns)
			{
				if (map.IsPrimaryKey == true)
				{
					if (needsSeperator == true)
					{
						sql.Append(" AND ");
					}
					sql.AppendFormat("{0} = @{0}", map.Name);

					needsSeperator = true;
				}
			}

			return sql.ToString();
		}

		/// <summary>
		/// Creates a SQL Update Statement to update one row based on the primary keys
		/// </summary>
		/// <returns></returns>
		private string GetUpdateSQL()
		{
			StringBuilder sql = new StringBuilder();

			sql.Append("UPDATE " + Name + " SET ");

			bool needsSeperator = false;

			foreach (ColumnMap map in Columns)
			{
				if (map.IsPrimaryKey == false)
				{
					if (needsSeperator == true)
					{
						sql.Append(", ");
					}
					sql.AppendFormat("{0} = @{0}", map.Name);

					needsSeperator = true;
				}
			}

			sql.Append(" WHERE ");

			needsSeperator = false;

			foreach (ColumnMap map in Columns)
			{
				if (map.IsPrimaryKey == true)
				{
					if (needsSeperator == true)
					{
						sql.Append(" AND ");
					}
					sql.AppendFormat("{0} = @{0}", map.Name);

					needsSeperator = true;
				}
			}

			return sql.ToString();
		}

		/// <summary>
		/// Creates a SQL Insert Statement to insert one row
		/// </summary>
		/// <returns></returns>
		private string GetInsertSQL()
		{
			StringBuilder sql = new StringBuilder();

			sql.Append("INSERT INTO " + Name + " ( ");

			bool needsSeperator = false;

			foreach (ColumnMap map in Columns)
			{
				if (map.IsIdentity == false)
				{
					if (needsSeperator == true)
					{
						sql.Append(", ");
					}
					sql.Append(map.Name);

					needsSeperator = true;
				}
			}

			sql.Append(" ) VALUES ( ");

			needsSeperator = false;

			foreach (ColumnMap map in Columns)
			{
				if (map.IsIdentity == false)
				{
					if (needsSeperator == true)
					{
						sql.Append(", ");
					}
					sql.Append("@" + map.Name);

					needsSeperator = true;
				}
			}

			sql.Append(" )");

			return sql.ToString();
		}

		/// <summary>
		/// Gets the newly created identity value
		/// </summary>
		/// <returns></returns>
		private string GetIdentitySQL()
		{
			return "SELECT IDENT_CURRENT('" + Name + "')";
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the column of this table that is both a primary key and an identity (or null)
		/// </summary>
		public ColumnMap PrimaryIdentityColumn
		{
			get { return m_PrimaryIdentityColumn; }
			set { m_PrimaryIdentityColumn = value; }
		}
		private ColumnMap m_PrimaryIdentityColumn;

		/// <summary>
		/// Gets / Sets the name of this table
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		private string m_Name;

		/// <summary>
		/// Gets the collection of columns within this table
		/// </summary>
		public ColumnMapCollection Columns
		{
			get
			{
				if (m_Columns == null)
				{
					m_Columns = new ColumnMapCollection();
				}
				return m_Columns;
			}
		}
		private ColumnMapCollection m_Columns;

		/// <summary>
		/// Gets a SQL Select Statement
		/// </summary>
		public string SelectSQL
		{
			get
			{
				if (m_SelectSQL == null)
				{
					m_SelectSQL = GetSelectSQL();
				}
				return m_SelectSQL;
			}
		}
		private string m_SelectSQL;

		/// <summary>
		/// Gets a SQL Select Statement for one row based on the primary keys
		/// </summary>
		public string DetailsSQL
		{
			get
			{
				if (m_DetailsSQL == null)
				{
					m_DetailsSQL = GetDetailsSQL();
				}
				return m_DetailsSQL;
			}
		}
		private string m_DetailsSQL;

		/// <summary>
		/// Gets a SQL Delete Statement for one row based on the primary keys
		/// </summary>
		public string DeleteSQL
		{
			get
			{
				if (m_DeleteSQL == null)
				{
					m_DeleteSQL = GetDeleteSQL();
				}
				return m_DeleteSQL;
			}
		}
		private string m_DeleteSQL;

		/// <summary>
		/// Gets a SQL Update Statement for one row based on the primary keys
		/// </summary>
		public string UpdateSQL
		{
			get
			{
				if (m_UpdateSQL == null)
				{
					m_UpdateSQL = GetUpdateSQL();
				}
				return m_UpdateSQL;
			}
		}
		private string m_UpdateSQL;

		/// <summary>
		/// Gets a SQL Insert Statement for one row
		/// </summary>
		public string InsertSQL
		{
			get
			{
				if (m_InsertSQL == null)
				{
					m_InsertSQL = GetInsertSQL();
				}
				return m_InsertSQL;
			}
		}
		private string m_InsertSQL;

		/// <summary>
		/// Gets a SQL Statement to get the latest identity value created for this table
		/// </summary>
		public string IdentitySQL
		{
			get
			{
				if (m_IdentitySQL == null)
				{
					m_IdentitySQL = GetIdentitySQL();
				}
				return m_IdentitySQL;
			}
		}
		private string m_IdentitySQL;

		#endregion
	}
}
