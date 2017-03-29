using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutCeiling
{
	public class Selection
	{
		public HashSet<int> indices;
		Point2 pivot;

		public Selection()
		{
			this.indices = new HashSet<int>();
		}

		public void SelectPoints(HashSet<int> indices)
		{
			this.indices.UnionWith(indices);
		}

		public void UnselectPoints(HashSet<int> indices)
		{
			this.indices.ExceptWith(indices);
		}

		public void UnselectAll()
		{
			this.indices.Clear();
		}

		public bool Contains(int index)
		{
			return this.indices.Contains(index);
		}

		public bool IsEmpty()
		{
			return this.indices.Count == 0;
		}
	}
}
