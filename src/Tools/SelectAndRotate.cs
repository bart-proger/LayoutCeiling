using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Tools
{
	class SelectAndRotate : Select
	{
		class RotateCmd : UndoCommand
		{
			MainForm mainForm;

			Point2 center;
			double dAngle;
//			List<PointF> prevPoints;

			public RotateCmd(MainForm mainForm, Point2 center, double dAngle)
				: base("Вращение")
			{
				this.mainForm = mainForm;
				this.center = center;
				this.dAngle = dAngle;

				Text += " ";
				foreach (int i in mainForm.selection.indices)
				{
					Text += CeilingLayout.PointLetter(i);
				}
//				prevPoints = new List<PointF>();
/*
				foreach (int i in mainForm.selection.indices)
				{
					prevPoints.Add(mainForm.layout.points[i]);
				}
 * */
			}

			public override void Do()
			{
				foreach (var i in mainForm.selection.indices)
				{
					mainForm.layout.points[i] = SelectAndRotate.RotatePoint(mainForm.layout.points[i], center, dAngle);
				}
			}

			public override void Undo()
			{
				foreach (var i in mainForm.selection.indices)
				{
					mainForm.layout.points[i] = SelectAndRotate.RotatePoint(mainForm.layout.points[i], center, -dAngle);
				}
/*
				int j = -1;
				foreach (var i in mainForm.selection.indices)
				{
					++j;
					mainForm.layout.points[i] = prevPoints[j];
				}
 * */
			}
		}

		private bool rotating;
		private Cursor cursorRotate;

		public SelectAndRotate(MainForm mainForm): base(mainForm)
		{
			HotKeys = Keys.E;

			ToolStripItem = new ToolStripRadioButton();
			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = "Повернуть";
			toolButton.ToolTipText = "Виделить и повернуть";
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolButton.Image = LayoutCeiling.Properties.Resources.tool_rotate;
			toolButton.Click += OnToolClick;

			cursorRotate = CustomCursor.Create("data/cursors/rotate.cur");
		}

		public static Point2 RotatePoint(Point2 p, Point2 center, double dAngle)
		{
			float localX = (p.X - center.X);
			float localY = (p.Y - center.Y);

			double radius = center.DistanceTo(p);

 			double angle = Math.Atan2(p.Y - center.Y, p.X - center.X);

			p.X = (float)(Math.Cos(angle + dAngle) * radius + center.X);
			p.Y = (float)(Math.Sin(angle + dAngle) * radius + center.Y);

			return p;
		}

		public override void ActivateTool()
		{
			base.ActivateTool();
			mainForm.selection.Pivot = CalcSelectionCenter();
		}

		public override void ApplyChanges()
		{
			if (rotating)
			{
				Point2 center = mainForm.selection.Pivot;
				double dAngle = Math.Atan2(to.Y - center.Y, to.X - center.X) - Math.Atan2(from.Y - center.Y, from.X - center.X);
				mainForm.undoStack.Push(new RotateCmd(mainForm, center, dAngle));
//  				foreach (var i in mainForm.selection.indices)
// 				{
// 					mainForm.layout.points[i] = RotatePoint(mainForm.layout.points[i]);
//  				}
			}
			else
			{
				base.ApplyChanges();
			}
		}

		public override void OnMouseDown(MouseEventArgs e, Point2 p)
		{
			base.OnMouseDown(e, p);

			if (e.Button == MouseButtons.Left)
			{
				if (selectMode == SelectMode.New && pointsInSelectArea.Count > 0)
				{
					if (!mainForm.selection.indices.Contains(pointsInSelectArea.First()))
					{
						ApplyChanges();
					}

					rotating = true;
					mainForm.selection.Pivot = CalcSelectionCenter();
					mainForm.viewport.Cursor = cursorRotate;
				}
			}
		}

		public override void OnMouseMove(MouseEventArgs e, Point2 p)
		{
			if (!rotating)
			{
				base.OnMouseMove(e, p);

				if (!selecting)
				{
					from = to = p;
					FindPointsInSelectArea();
					if (pointsInSelectArea.Count > 0 && selectMode == SelectMode.New)
					{
						mainForm.viewport.Cursor = cursorRotate;
					}
					else
					{
						mainForm.viewport.Cursor = cursor;
					}
				}
			}
			else
			{
				to = p;
			}
		}

		public override void OnMouseUp(MouseEventArgs e, Point2 p)
		{
			base.OnMouseUp(e, p);

			if (e.Button == MouseButtons.Left)
			{
				mainForm.selection.Pivot = CalcSelectionCenter();
				rotating = false;
			}
		}

		public override void DrawChangesPreview(Graphics g)
		{
			mainForm.viewport.DrawPivot();

			if (rotating)
			{
				Point2 center = mainForm.selection.Pivot;
				double dAngle = Math.Atan2(to.Y - center.Y, to.X - center.X) - Math.Atan2(from.Y - center.Y, from.X - center.X);

				for (int i = 0; i < mainForm.selection.indices.Count; ++i)
				{
					int moved = mainForm.selection.indices.ElementAt(i);
					int next = (moved + 1) % mainForm.layout.points.Count;
					int prev = (moved - 1 + mainForm.layout.points.Count) % mainForm.layout.points.Count;

					Point2 p1 = RotatePoint(mainForm.layout.points[moved], center, dAngle);
					Point2 p2 = mainForm.layout.points[next];
					if (mainForm.selection.Contains(next))
					{
						p2 = RotatePoint(p2, center, dAngle);
					}

					mainForm.viewport.DrawLine(p1, p2, Viewport.DrawStyle.Preview);
					if (!mainForm.selection.Contains(prev))
					{
						mainForm.viewport.DrawLine(p1, mainForm.layout.points[prev], Viewport.DrawStyle.Preview);
					}
					mainForm.viewport.DrawPoint(p1, Viewport.DrawStyle.New);
				}
			}
			else
			{
				base.DrawChangesPreview(g);
			}
		}



	}
}
