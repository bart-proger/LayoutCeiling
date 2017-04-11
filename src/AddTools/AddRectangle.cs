using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LayoutCeiling.AddTools
{
	class AddRectangle : AddShape
	{
		class Command : UndoCommand
		{
			MainForm mainForm;
			Point2 from, to;

			public Command(MainForm mainForm, Point2 from, Point2 to) : base("+ Прямоугольник")
			{
				this.mainForm = mainForm;
				this.from = from;
				this.to = to;

				Text += ":" + Math.Abs(to.X - from.X).ToString("0.#") + "x" + Math.Abs(to.Y - from.Y).ToString("0.#");
			}

			public override void Do()
			{
				List<Point2>  points = new List<Point2>(4);
				points.Add(from);
				points.Add(new Point2(to.X, from.Y));
				points.Add(to);
				points.Add(new Point2(from.X, to.Y));

				mainForm.layout.Shapes.Add(new Shape(points));
				//TODO: выделить новый shape - mainForm.selection. ...
			}

			public override void Undo()
			{
				mainForm.layout.Shapes.RemoveAt(mainForm.layout.Shapes.Count - 1);
			}
		}

		private Point2 from, to;
		private bool stretching;
		List<Point2> points;

		public AddRectangle(MainForm mainForm) : base(mainForm, "Прямоугольник", Keys.Alt | Keys.R)
		{
			ToolStripItem = new ToolStripRadioButton();
			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = name;
			toolButton.ToolTipText = hint;
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolButton.Image = LayoutCeiling.Properties.Resources.tool_rectangle;
			toolButton.Click += OnToolClick;

			stretching = false;
			points = new List<Point2>(4);

			points.Add(from);
			points.Add(new Point2(to.X, from.Y));
			points.Add(to);
			points.Add(new Point2(from.X, to.Y));
		}

		public override void ApplyChanges()
		{
			mainForm.undoStack.Push(new Command(mainForm, from, to));
			DeactivateTool();
		}

		public override void OnMouseUp(MouseEventArgs e, Point2 p)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (!stretching)
				{
					from = to = p;
					stretching = true;
				}
				else
				{
					ApplyChanges();
					stretching = false;
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				DeactivateTool();
			}
			
		}

		public override void OnMouseMove(MouseEventArgs e, Point2 p)
		{
			if (stretching)
			{
				to = p;
			}
		}

		public override void DrawChangesPreview(Graphics g)
		{
			if (stretching)
			{
				points[0] = from;
				points[1] = new Point2(to.X, from.Y);
				points[2] = to;
				points[3] = new Point2(from.X, to.Y);

				for (int i = 0; i < 3; ++i)
				{
					mainForm.viewport.DrawLine(points[i], points[i + 1], Viewport.DrawStyle.Preview);
				}
				mainForm.viewport.DrawLine(points.Last(), points.First(), Viewport.DrawStyle.Preview);
			}
		}
	}
}
