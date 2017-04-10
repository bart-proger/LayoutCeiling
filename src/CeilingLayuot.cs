using System.Collections.Generic;
using System.Linq;

namespace LayoutCeiling
{
	public class CeilingLayout
	{
		public List<Shape> Shapes { get; set; }
		public List<Point2> points;
		public List<Point2> cutout;

		public CeilingLayout()
		{
			Shapes = new List<Shape>();
			points = new List<Point2>();
			cutout = new List<Point2>();
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
			float result = 0;
			int i = points.Count-1;
			for (int j = 0; j < points.Count; ++j)
			{
				result += points[i].DistanceTo(points[j]);
				i = j;
			}
			return result;
		}

		public Point2 PointsCenter(HashSet<int> indices)
		{
			Point2 center = new Point2(0, 0);
			if (indices.Count == 0)
				return center;
			if (indices.Count == 1)
			{
				center = points[indices.First()];
				return center;
			}

			foreach (var i in indices)
			{
				center += points[i];
			}
			center /= indices.Count;

			return center;
		}

		public Point2 CenterBBox()
		{
			Point2 center = new Point2(0, 0);
			if (points.Count == 0)
				return center;
			if (points.Count == 1)
			{
				center = points.First();
				return center;
			}

			Point2 min, max;
			min = max = points.First();
			foreach (var p in points)
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
			center = (min + max) / 2;

			return center;
		}

		public float Width()
		{
			if (points.Count == 0)
				return 0f;

			float min, max;
			min = max = points.First().X;

			foreach (var p in points)
			{
				if (min > p.X)
					min = p.X;
				if (max < p.X)
					max = p.X;
			}

			return max - min;
		}

		public float Height()
		{
			if (points.Count == 0)
				return 0f;

			float min, max;
			min = max = points.First().Y;

			foreach (var p in points)
			{
				if (min > p.Y)
					min = p.Y;
				if (max < p.Y)
					max = p.Y;
			}

			return max - min;
		}
	}
}
