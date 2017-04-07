using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Tools
{
	public class AddPolygon : Tool
	{
		//TODO: undo/redo cmd

		Cursor cursorPen;
		Cursor cursorPenFinish;
		Point2 newPoint;
		List<Point2> polygon;

		bool finish;

		public AddPolygon(MainForm form) : base(form, "Многоугольник")
		{
			cursorPen = CustomCursor.Create("data/cursors/pen.cur");
			cursorPenFinish = CustomCursor.Create("data/cursors/penFinish.cur");

			ToolStripItem = new ToolStripRadioButton();

			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = name;
			toolButton.ToolTipText = hint;
			toolButton.Image = LayoutCeiling.Properties.Resources.tool_addPolygon;
			toolButton.Click += OnToolClick;
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

			polygon = new List<Point2>();
			finish = false;
		}

		public override void OnMouseMove(MouseEventArgs e, Point2 p)
		{
			newPoint = p;

			if (polygon.Count > 2 && p.DistanceTo(polygon.First()) <= mainForm.viewport.PointSize / mainForm.viewport.Zoom)
			{
				mainForm.viewport.Cursor = cursorPenFinish;
				finish = true;
			}
			else
			{
				mainForm.viewport.Cursor = cursorPen;
				finish = false;
			}
		}

		public override void OnMouseUp(MouseEventArgs e, Point2 p)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (finish)
				{
					ApplyChanges();
					mainForm.selection.UnselectAll();
					DeactivateTool();
				}
				else
				{
					polygon.Add(newPoint);
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				DeactivateTool();
			}
		}

		public override void ActivateTool()
		{
			base.ActivateTool();
			mainForm.viewport.Cursor = cursorPen;
		}

		public override void DeactivateTool()
		{
			polygon.Clear();
			base.DeactivateTool();
		}

		public override void ApplyChanges()
		{
			mainForm.layout.points.Clear();
			mainForm.layout.points.AddRange(polygon);
		}

		public override void DrawChagesPreview(Graphics g)
		{
			if (polygon.Count > 0)
			{
				for (int i = 0; i < polygon.Count - 1; ++i)
				{
					//g.DrawLine(Pens.Green, polygon[i].X, polygon[i].Y, polygon[i + 1].X, polygon[i + 1].Y);
					mainForm.viewport.DrawLine(polygon[i], polygon[i+1], Viewport.DrawStyle.Preview);
				}
				//g.DrawLine(Pens.Green, polygon.Last().X, polygon.Last().Y, newPoint.X, newPoint.Y);
				mainForm.viewport.DrawLine(polygon.Last(), newPoint, Viewport.DrawStyle.Preview);

				//g.FillEllipse(Brushes.Gray, newPoint.X - mainForm.viewport.PointSize / 2, newPoint.Y - mainForm.viewport.PointSize / 2, mainForm.viewport.PointSize, mainForm.viewport.PointSize);
			}

			foreach (var p in polygon)
			{
				//g.FillEllipse(Brushes.Green, p.X - mainForm.viewport.PointSize / 2, p.Y - mainForm.viewport.PointSize / 2, mainForm.viewport.PointSize, mainForm.viewport.PointSize);
				mainForm.viewport.DrawPoint(p, Viewport.DrawStyle.Preview);
			}
		}
	}
}
