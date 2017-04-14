

using System;
using System.Collections.Generic;
using System.Linq;

namespace LayoutCeiling
{
	public static class Geometry
	{
// 		// расстояние между точками
// 		public static double Distance(Point2 from, Point2 to)
// 		{
// 			return Math.Sqrt((from.X - to.X) * (from.X - to.X) + (from.Y - to.Y) * (from.Y - to.Y));
// 		}

		// пересечение отрезков ab и cd
		public static bool IntersectSegmentSegment(Point2 a, Point2 b, Point2 c, Point2 d)
		{
			double x43 = d.X - c.X,
				y13 = a.Y - c.Y,
				y43 = d.Y - c.Y,
				x13 = a.X - c.X,
				x21 = b.X - a.X,
				y21 = b.Y - a.Y;
			double zn = (y43 * x21 - x43 * y21);

			if (zn >= -double.Epsilon && zn <= double.Epsilon)	// прямые ||
				return false;

			double ua = (x43 * y13 - y43 * x13) / zn;
			double ub = (x21 * y13 - y21 * x13) / zn;

			return (ua > 0 && ua < 1 && ub > 0 && ub < 1);
		}

		// проекция точки на отрезок (возвращает false если проекция за пределами отрезка)
		public static bool ProjectionPointToSegment(Point2 p, Point2 a, Point2 b, ref Point2 result)
		{
			Point2 line = b - a;
			float t = Point2.DotProduct((p - a), line) / line.SquareLength();

			if (t < 0 || t > 1)	// точка за пределами отрезка
				return false;

			Point2 proj = a + t * line;
			if (result != null)
				result = proj;

			return true;
		}

		// находится ли точка внутри окружности
		public static bool PointInCircle(Point2 p, Point2 center, float radius)
		{
			return p.DistanceTo(center) < radius;
		}

		private static int IntersectRayXSegment(Point2 rayBegin, Point2 a, Point2 b)
		{
			Point2 aa = a - rayBegin;
			Point2 bb = b - rayBegin;

			if (aa.Y * bb.Y > 0)
				return 1;
			int s = Math.Sign(aa.X * bb.Y - aa.Y * bb.X);
			if (s == 0)
			{
				if (aa.X * bb.X <= 0)
					return 0;
				return 1;
			}
			if (aa.Y < 0)
				return -s;
			if (bb.Y < 0)
				return s;
			return 1;
		}

		public static bool PointInPolygon(Point2 p, List<Point2> polygon)
		{
			int count = polygon.Count();
			int result = 1; // точка снаружи

			for (int i = 0, j = 1; i < count; ++i, ++j)
			{
				j = j == count ? 0 : j;

				int check = IntersectRayXSegment(p, polygon[i], polygon[j]);
				if (check == 0)	
					return true;    //точка на ребре

				result *= check;
			}
			return result < 0;	//точка внутри 
		}
	}
}
