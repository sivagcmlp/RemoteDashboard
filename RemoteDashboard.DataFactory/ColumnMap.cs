using System;
using System.Reflection;

namespace RemoteDashboard.DataFactory
{
	/// <summary>
	/// ColumnMap: Maps a column from the database.
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// 	<listheader><term>Change History</term><description>Description</description></listheader>
	///		<item><term>05/27/2005 SE</term><description>Created</description></item>
	/// </list>
	/// </remarks>
	public class ColumnMap
	{
		#region Properties

		/// <summary>
		/// Gets / Sets the name of this column
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		private string m_Name;

		/// <summary>
		/// Gets / Sets The attributes of this column
		/// </summary>
		public ColumnMapAttributes Attributes
		{
			get { return m_Attributes; }
			set { m_Attributes = value; }
		}
		private ColumnMapAttributes m_Attributes;

		/// <summary>
		/// Gets / Sets the target for this column's data
		/// </summary>
		public PropertyInfo Target
		{
			get { return m_Target; }
			set { m_Target = value; }
		}
		private PropertyInfo m_Target;

		/// <summary>
		/// Is this column a primary key
		/// </summary>
		public bool IsPrimaryKey
		{
			get { return (Attributes & ColumnMapAttributes.PrimaryKey) == ColumnMapAttributes.PrimaryKey; }
		}

		/// <summary>
		/// Is this column an identity
		/// </summary>
		public bool IsIdentity
		{
			get { return (Attributes & ColumnMapAttributes.Identity) == ColumnMapAttributes.Identity; }
		}

		/// <summary>
		/// Is this column a primary key and is an identity
		/// </summary>
		public bool IsPrimaryIdentity
		{
			get { return (Attributes & ColumnMapAttributes.PrimaryIdentity) == ColumnMapAttributes.PrimaryIdentity; }
		}

		#endregion
	}
}
