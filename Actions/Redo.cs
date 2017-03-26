using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Actions
{
	class Redo : Action
	{
		public Redo(MainForm form) : base(form, "Повторить", (Keys)(Keys.Control | Keys.Y))
		{
			MenuItem = new ToolStripMenuItem(name);
			MenuItem.ShowShortcutKeys = true;
			MenuItem.ShortcutKeys = hotKeys;
			MenuItem.Image = Properties.Resources.act_redo_s;
			MenuItem.Click += OnActionClick;
		}

		protected override void OnActionClick(object sender, EventArgs e)
		{
			mainForm.undoStack.Redo();

			base.OnActionClick(sender, e);
		}
	}
}
