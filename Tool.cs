using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling
{
	public abstract class Tool
	{
		//public enum ToolGroup { None = 0, EditTools = 1, AddTools = 2 }

		protected string name;
		protected string hint;
		public Keys HotKeys { set; get; }

		public ToolStripItem ToolStripItem { set; get; }
		//public Form form;
		protected Cursor cursor;

		protected MainForm mainForm;
		private Tool prevTool;

		//public EventHandler OnToolClick;

		public Tool(MainForm mainForm, string name, Keys hotKeys = Keys.None)
		{
			this.name = name;
			this.hint = name;
			this.HotKeys = hotKeys;
			ToolStripItem = null;
			//form = null;
			//cursor = Cursors.Default;
			this.mainForm = mainForm;

		/*	OnToolClick = new EventHandler(delegate(object o, EventArgs a)
			{
				ActivateTool();
			});*/
		}

		protected void OnToolClick(object sender, EventArgs e)
		{
			ActivateTool();
			mainForm.viewport.Draw();
		}

		public virtual void ActivateTool()
		{
			if (ToolStripItem != null)
				//ToolStripItem.PerformClick();
				((ToolStripRadioButton)ToolStripItem).Checked = true;
			prevTool = mainForm.activeTool;
			mainForm.activeTool = this;
			mainForm.viewport.Cursor = cursor;
			mainForm.viewport.Draw();
		}

		public virtual void DeactivateTool()
		{
		//	if (toolStripItem != null)
			//	((ToolStripRadioButton)toolStripItem).Checked = false;
			mainForm.viewport.Cursor = Cursors.Default;
			if (prevTool != null)
				//prevTool.ToolStripItem.PerformClick();
				prevTool.ActivateTool();
		}

		public virtual void DrawChagesPreview(Graphics g) { }
		public abstract void ApplyChanges();

		public virtual void OnMouseMove(MouseEventArgs e, Point2 p) { }
		public virtual void OnMouseUp(MouseEventArgs e, Point2 p) { }
		public virtual void OnMouseDown(MouseEventArgs e, Point2 p) { }
		public virtual void OnKeyDown(KeyEventArgs e) { }
		public virtual void OnKeyUp(KeyEventArgs e) { }
	}
}
