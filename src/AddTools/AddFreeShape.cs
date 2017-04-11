using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.AddTools
{
	public class AddFreeShape : Tool
	{
		class Command : UndoCommand
		{
			MainForm mainForm;
			List<Point2> points;

			public Command(MainForm mainForm, List<Point2> points) : base("+ Произвольный контур")
			{
				this.mainForm = mainForm;
				this.points = new List<Point2>(points);
			}

			public override void Do()
			{
				mainForm.layout.Shapes.Add(new Shape(points));
				//TODO: выделить новый shape - mainForm.selection. ...
			}

			public override void Undo()
			{
				mainForm.layout.Shapes.RemoveAt(mainForm.layout.Shapes.Count - 1);
			}
		}

		Cursor cursorPenFinish;
		Point2 newPoint;
		List<Point2> points;

		bool finish;

		public AddFreeShape(MainForm form) : base(form, "Произвольный контур")
		{
			cursor = CustomCursor.Create("data/cursors/pen.cur");
			cursorPenFinish = CustomCursor.Create("data/cursors/penFinish.cur");
			Group = ToolGroup.AddTools;

			ToolStripItem = new ToolStripRadioButton();

			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = name;
			toolButton.ToolTipText = hint;
			toolButton.Image = LayoutCeiling.Properties.Resources.tool_addPolygon;
			toolButton.Click += OnToolClick;
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

			points = new List<Point2>();
			finish = false;
		}

		public override void ApplyChanges()
		{
			mainForm.undoStack.Push(new Command(mainForm, points));
			DeactivateTool();
		}

		public override void OnMouseMove(MouseEventArgs e, Point2 p)
		{
			newPoint = p;

			if (points.Count > 2 && mainForm.viewport.CoordHoverPoint(p, points.First()))
			{
				mainForm.viewport.Cursor = cursorPenFinish;
				finish = true;
			}
			else
			{
				mainForm.viewport.Cursor = cursor;
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
				}
				else
				{
					points.Add(newPoint);
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				DeactivateTool();
			}
		}

		public override void DeactivateTool()
		{
			points.Clear();
			base.DeactivateTool();
		}

		public override void DrawChangesPreview(Graphics g)
		{
			if (points.Count > 0)
			{
				for (int i = 0; i < points.Count - 1; ++i)
				{
					mainForm.viewport.DrawLine(points[i], points[i+1], Viewport.DrawStyle.Preview);
				}
				mainForm.viewport.DrawLine(points.Last(), newPoint, Viewport.DrawStyle.Preview);
			}

			foreach (var p in points)
			{
				mainForm.viewport.DrawPoint(p, Viewport.DrawStyle.Preview);
			}
		}
	}
}
