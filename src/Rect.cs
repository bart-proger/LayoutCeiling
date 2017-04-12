using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutCeiling
{
	public struct Rect
	{
		private Point2 pos_, size_;

		public Point2 Pos { get { return pos_; } set { pos_ = value; } }
		public Point2 Size { get { return size_; } set { size_ = value; } }

		public float X { get { return pos_.X; } set { pos_.X = value; } }
		public float Y { get { return pos_.Y; } set { pos_.Y = value; } }
		public float Width { get { return size_.X; } set { size_.X = value; } }
		public float Height { get { return size_.Y; } set { size_.Y = value; } }

		public Rect(float x, float y, float width, float height)
		{
			pos_ = new Point2(x, y);
			size_ = new Point2(width, height);
		}

		public Rect(float width, float height)
		{
			pos_ = new Point2(0, 0);
			size_ = new Point2(width, height);
		}

		public Rect(Point2 pos, Point2 size)
		{
			pos_ = pos;
			size_ = size;
		}
	}
}
