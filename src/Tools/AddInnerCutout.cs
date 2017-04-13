using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LayoutCeiling.Tools
{
	public class AddInnerCutout : Tool
	{
		//TODO: undo/redo cmd

		Cursor cursorPen;
		Cursor cursorPenFinish;
		Point2 newPoint;
		List<Point2> cutout;

		bool finish;

		public AddInnerCutout(MainForm form) : base(form, "Внутренний вырез")
		{
			cursorPen = CustomCursor.Create("data/cursors/pen.cur");
			cursorPenFinish = CustomCursor.Create("data/cursors/penFinish.cur");

			ToolStripItem = new ToolStripRadioButton();

			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = name;
			toolButton.ToolTipText = hint;
			toolButton.Image = LayoutCeiling.Properties.Resources.tool_innerCutout;
			toolButton.Click += OnToolClick;
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

			cutout = new List<Point2>();
			finish = false;
		}

		public override void OnMouseMove(MouseEventArgs e, Point2 p)
		{
			newPoint = p;

			if (cutout.Count > 2 && p.DistanceTo(cutout.First()) <= mainForm.viewport.PointSize / mainForm.viewport.Zoom)
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
					mainForm.selection.UnselectAllPoints();
					DeactivateTool();
				}
				else
				{
					cutout.Add(newPoint);
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
			cutout.Clear();
			base.DeactivateTool();
		}

		public override void ApplyChanges()
		{
			mainForm.layout.cutout.Clear();
			mainForm.layout.cutout.AddRange(cutout);
		}

		public override void DrawChangesPreview(Graphics g)
		{
			if (cutout.Count > 0)
			{
				for (int i = 0; i < cutout.Count - 1; ++i)
				{
					mainForm.viewport.DrawLine(cutout[i], cutout[i+1], Viewport.DrawStyle.Preview);
				}
				mainForm.viewport.DrawLine(cutout.Last(), newPoint, Viewport.DrawStyle.Preview);
			}

			foreach (var p in cutout)
			{
				//g.FillEllipse(Brushes.Green, p.X - mainForm.viewport.PointSize / 2, p.Y - mainForm.viewport.PointSize / 2, mainForm.viewport.PointSize, mainForm.viewport.PointSize);
				mainForm.viewport.DrawPoint(p, Viewport.DrawStyle.Preview);
			}
		}
	}
}
