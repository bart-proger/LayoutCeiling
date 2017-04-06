using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutCeiling
{
	public class CeilingLayout
	{
		/*private MainForm mainForm;*/

		public List<Point2> points;

		public CeilingLayout(/*MainForm mainForm*/)
		{
			/*this.mainForm = mainForm;*/
			points = new List<Point2>();
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
	}
}
