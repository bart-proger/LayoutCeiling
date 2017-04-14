using System.Collections.Generic;
using System.Linq;

namespace LayoutCeiling
{
	public class CeilingLayout
	{
		public List<Shape> Shapes { get; set; }

		//TODO: удалять shape при удалении его последней точки
		//FIX: переделать все на shape'ы

		public CeilingLayout()
		{
			Shapes = new List<Shape>();
		}

		public static string PointLetter(int index)
		{
			index += 'A';
			int number = 0;
			while (index > 'Z')
			{
				index -= ('Z' - 'A' + 1);
				++number;
			}
			return ((char)index).ToString() + (number > 0 ? number.ToString() : "");
		}

		public float Perimeter()
		{
//FIX: расчет периметра
			float result = 0;
// 			int i = points.Count-1;
// 			for (int j = 0; j < points.Count; ++j)
// 			{
// 				result += points[i].DistanceTo(points[j]);
// 				i = j;
// 			}
			return result;
		}

// 		public Point2 PointsCenter(HashSet<int> indices)
// 		{
// 			Point2 center = new Point2(0, 0);
// 			if (indices.Count == 0)
// 				return center;
// 			if (indices.Count == 1)
// 			{
// 				center = points[indices.First()];
// 				return center;
// 			}
// 
// 			foreach (var i in indices)
// 			{
// 				center += points[i];
// 			}
// 			center /= indices.Count;
// 
// 			return center;
// 		}

		public Rect BoundingRect()
		{
			Rect result = new Rect(0, 0);

			if (Shapes.Count == 0)
				return result;

			Point2 min, max;
			min = max = Shapes.First().Points.First();

			foreach (var s in Shapes)
			{
				foreach (var p in s.Points)
				{
					if (p.X < min.X)
						min.X = p.X;
					if (p.Y < min.Y)
						min.Y = p.Y;
					if (p.X > max.X)
						max.X = p.X;
					if (p.Y > max.Y)
						max.Y = p.Y;
				}
			}
			result.Pos = min;
			result.Size = max - min;
			return result;
		}

		public bool IsEmpty()
		{
			//FIX: ?? учитывать cutout
			return Shapes.Count == 0;
		}

		public int ShapeIndexAtCoord(Point2 coord)
		{
			for(int i = Shapes.Count-1; i >= 0; --i)
			{
				if (Geometry.PointInPolygon(coord, Shapes[i].Points))
					return i;
			}
			return -1;
		}
	}
}
