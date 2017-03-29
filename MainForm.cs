using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling
{
	public partial class MainForm : Form
	{
		List<Tool> tools;
		public Tool activeTool;

		List<Action> actions;

		public CeilingLayout layout;
		public Viewport viewport;
		public Selection selection;
		public UndoStack undoStack;

		public MainForm()
		{
			InitializeComponent();
			viewport = new Viewport(this, panel);
			Controls.Remove(panel);
			Controls.Add(viewport);
			this.ResumeLayout(false);
			this.PerformLayout();

			layout = new CeilingLayout();
			selection = new Selection();
			undoStack = new UndoStack(25);

			tools = new List<Tool>();
			tools.Add(new Tools.Select(this));
			tools.Add(new Tools.SelectAndMove(this));
			tools.Add(new Tools.SelectAndRotate(this));
			tools.Add(new Tools.SelectAndScale(this));
			tools.Add(new Tools.AddPolygon(this));
			tools.Add(new Tools.AddPoint(this));

			foreach (var tool in tools)
			{
				editTools.Items.Add(tool.ToolStripItem);
			}

			actions = new List<Action>();
			actions.Add(new Actions.SelectAll(this));
			actions.Add(new Actions.Undo(this));
			actions.Add(new Actions.Redo(this));
			actions.Add(new Actions.DeletePoint(this));

			foreach (var act in actions)
			{
				if (act.MenuItem != null)
					mmEdit.DropDownItems.Add(act.MenuItem);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			foreach (var t in tools)
			{
				if (keyData == t.HotKeys)
				{
					t.ToolStripItem.PerformClick();
					break;
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (activeTool != null)
				activeTool.OnKeyDown(e);
//			viewport.Draw();
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (activeTool != null)
				activeTool.OnKeyUp(e);
//			viewport.Draw();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			editTools.Items[0].PerformClick();
			editTools.Items[4].PerformClick();
		}
	}
}
