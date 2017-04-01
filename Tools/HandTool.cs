using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Tools
{
	class HandTool : Tool
	{
		private bool moving;
		private Point2 from, to;

		public HandTool(MainForm mainForm): base(mainForm, "Рука")
		{
			ToolStripItem = new ToolStripRadioButton();

			ToolStripRadioButton toolButton = (ToolStripRadioButton)ToolStripItem;
			toolButton.Text = name;
			toolButton.ToolTipText = "Перемещение по макету";
			toolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

			toolButton.Image = LayoutCeiling.Properties.Resources.tool_handTool;
			toolButton.Click += OnToolClick;

			//cursor = CustomCursor.Create("data/cursors/hand.cur");

			moving = false;
		}

		public override void ApplyChanges()
		{
		}

		public override void OnMouseDown(MouseEventArgs e, Point2 p)
		{
			moving = true;
			from = Point2.FromPoint(e.Location);
		}

		public override void OnMouseMove(MouseEventArgs e, Point2 p)
		{
			if (moving)
			{
				to = Point2.FromPoint(e.Location);
				mainForm.viewport.Offset += to - from;
				from = to;
			}
		}

		public override void OnMouseUp(MouseEventArgs e, Point2 p)
		{
			moving = false;
		}
	}
}
