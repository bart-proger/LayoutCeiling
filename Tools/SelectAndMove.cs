using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Tools
{
	class SelectAndMove : Select
	{
		class MoveCmd : UndoCommand
		{
			MainForm mainForm;

			float dx, dy;

			public MoveCmd(MainForm mainForm, float dx, float dy) : base("Перемещение")
			{
				this.mainForm = mainForm;
				this.dx = dx;
				this.dy = dy;

				Text += " ";
				foreach (int i in mainForm.selection.indices)
				{
					Text += CeilingLayout.PointLetter(i);
				}
			}

			public override void Do()
			{
				foreach (var i in mainForm.selection.indices)
				{
					mainForm.layout.points[i] = SelectAndMove.MovePoint(mainForm.layout.points[i], dx, dy);
				}
			}

			public override void Undo()
			{
				foreach (var i in mainForm.selection.indices)
				{
					mainForm.layout.points[i] = SelectAndMove.MovePoint(mainForm.layout.points[i], -dx, -dy);
				}
			}
		}

		private bool moving;
		private Cursor cursorMove;

		public SelectAndMove(MainForm mainForm)
			: base(mainForm)
		{
			HotKeys = Keys.W;

			ToolStripItem = new ToolStripRadioButton();
			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = "Переместить";
			toolButton.ToolTipText = "Виделить и переместить";
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolButton.Image = LayoutCeiling.Properties.Resources.tool_move;
			toolButton.Click += OnToolClick;

			cursorMove = Cursors.SizeAll;
		}

		public static Point2 MovePoint(Point2 p, float dx, float dy)
		{
			p.X += dx;
			p.Y += dy;

			return p;
		}

		public override void ApplyChanges()
		{
			if (moving)
			{
				float dx = (to.X - from.X), dy = (to.Y - from.Y);
				if (dx != 0 || dy != 0)
					mainForm.undoStack.Push(new MoveCmd(mainForm, dx, dy));
// 				foreach (var i in mainForm.selection.pointsIndices)
// 				{
// 					mainForm.layout.points[i] = MovePoint(mainForm.layout.points[i]);
// 				}
			}
			else
			{
				base.ApplyChanges();
			}
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				if (selectMode == SelectMode.New && pointsInSelectArea.Count > 0)
				{
					if (!mainForm.selection.indices.Contains(pointsInSelectArea.First()))
					{
						ApplyChanges();
					}

					moving = true;
					mainForm.viewport.Cursor = cursorMove;
				}
			}
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			if (!moving)
			{
				base.OnMouseMove(e);

				if (!selecting)
				{
					from = to = Point2.FromPoint(e.Location);
					FindPointsInSelectArea();
					if (pointsInSelectArea.Count > 0 && selectMode == SelectMode.New)
					{
						mainForm.viewport.Cursor = cursorMove;
					}
					else
					{
						mainForm.viewport.Cursor = cursor;
					}
				}
			}
			else
			{
				to = Point2.FromPoint(e.Location);
			}
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (e.Button == MouseButtons.Left)
			{
				moving = false;
			}
		}

		public override void DrawChagesPreview(Graphics g)
		{
			if (moving)
			{
				float dx = (to.X - from.X), dy = (to.Y - from.Y);
				for (int i = 0; i < mainForm.selection.indices.Count; ++i)
				{
					int moved = mainForm.selection.indices.ElementAt(i);
					int next = (moved + 1) % mainForm.layout.points.Count;
					int prev = (moved - 1 + mainForm.layout.points.Count) % mainForm.layout.points.Count;

					Point2 p1 = MovePoint(mainForm.layout.points[moved], dx, dy);
					Point2 p2 = mainForm.layout.points[next];
					if (mainForm.selection.Contains(next))
					{
						p2 = MovePoint(p2, dx, dy);
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
				base.DrawChagesPreview(g);
			}
		}



	}
}
