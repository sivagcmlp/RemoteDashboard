using System;

namespace RemoteDashboard.DataFactory
{
	/// <summary>
	/// ColumnMapAttributes: Maps column attributes from the database
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// 	<listheader><term>Change History</term><description>Description</description></listheader>
	///		<item><term>05/27/2005 SE</term><description>Created</description></item>
	/// </list>
	/// </remarks>
	[Flags]
	public enum ColumnMapAttributes
	{
		/// <summary>
		/// None
		/// </summary>
		None			= 0,
		/// <summary>
		/// Primary Key
		/// </summary>
		PrimaryKey		= 1,
		/// <summary>
		/// Identity
		/// </summary>
		Identity		= 2,
		/// <summary>
		/// Primary Key &amp; Identity
		/// </summary>
		PrimaryIdentity = PrimaryKey | Identity
	}
}
