using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutCeiling
{
	public class Shape
	{
		public List<Point2> Points { get; set; }
		public bool IsCutout { get; set; }
		//Point2 scale, offset ??

		public Shape()
		{
			Points = new List<Point2>();
		}

		public Shape(List<Point2> points)
		{
			Points = new List<Point2>(points);
		}


	}
}
