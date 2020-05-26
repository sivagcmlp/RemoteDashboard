using System;
using System.Data;
using System.Collections;
using System.Reflection;

namespace RemoteDashboard.DataFactory
{
	/// <summary>
	/// DataMap: Maps data objects to relational SQL tables
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// 	<listheader><term>Change History</term><description>Description</description></listheader>
	///		<item><term>05/27/2005 SE</term><description>Created</description></item>
	/// </list>
	/// </remarks>
	public class DataMap
	{
		#region Member Variables

		/// <summary>
		/// Gets a singleton instance of the data map
		/// </summary>
		public static readonly DataMap Instance = new DataMap();
		
		/// <summary>
		/// List of table maps
		/// </summary>
		private Hashtable m_TableMaps = new Hashtable();

		#endregion

		#region Constructor

		#endregion

		/// <summary>
		/// Creates a map based on an object
		/// </summary>
		private void Add(Type type)
		{
			using (Db db = new Db())
			{
				db.CommandText = "SELECT " +
					"   obj.name AS tablename," +
					"   obj.xtype," +
					"	col.name AS columnname," +
					" 	col.length AS columnlength," +
					" 	type.name AS typename," +
					" 	CASE WHEN col.status & 0x80 != 0 THEN 'Y' ELSE 'N' END AS isidentity," +
					" 	CASE WHEN keys.id IS NULL THEN 'N' ELSE 'Y' END AS iskey" +
					" FROM	sysobjects obj" +
					" 	INNER JOIN syscolumns col ON obj.id = col.id" +
					" 	INNER JOIN systypes type ON col.usertype = type.usertype" +
					" 	LEFT JOIN sysindexes idx ON obj.id = idx.id" +
					" 		AND idx.indid = 1" +
					" 	LEFT JOIN sysindexkeys keys ON idx.id = keys.id" +
					" 		AND idx.indid = keys.indid" +
					" 		AND keys.id = col.id" +
					" 		AND keys.colid = col.colid" +
					" WHERE obj.xtype IN ('U', 'V')" +
					"   AND obj.name = @tablename" +
					" ORDER BY obj.name, col.colid";
				db.AddParameter("tablename", type.Name);
				using (DataSet ds = db.GetDataSet())
				{
					Add(ds.Tables[0], type);
				}
			}
		}

		/// <summary>
		/// Create the table map
		/// </summary>
		/// <param name="dt">Datatable that contains SQL Information about a selected table</param>
		/// <param name="type">Object to map</param>
		private void Add(DataTable dt, Type type)
		{
			TableMap table = new TableMap();
			table.Name = type.Name;

			foreach (PropertyInfo p in type.GetProperties())
			{
				DataRow[] dr = dt.Select(string.Format("tablename = '{0}' AND columnname = '{1}'", table.Name, p.Name));

				if (dr.Length > 0)
				{				
					ColumnMap cm = new ColumnMap();
					cm.Name = p.Name;
					cm.Target = p;
					if (Convert.ToString(dr[0]["iskey"]) == "Y")
					{
						cm.Attributes |= ColumnMapAttributes.PrimaryKey;
					}
					if (Convert.ToString(dr[0]["isidentity"]) == "Y")
					{
						cm.Attributes |= ColumnMapAttributes.Identity;
					}
					table.Columns.Add(cm);
				}
			}

			foreach (ColumnMap col in table.Columns)
			{
				if ((col.Attributes & ColumnMapAttributes.PrimaryIdentity) == ColumnMapAttributes.PrimaryIdentity)
				{
					table.PrimaryIdentityColumn = col;
					break;
				}
			}

			m_TableMaps[type] = table;
		}

		/// <summary>
		/// Gets a table map based on the type, or adds it if it does not exist
		/// </summary>
		public TableMap this[Type type]
		{
			get 
			{
				if (m_TableMaps[type] == null)
				{
					Add(type);
				}
				return (TableMap) m_TableMaps[type]; 
			}
		}
	}
}
