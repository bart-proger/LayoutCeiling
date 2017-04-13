using System.Collections.Generic;

namespace LayoutCeiling
{
	public class Selection
	{
		//TODO: выделение shap'ов
		public int ShapeIndex { get; private set; }
		public HashSet<int> PointsIndices { get; }
		public Point2 Pivot { get; set; }

		public Selection()
		{
			ShapeIndex = -1;
			PointsIndices = new HashSet<int>();
		}

		public void SelectShape(int index)
		{
			ShapeIndex = index;
		}

		public void UnselectShape()
		{
			ShapeIndex = -1;
		}

		public void SelectPoints(HashSet<int> indices)
		{
			PointsIndices.UnionWith(indices);
		}

		public void UnselectPoints(HashSet<int> indices)
		{
			PointsIndices.ExceptWith(indices);
		}

		public void UnselectAllPoints()
		{
			PointsIndices.Clear();
		}

		public bool Contains(int index)
		{
			return this.PointsIndices.Contains(index);
		}

		public bool ContainsPoints()
		{
			return PointsIndices.Count == 0;
		}

		public bool ContainsShape()
		{
			return ShapeIndex > -1;
		}
	}
}
