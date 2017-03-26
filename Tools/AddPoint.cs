using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Tools
{
	public class AddPoint : Tool
	{
		public class AddPointCmd : UndoCommand
		{
			MainForm mainForm;

			Point2 p;
			int index;

			public AddPointCmd(MainForm mainForm, Point2 p, int index)
				: base("Добавление точки")
			{
				this.mainForm = mainForm;
				this.p = p;
				this.index = index;

				Text += " " + CeilingLayout.PointLetter(index);
			}

			public override void Do()
			{
				mainForm.layout.points.Insert(index, p);
			}

			public override void Undo()
			{
				mainForm.layout.points.RemoveAt(index);
			}
		}

		Point2 newPoint;
		bool overLine;
		int insertIndex;

		public AddPoint(MainForm mainForm): base(mainForm, "Добавить точку")
		{
			ToolStripItem = new ToolStripRadioButton();

			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = name;
			toolButton.ToolTipText = hint;
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolButton.Image = LayoutCeiling.Properties.Resources.tool_addPoint;
			toolButton.Click += OnToolClick;

			cursor = CustomCursor.Create("data/cursors/pen.cur");

			insertIndex = -1;
			overLine = false;
		}

		public override void ApplyChanges()
		{
			mainForm.undoStack.Push(new AddPointCmd(mainForm, newPoint, insertIndex));
		}

		public override void ActivateTool()
		{
			base.ActivateTool();

			insertIndex = -1;
			overLine = false;
		}
		
		public override void OnMouseDown(MouseEventArgs e)
		{
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			Point2 cursorPos = newPoint = Point2.FromPoint(e.Location);

			overLine = false;
			int i = mainForm.layout.points.Count-1;
			for (int j = 0; j < mainForm.layout.points.Count; ++j)
			{
				if (Geometry.ProjectionPointToSegment(cursorPos, mainForm.layout.points[i], mainForm.layout.points[j], ref newPoint)
					&& Geometry.PointInCircle(newPoint, cursorPos, mainForm.viewport.PointSize / 2))
				{
					insertIndex = i+1;
					overLine = true;
					break;
				}
				i = j;
			}
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			if (overLine)
				ApplyChanges();
		}

		public override void DrawChagesPreview(Graphics g)
		{
			if (overLine)
			{
				mainForm.viewport.DrawPoint(newPoint, Viewport.DrawStyle.Preview);
			}
		}
	}
}
