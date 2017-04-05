using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling
{
	public class Viewport : Panel
	{
		public enum DrawStyle { Normal, Selected, New, Error, Preview }

		private MainForm mainForm;
		//		Timer redrawTimer;

		private Bitmap backBuffer;
		private Rectangle backBufferRect;
		private Graphics g, graphics;

		Pen penNormal, penGrid, penSelectArea;
		Font fontLetter, fontLen;

		public float PointSize { set; get; }
		public float GridSize { set; get; }

		public float Zoom { set; get; }
		public Point2 Offset { set; get; }

		public Viewport(MainForm mainForm, Panel source)
			: base()
		{
			PointSize = 8;
			GridSize = 50;
			Zoom = 1;
			Offset = new Point2(0, 0);

			this.mainForm = mainForm;
			//			redrawTimer = new Timer();
			//			redrawTimer.Interval = 16; // ~ 60 кадров/сек
			// 			redrawTimer.Tick += new EventHandler(delegate (object o, EventArgs a)
			// 												{
			// 													Draw();
			// 												});
			//			redrawTimer.Start();

			BackColor = source.BackColor;
			BorderStyle = source.BorderStyle;
			Location = source.Location;
			Size = source.Size;
			TabIndex = source.TabIndex;

			MouseDown += OnMouseDown;
			MouseMove += OnMouseMove;
			MouseUp += OnMouseUp;
			MouseWheel += OnMouseWheel;

			graphics = Graphics.FromHwnd(Handle);
			backBuffer = new Bitmap(Width, Height, graphics);
			backBufferRect = new Rectangle(0, 0, Width, Height);
			g = Graphics.FromImage(backBuffer);

			penNormal = new Pen(Color.Black, 2);
			penGrid = new Pen(Color.FromArgb(240, 240, 255));
			fontLetter = new Font("Arial", 10);
			fontLen = new Font("Arial", 8);

			penSelectArea = new Pen(Color.FromArgb(128, Color.Black));
			penSelectArea.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
			penSelectArea.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

			//g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
		}

		private void OnMouseWheel(object sender, MouseEventArgs e)
		{
			Zoom += e.Delta * 0.1f;
		}

		public void Draw()
		{
			g.Clear(BackColor);
// 			g.ResetTransform();
// 			g.TranslateTransform(Offset.X, Offset.Y);
// 			g.ScaleTransform(Zoom, Zoom);

			DrawGrid();
			DrawLayout(mainForm.layout);
			if (mainForm.activeTool != null)
				mainForm.activeTool.DrawChagesPreview(g);

			g.ResetTransform();
			// info
			g.DrawString("select: " + mainForm.selection.indices.Count.ToString(), Font, Brushes.Gray, 2, 2);
			g.DrawString("P = " + mainForm.layout.Perimeter().ToString("#.#") + " см", Font, Brushes.Gray, 2, 22);
			g.DrawString(p.ToString(), Font, Brushes.Gray, 2, 42);
			g.DrawString("zoom: " + (Zoom * 100).ToString("#.#") + "%", Font, Brushes.Gray, 2, 62);

			g.Flush();
			graphics.DrawImage(backBuffer, backBufferRect);

			// undo stack
			mainForm.lbUndoStack.Items.Clear();
			foreach (var cmd in mainForm.undoStack.history)
			{
				mainForm.lbUndoStack.Items.Add(cmd.Text);
			}
			mainForm.lbUndoStack.SelectedIndex = mainForm.undoStack.Index();
		}

		Point2 p;

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			p = FromViewportSpace(e.Location);

			if (mainForm.activeTool != null)
				mainForm.activeTool.OnMouseMove(e, p);
			Draw();
		}

		private void OnMouseUp(object sender, MouseEventArgs e)
		{
			p = FromViewportSpace(e.Location);

			if (mainForm.activeTool != null)
				mainForm.activeTool.OnMouseUp(e, p);
			Draw();
		}

		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			p = FromViewportSpace(e.Location);

			if (mainForm.activeTool != null)
				mainForm.activeTool.OnMouseDown(e, p);
			Draw();
		}

		public void DrawPoint(Point2 point, DrawStyle style)
		{
			point = ToViewportSpace(point);

			float psz = PointSize/* / Zoom*/;

			switch (style)
			{
				case DrawStyle.Normal:
					g.FillRectangle(Brushes.Green, point.X - psz / 2, point.Y - psz / 2, psz, psz);
					break;
				case DrawStyle.Selected:
					g.FillRectangle(Brushes.DodgerBlue, point.X - psz * 1.3f / 2f, point.Y - psz * 1.3f / 2f, psz * 1.3f, psz * 1.3f);
					break;
				case DrawStyle.New:
				case DrawStyle.Preview:
					g.FillRectangle(Brushes.Gray, point.X - psz / 2, point.Y - psz / 2, psz, psz);
					break;
			}
		}

		public void DrawLine(Point2 p1, Point2 p2, DrawStyle style)
		{
			string len = p1.DistanceTo(p2).ToString("0.#");
			RectangleF lenRect = new RectangleF();
			lenRect.Size = g.MeasureString(len, fontLen);

			p1 = ToViewportSpace(p1);
			p2 = ToViewportSpace(p2);

			Point2 lenPos = new Point2(p1.X + (p2.X - p1.X) / 2 - lenRect.Size.Width / 2, p1.Y + (p2.Y - p1.Y) / 2 - lenRect.Size.Height / 2);
			lenRect.Location = lenPos.ToPointF();
			// 			lenRect.Width += 6;
			// 			lenRect.Height += 6;
			// 			lenRect.Offset(-3, -3);

			switch (style)
			{
				case DrawStyle.Normal:
					g.DrawLine(penNormal, p1.ToPointF(), p2.ToPointF());
					g.FillEllipse(Brushes.White, lenRect);
					g.DrawString(len, fontLen, Brushes.Black, lenPos.ToPointF());
					break;
				case DrawStyle.Error:
					g.DrawLine(Pens.Red, p1.ToPointF(), p2.ToPointF());
					g.FillEllipse(Brushes.White, lenRect);
					g.DrawString(len, fontLen, Brushes.Red, lenPos.ToPointF());
					break;
				case DrawStyle.Preview:
					g.DrawLine(Pens.LightGray, p1.ToPointF(), p2.ToPointF());
					g.FillEllipse(Brushes.White, lenRect);
					g.DrawString(len, fontLen, Brushes.Gray, lenPos.ToPointF());
					break;
			}
		}

		private void DrawLayout(CeilingLayout layout)
		{
			HashSet<int> xLines = new HashSet<int>();
			for (int i = 0; i < layout.points.Count - 1; ++i)
			{
				int j;
				for (j = i + 1; j < layout.points.Count - 1; ++j)
				{
					bool xi = false;
					if (Geometry.IntersectSegmentSegment(layout.points[i], layout.points[i + 1], layout.points[j], layout.points[j + 1]))
					{
						xi = true;
						xLines.Add(j);
					}
					if (xi)
						xLines.Add(i);
				}
				if (Geometry.IntersectSegmentSegment(layout.points[i], layout.points[i + 1], layout.points[j], layout.points[0]))
				{
					xLines.Add(i);
					xLines.Add(j);
				}
			}

			if (layout.points.Count > 2)
			{
				PointF[] pointsArray = new PointF[layout.points.Count];
				for (int i = 0; i < layout.points.Count; ++i)
				{
					pointsArray[i].X = layout.points[i].X * Zoom + Offset.X;
					pointsArray[i].Y = layout.points[i].Y * Zoom + Offset.Y;
				}
				g.FillPolygon(Brushes.White, pointsArray);
			}
			if (layout.points.Count > 1)
			{
				for (int i = 0; i < layout.points.Count - 1; ++i)
				{
					//g.DrawLine(Pens.Black, layout.points[i], layout.points[i + 1]);
					DrawLine(layout.points[i], layout.points[i + 1], xLines.Contains(i) ? DrawStyle.Error : DrawStyle.Normal);
				}
				//g.DrawLine(Pens.Black, layout.points.Last().X, layout.points.Last().Y, layout.points.First().X, layout.points.First().Y);
				DrawLine(layout.points.Last(), layout.points.First(), xLines.Contains(layout.points.Count-1) ? DrawStyle.Error : DrawStyle.Normal);
			}
			for (int i = 0; i < layout.points.Count; ++i)
			{
				Point2 p = layout.points[i];
				if (mainForm.selection.Contains(i))
				{
					DrawPoint(p, DrawStyle.Selected);
				}
				else
				{
					DrawPoint(p, DrawStyle.Normal);
				}

				p = ToViewportSpace(p);
				g.DrawString(CeilingLayout.PointLetter(i), fontLetter, Brushes.Black, p.X + 2, p.Y + 2);
			}
		}

		private void DrawGrid()
		{
 			float gsz = (GridSize * Zoom);
			float offx = Offset.X - (float)Math.Truncate(Offset.X / gsz) * gsz;
			float offy = Offset.Y - (float)Math.Truncate(Offset.Y / gsz) * gsz;

			for (float x = offx; x < Width; x += gsz)
			{
				g.DrawLine(penGrid, x, 0, x, Height);
			}
			for (float y = offy; y < Height; y += gsz)
			{
				g.DrawLine(penGrid, 0, y, Width, y);
			}
		}

		public void DrawPivot()
		{
			int x = (int)ToViewportSpace(mainForm.selection.Pivot).X;
			int y = (int)ToViewportSpace(mainForm.selection.Pivot).Y;

			if (mainForm.selection.indices.Count > 0)
			{
				g.DrawEllipse(Pens.Blue, x - 3, y - 3, 6, 6);
				g.DrawLine(Pens.Blue, x - 6, y, x + 6, y);
				g.DrawLine(Pens.Blue, x, y - 6, x, y + 6);
			}
		}

		public void DrawSelectArea(Point2 pos, Point2 size)
		{
			pos = ToViewportSpace(pos);
			size *= Zoom; 
			g.DrawRectangle(penSelectArea, pos.X, pos.Y, size.X, size.Y);
		}

		public Point2 ToViewportSpace(Point2 p)
		{
//			p -= Offset + new Point2(Width / 2f, Height / 2f);
			p *= Zoom;
			p += Offset;// + new Point2(Width / 2f, Height / 2f);
			return p;
		}

		public Point2 FromViewportSpace(Point p)
		{
			return new Point2((p.X - Offset.X/* + Width/2*/) / Zoom/* - Width/2 - Offset.X*/, (p.Y - Offset.Y/* + Height/2*/) / Zoom/* - Height/ 2 - Offset.X*/);
		}
	}
}
