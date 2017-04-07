using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutCeiling
{
	public struct Point2
	{
		private float _x, _y;

		public float X { set { _x = value; } get { return _x; } }
		public float Y { set { _y = value; } get { return _y; } }

		public Point2(float x = 0, float y = 0)
		{
			_x = x;
			_y = y;
		}

		public Point2(float angle)
		{
			_x = (float)Math.Cos(angle);
			_y = (float)Math.Sin(angle);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Point2 p = (Point2)obj;

			return (_x == p._x && _y == p._y);
		}

		public override int GetHashCode()
		{
			return ShiftAndWrap(_x.GetHashCode(), 2) ^ _y.GetHashCode();
		}

		private int ShiftAndWrap(int value, int positions)
		{
			positions = positions & 0x1F;
			uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
			uint wrapped = number >> (32 - positions);

			return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
		}

		public static bool operator ==(Point2 a, Point2 b)
		{
			return (a._x == b._x && a._y == b._y);
		}

		public static bool operator !=(Point2 a, Point2 b)
		{
			return (a._x != b._x || a._y != b._y);
		}

		public static Point2 operator -(Point2 a)
		{
			return new Point2(-a._x, -a._y);
		}

		public static Point2 operator+(Point2 a, Point2 b)
		{
			return new Point2(a._x + b._x, a._y + b._y);
		}

		public static Point2 operator-(Point2 a, Point2 b)
		{
			return new Point2(a._x - b._x, a._y - b._y);
		}

		public static Point2 operator*(Point2 p, float f)
		{
			return new Point2(p._x * f, p._y * f);
		}

		public static Point2 operator *(float f, Point2 p)
		{
			return new Point2(p._x * f, p._y * f);
		}

		public static Point2 operator /(Point2 p, float d)
		{
			return new Point2(p._x / d, p._y / d);
		}

		public static float Distance(Point2 a, Point2 b)
		{
			return (float)Math.Sqrt((a._x - b._x) * (a._x - b._x) + (a._y - b._y) * (a._y - b._y));
		}

		public float DistanceTo(Point2 p)
		{
			return Distance(this, p);
		}

		public float Length()
		{
			return (float)Math.Sqrt(SquareLength());
		}

		public float SquareLength()
		{
			return (_x * _x + _y * _y);
		}

		public Point2 Normalized()
		{
			float len = Length();
			return new Point2(_x / len, _y / len);
		}

		public Point2 Normalize()
		{
			Point2 n = Normalized();
			_x = n._x;
			_y = n._y;
			return this;
		}

		public float Angle()
		{
			return (float)Math.Atan2(_y, _x);
		}

		public static float DotProduct(Point2 a, Point2 b)
		{
			return (a._x * b._x + a._y * b._y);
		}

		public static Point2 FromPoint(System.Drawing.Point p)
		{
			return new Point2(p.X, p.Y);
		}

		public System.Drawing.Point ToPoint()
		{
			return new System.Drawing.Point((int)_x, (int)_y);
		}

		public System.Drawing.PointF ToPointF()
		{
			return new System.Drawing.PointF(_x, _y);
		}

		public override string ToString()
		{
			return String.Format("({0:0.#}; {1:0.#})", _x, _y);
		}
	}
}
