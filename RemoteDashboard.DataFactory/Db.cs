using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RemoteDashboard.DataFactory
{
	/// <summary>
	/// Db: Database Access object
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// <listheader><term>Change History</term><description>Description</description></listheader>
	/// <item><term>05/27/2005 SE</term><description>Created</description></item>
	/// </list>
	/// </remarks>
	public class Db : IDisposable
	{
		#region Member Variables

		private ArrayList m_Parameters = new ArrayList();
		private CommandType m_SqlType = CommandType.Text;
		private bool m_Disposed = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a Db object
		/// </summary>
		public Db()
		{
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets a command object based on the current connection string
		/// </summary>
		private IDbCommand GetCommand()
		{
			IDbCommand cmd = Connection.CreateCommand();
			cmd.CommandType = m_SqlType;
			cmd.CommandText = CommandText;

			Debug.WriteLine(CommandText);

			foreach (SqlParameter parm in m_Parameters)
			{
				cmd.Parameters.Add(parm);

				Debug.WriteLine(string.Format("{0} = {1}", parm.ParameterName, parm.Value));
			}

			return cmd;
		}

		/// <summary>
		/// Execute an update / delete / insert command against a database
		/// </summary>
		public int ExecuteNonQuery()
		{
			using (IDbCommand cmd = GetCommand())
			{
				return cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Execute a SQL statement against the database and return the first row, first column
		/// </summary>
		/// <returns></returns>
		public object GetScalar()
		{
			object obj;
			using (IDbCommand cmd = GetCommand())
			{
				obj = cmd.ExecuteScalar();
			}
			return obj;
		}

		/// <summary>
		///	Gets a datareader
		/// </summary>
		public IDataReader GetDataReader()
		{
			using (IDbCommand cmd = GetCommand())
			{
				return cmd.ExecuteReader();
			}
		}

		/// <summary>
		/// Get some records from the database
		/// </summary>
		public DataSet GetDataSet()
		{
			DataSet ds = new DataSet();
			using (IDbCommand cmd = GetCommand())
			{
				using (SqlDataAdapter da = new SqlDataAdapter((SqlCommand)cmd))
				{
					da.Fill(ds);
				}
			}
			return ds;
		}

		/// <summary>
		/// Get some records from the database
		/// </summary>
		public DataTable GetDataTable()
		{
			return GetDataSet().Tables[0];
		}

		/// <summary>
		/// Get some records from the database
		/// </summary>
		public DataView GetDataView(string filter, string sort)
		{
			return new DataView(
				GetDataTable(),
				filter,
				sort,
				DataViewRowState.CurrentRows
				);
		}

		/// <summary>
		/// Get a record from the database
		/// </summary>
		public DataRow GetDataRow()
		{
			DataTable table = GetDataTable();
			if (table.Rows.Count > 0)
			{
				return table.Rows[0];
			}
			return table.NewRow();
		}

		/// <summary>
		/// Add a parameter to a SQL statement
		/// </summary>
		/// <param name="name">Parameter Name</param>
		/// <param name="value">Parameter Value</param>
		public void AddParameter(string name, object val)
		{
			if (name.StartsWith("@") == false)
			{
				name = "@" + name;
			}
			SqlParameter p = new SqlParameter(name, val);
			m_Parameters.Add(p);
		}

		/// <summary>
		/// Clears any parameters already added to the command
		/// </summary>
		public void ClearParameters()
		{
			m_Parameters.Clear();
		}

		/// <summary>
		/// Close the database connection
		/// </summary>
		public void Close()
		{
			if (m_Disposed == true)
			{
				return;
			}

			if (m_Connection != null)
			{
				m_Connection.Close();
				m_Connection.Dispose();
			}
		}

		#endregion

		#region Interface Methods

		/// <summary>
		/// Dispose (close) of this database connection
		/// </summary>
		public void Dispose()
		{
			if (m_Disposed == true)
			{
				return;
			}
			//	Dispose and close the connection
			Close();
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
			m_Disposed = true;
		}

        #endregion

        #region Properties

        /// <summary>
        ///	Gets the connection string
        /// </summary>
        private static string ConnectionString { get; } = ConfigurationSettings.AppSettings["GCM.RemoteDashboard.ConnectionString"];

        /// <summary>
        ///	Gets the database connection
        /// </summary>
        private IDbConnection Connection
		{
			get
			{
				if (m_Connection == null)
				{
					m_Connection = new SqlConnection(ConnectionString);
					m_Connection.Open();
				}
				return m_Connection;
			}
		}
		private IDbConnection m_Connection;

		/// <summary>
		/// Gets / Sets the SQL statement to execute against the database
		/// </summary>
		public string CommandText
		{
			get { return m_CommandText; }
			set
			{
				m_CommandText = value;
				m_Parameters.Clear();

				if (m_CommandText.IndexOf(" ") < 0 && m_CommandText.IndexOf("\t") < 0)
				{
					m_SqlType = CommandType.StoredProcedure;
				}
				else
				{
					m_SqlType = CommandType.Text;
				}
			}
		}
		private string m_CommandText = "";

		#endregion
	}
}