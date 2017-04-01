using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Tools
{
	public class Select : Tool
	{
		public class SelectCmd : UndoCommand
		{
			MainForm mainForm;

			SelectMode mode;
			HashSet<int> pointsIndices;
			HashSet<int> prevSelectionPointsIndices;

			public SelectCmd(MainForm mainForm, SelectMode mode, HashSet<int> pointsIndices) : base("Выделение")
			{
				this.mainForm = mainForm;
				this.mode = mode;
				this.pointsIndices = new HashSet<int>();
				this.pointsIndices.UnionWith(pointsIndices);

				if (mode == SelectMode.New)
				{
					prevSelectionPointsIndices = new HashSet<int>(mainForm.selection.indices);
				}

				if (pointsIndices.Count == 0)
				{
					Text = "Сброс выделения";
				}
				else
				{
					Text += " ";
					foreach (int i in pointsIndices)
					{
						Text += CeilingLayout.PointLetter(i);
					}
				}
			}

			public override void Do()
			{
				switch (mode)
				{
					case SelectMode.New:
						mainForm.selection.UnselectAll();
						mainForm.selection.SelectPoints(pointsIndices);
						break;
					case SelectMode.Add:
						mainForm.selection.SelectPoints(pointsIndices);
						break;
					case SelectMode.Remove:
						mainForm.selection.UnselectPoints(pointsIndices);
						break;
				}
			}

			public override void Undo()
			{
				switch (mode)
				{
					case SelectMode.New:
						mainForm.selection.UnselectAll();
						mainForm.selection.SelectPoints(prevSelectionPointsIndices);
						break;
					case SelectMode.Add:
						mainForm.selection.UnselectPoints(pointsIndices);
						break;
					case SelectMode.Remove:
						mainForm.selection.SelectPoints(pointsIndices);
						break;
				}
			}
		}

		protected bool selecting;/*private enum ToolMode { Browse, Select, Move }*/
		public enum SelectMode { New, Add, Remove }
		/*private enum MoveMode { Free, Linked, Strict }*/

		/*private ToolMode toolMode;*/
		private Pen pen;
		protected Cursor cursorSelect, cursorAdd, cursorRemove;

		protected SelectMode selectMode;
		protected Point2 from, to;
		protected HashSet<int> pointsInSelectArea;
		private float left, right, top, bottom;
		
		//private MoveMode moveMode;

		public Select(MainForm mainForm): base(mainForm, "Выделить", Keys.Q)
		{
			ToolStripItem = new ToolStripRadioButton();

			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = name;
			toolButton.ToolTipText = hint;
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolButton.Image = LayoutCeiling.Properties.Resources.tool_select;
			toolButton.Click += OnToolClick;

			selecting = false;
			pen = new Pen(Color.FromArgb(128, Color.Black));
			pen.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
			pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

			selectMode = SelectMode.New;
			cursorAdd = CustomCursor.Create("data/cursors/add.cur");
			cursorRemove = CustomCursor.Create("data/cursors/remove.cur");
			cursor = cursorSelect = Cursors.Arrow;
			pointsInSelectArea = new HashSet<int>();
			left = right = top = bottom = -1;
		}

		protected Point2 CalcSelectionCenter()
		{
			Point2 center = new Point2(0, 0);
			if (mainForm.selection.indices.Count == 0)
				return center;
			if (mainForm.selection.indices.Count == 1)
			{
				center = mainForm.layout.points[mainForm.selection.indices.First()];
				return center;
			}

			foreach (var i in mainForm.selection.indices)
			{
				center.X += mainForm.layout.points[i].X;
				center.Y += mainForm.layout.points[i].Y;
			}
			center.X /= mainForm.selection.indices.Count;
			center.Y /= mainForm.selection.indices.Count;

			return center;
		}

		protected void FindPointsInSelectArea()
		{
			pointsInSelectArea.Clear();

			if (from == to)
			{
				for (int i = 0; i < mainForm.layout.points.Count; ++i)
				{
					if (from.DistanceTo(mainForm.layout.points[i]) <= mainForm.viewport.PointSize /*/ 2*/ / mainForm.viewport.Zoom)
					{
						bool add = true;
						switch (selectMode)
						{
							case SelectMode.Add:
								add = !mainForm.selection.Contains(i);
								break;
							case SelectMode.Remove:
								add = mainForm.selection.Contains(i);
								break;
						}
						if (add)
							pointsInSelectArea.Add(i);
						break;
					}
				}
				return;
			}

			if (from.X < to.X)
			{
				left = from.X;
				right = to.X;
			}
			else
			{
				left = to.X;
				right = from.X;
			}
			if (from.Y < to.Y)
			{
				top = from.Y;
				bottom = to.Y;
			}
			else
			{
				top = to.Y;
				bottom = from.Y;
			}

			for (int i = 0; i < mainForm.layout.points.Count; ++i)
			{
				Point2 p = mainForm.layout.points[i];
				if (p.X >= left-3 && p.X <= right+3 && p.Y >= top-3 && p.Y <= bottom+3)
				{
					bool add = true;
					switch (selectMode)
					{
						case SelectMode.Add:
							add = !mainForm.selection.Contains(i);
							break;
						case SelectMode.Remove:
							add = mainForm.selection.Contains(i);
							break;
					}
					if (add)
						pointsInSelectArea.Add(i);
				}
			}
		}

		public override void DeactivateTool()
		{
			pointsInSelectArea.Clear();
			base.DeactivateTool();
		}

		public override void ApplyChanges()
		{
			if (selecting)
			{
				bool doCmd = false;
				switch (selectMode)
				{
					case SelectMode.New:
						doCmd = ((pointsInSelectArea.Count > 0 || mainForm.selection.indices.Count > 0) && 
							!mainForm.selection.indices.SetEquals(pointsInSelectArea));
						break;
					case SelectMode.Add:
// 						pointsInSelectArea.ExceptWith(mainForm.selection.pointsIndices);
// 												doCmd = (pointsInSelectArea.Count > 0);// || (pointsInSelectArea.Count == 1 && !mainForm.selection.Contains(pointsInSelectArea.First())));
// 												break;
					case SelectMode.Remove:
						//pointsInSelectArea.IntersectWith(mainForm.selection.pointsIndices);
						doCmd = (pointsInSelectArea.Count > 0);// || (pointsInSelectArea.Count == 1 && mainForm.selection.Contains(pointsInSelectArea.First())));
						break;
				}
				if (doCmd)
				{
					mainForm.undoStack.Push(new SelectCmd(mainForm, selectMode, pointsInSelectArea));
				}
			}
		}
		
		public override void OnKeyDown(KeyEventArgs e)
		{
			selectMode = SelectMode.New;

			if (e.Control)
			{
				selectMode = SelectMode.Add;
				mainForm.viewport.Cursor = cursor = cursorAdd;
			}
			else if (e.Alt)
			{
				selectMode = SelectMode.Remove;
				mainForm.viewport.Cursor = cursor = cursorRemove;
			}
		}

		public override void OnKeyUp(KeyEventArgs e)
		{
			selectMode = SelectMode.New;
			mainForm.viewport.Cursor = cursor = cursorSelect;
		}
		
		public override void OnMouseDown(MouseEventArgs e, Point2 p)
		{
			if (e.Button == MouseButtons.Left)
			{
				selecting = true;/*toolMode = ToolMode.Select;*/

				from = to = p;
				FindPointsInSelectArea();
			}
		}

		public override void OnMouseMove(MouseEventArgs e, Point2 p)
		{
			if (selecting)
			{
				to = p;
				FindPointsInSelectArea();
			}

			//mainForm.viewport.Cursor = cursorSelect;
		}

		public override void OnMouseUp(MouseEventArgs e, Point2 p)
		{
			if (e.Button == MouseButtons.Left)
			{
				ApplyChanges();

				selecting = false;/*toolMode = ToolMode.Browse;*/
				selectMode = SelectMode.New;
				left = right = top = bottom = -1;
			}
		}

		public override void DrawChagesPreview(Graphics g)
		{
			if (selecting)
			{
				Viewport.DrawStyle drawStyle = Viewport.DrawStyle.Selected;
				if (selectMode == SelectMode.Remove)
					drawStyle = Viewport.DrawStyle.Normal;
				foreach (int i in pointsInSelectArea)
				{
					mainForm.viewport.DrawPoint(mainForm.layout.points[i], drawStyle);
				}

				Point2 rectPos = mainForm.viewport.ToViewportSpace(new Point2(left, top));
				Point2 rectSize = mainForm.viewport.ToViewportSpace(new Point2(right - left, bottom - top));
				g.DrawRectangle(pen, rectPos.X, rectPos.Y, rectSize.X, rectSize.Y);
				mainForm.viewport.DrawSelectArea();
			}
		}
	}
}
