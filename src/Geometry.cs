

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
	}
}
