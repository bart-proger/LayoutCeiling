using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Tools
{
	class SelectAndScale : Select
	{
		class ScaleCmd : UndoCommand
		{
			MainForm mainForm;

			Point2 center;
			double dScaleX, dScaleY;
			List<Point2> prevPoints;

			public ScaleCmd(MainForm mainForm, Point2 center, double dScaleX, double dScaleY)
				: base("Масштабирование")
			{
				this.mainForm = mainForm;
				this.center = center;
				this.dScaleX = dScaleX;
				this.dScaleY = dScaleY;
				prevPoints = new List<Point2>();

				Text += " ";
				foreach (int i in mainForm.selection.indices)
				{
					Text += CeilingLayout.PointLetter(i);
					prevPoints.Add(mainForm.layout.points[i]);
				}


			}

			public override void Do()
			{
 				foreach (var i in mainForm.selection.indices)
				{
					mainForm.layout.points[i] = SelectAndScale.ScalePoint(mainForm.layout.points[i], center, dScaleX, dScaleY);
 				}				
			}

			public override void Undo()
			{
// 				foreach (var i in mainForm.selection.pointsIndices)
// 				{
// 					mainForm.layout.points[i] = SelectAndScale.ScalePoint(mainForm.layout.points[i], center, -dScaleX, -dScaleY);
// 				}
				
				int j = -1;
				foreach (var i in mainForm.selection.indices)
				{
					++j;
					mainForm.layout.points[i] = prevPoints[j];
				}
			}
		}


		private bool scaling;
		private Cursor cursorScale;
		private Point2 center;
		public static float ScaleSize = 0.005f;

		public SelectAndScale(MainForm mainForm): base(mainForm)
		{
			HotKeys = Keys.R;

			ToolStripItem = new ToolStripRadioButton();
			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = "Масштабировать";
			toolButton.ToolTipText = "Виделить и масштабировать";
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolButton.Image = LayoutCeiling.Properties.Resources.tool_scale;
			toolButton.Click += OnToolClick;

			cursorScale = CustomCursor.Create("data/cursors/scale.cur");
		}

		private void CalcSelectionCenter()
		{
			center = new Point2(0, 0);
			if (mainForm.selection.indices.Count == 0)
				return;
			if (mainForm.selection.indices.Count == 1)
			{
				center = mainForm.layout.points[mainForm.selection.indices.First()];
				return;
			}

			foreach (var i in mainForm.selection.indices)
			{
				center.X += mainForm.layout.points[i].X;
				center.Y += mainForm.layout.points[i].Y;
			}
			center.X /= mainForm.selection.indices.Count;
			center.Y /= mainForm.selection.indices.Count;
		}

		public static Point2 ScalePoint(Point2 p, Point2 center, double dScaleX, double dScaleY)
		{
			double localX = (p.X - center.X);
			double localY = (p.Y - center.Y);

			double scaleX = (1 + dScaleX);
			double scaleY = (1 + dScaleY);
			if (scaleX < 0)
				scaleX = 0;
			if (scaleY < 0)
				scaleY = 0;

// 			int curScaleX = Math.Abs(to.X - center.X);
// 			int curScaleY = Math.Abs(to.Y - center.Y);

/*			double scaleX = dx + (1f / ScaleSize);
			double scaleY = dy - (1f / ScaleSize);

			scaleX = (scaleX > 0 ? scaleX : 1 / (-scaleX + 1));
			scaleY = (-scaleY > 0 ? -scaleY : 1 / (scaleY + 1));
*/
			p.X = (float)(scaleX * localX + center.X);
			p.Y = (float)(scaleY * localY + center.Y);

// 			p.X = (Math.Abs(p.X) > Math.Abs(center.X) ? p.X : center.X);
// 			p.Y = (Math.Abs(p.Y) > Math.Abs(center.Y) ? p.Y : center.Y);

			return p;
		}

		public override void ActivateTool()
		{
			base.ActivateTool();
			CalcSelectionCenter();
		}

		public override void ApplyChanges()
		{
			if (scaling)
			{
				float dx = (to.X - from.X), dy = (to.Y - from.Y);
				double dScaleX = dx * ScaleSize;
				double dScaleY = -dy * ScaleSize;

				if (dx != 0 || dy != 0)
				{
					mainForm.undoStack.Push(new ScaleCmd(mainForm, center, dScaleX, dScaleY));
				}
//  				foreach (var i in mainForm.selection.pointsIndices)
// 				{
// 					mainForm.layout.points[i] = ScalePoint(mainForm.layout.points[i]);
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

					scaling = true;
					CalcSelectionCenter();
					mainForm.viewport.Cursor = cursorScale;
				}
			}
		}

		public override void OnMouseMove(MouseEventArgs e, Point2 p)
		{
			if (!scaling)
			{
				base.OnMouseMove(e, p);

				if (!selecting)
				{
					from = to = p;
					FindPointsInSelectArea();
					if (pointsInSelectArea.Count > 0 && selectMode == SelectMode.New)
					{
						mainForm.viewport.Cursor = cursorScale;
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
				CalcSelectionCenter();
				scaling = false;
			}
		}

		public override void DrawChagesPreview(Graphics g)
		{
			mainForm.viewport.DrawPivot(center);

			if (scaling)
			{
				float dx = (to.X - from.X), dy = (to.Y - from.Y);
				double dScaleX = dx * ScaleSize;
				double dScaleY = -dy * ScaleSize;

				for (int i = 0; i < mainForm.selection.indices.Count; ++i)
				{
					int moved = mainForm.selection.indices.ElementAt(i);
					int next = (moved + 1) % mainForm.layout.points.Count;
					int prev = (moved - 1 + mainForm.layout.points.Count) % mainForm.layout.points.Count;

					Point2 p1 = ScalePoint(mainForm.layout.points[moved], center, dScaleX, dScaleY);
					Point2 p2 = mainForm.layout.points[next];
					if (mainForm.selection.Contains(next))
					{
						p2 = ScalePoint(p2, center, dScaleX, dScaleY);
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
