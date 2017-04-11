using System.Collections.Generic;

namespace LayoutCeiling
{
	public class Selection
	{
		//TODO: выделение shap'ов
		public HashSet<int> indices;
		public Point2 Pivot { get; set; }

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
