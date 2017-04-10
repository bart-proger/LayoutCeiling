using System;
using System.Drawing;
using System.Windows.Forms;

namespace LayoutCeiling
{
	public abstract class Tool
	{
		public enum ToolGroup { EditTools = 0, AddTools = 1 }

		protected string name;
		protected string hint;
		public Keys HotKeys { set; get; }

		public ToolStripItem ToolStripItem { set; get; }
		public ToolGroup Group { get; set; }

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
			Group = ToolGroup.EditTools;
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
			if (mainForm.activeTool != this)
			{
				prevTool = mainForm.activeTool;
				if (prevTool != null)
					((ToolStripRadioButton)prevTool.ToolStripItem).Checked = false;
			}
			mainForm.activeTool = this;
			mainForm.viewport.Cursor = cursor;
			mainForm.viewport.Draw();
		}

		public virtual void DeactivateTool()
		{
			if (ToolStripItem != null)
				((ToolStripRadioButton)ToolStripItem).Checked = false;
			mainForm.viewport.Cursor = Cursors.Default;
			if (prevTool != null)
				//prevTool.ToolStripItem.PerformClick();
				prevTool.ActivateTool();
		}

		public virtual void DrawChangesPreview(Graphics g) { }
		public abstract void ApplyChanges();

		public virtual void OnMouseMove(MouseEventArgs e, Point2 p) { }
		public virtual void OnMouseUp(MouseEventArgs e, Point2 p) { }
		public virtual void OnMouseDown(MouseEventArgs e, Point2 p) { }
		public virtual void OnKeyDown(KeyEventArgs e) { }
		public virtual void OnKeyUp(KeyEventArgs e) { }
	}
}
