using System;
using System.Collections;

namespace RemoteDashboard.DataFactory
{
	/// <summary>
	/// ColumnMapCollection: Column Map Collection
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// 	<listheader><term>Change History</term><description>Description</description></listheader>
	///		<item><term>05/27/2005 SE</term><description>Created</description></item>
	/// </list>
	/// </remarks>
	public class ColumnMapCollection : CollectionBase 
	{
		#region Constructor

		#endregion

		#region Methods

		/// <summary>
		/// Adds a Column Map to the list
		/// </summary>
		/// <param name="columnMap"></param>
		public void Add(ColumnMap columnMap)
		{
			List.Add(columnMap);
		}

		/// <summary>
		/// Removes a Column Map from the list
		/// </summary>
		/// <param name="columnMap"></param>
		public void Remove(ColumnMap columnMap)
		{
			List.Remove(columnMap);
		}

		#endregion

		#region Indexer

		/// <summary>
		/// Gets the Column Map at the selected index
		/// </summary>
		public ColumnMap this[int index]
		{
			get { return (ColumnMap) List[index]; }
		}

		#endregion
	}
}
