using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Actions
{
	class Undo : Action
	{
		public Undo(MainForm form) : base(form, "Отменить", (Keys)(Keys.Control | Keys.Z))
		{
			MenuItem = new ToolStripMenuItem(name);
			MenuItem.ShowShortcutKeys = true;
			MenuItem.ShortcutKeys = hotKeys;
			MenuItem.Image = Properties.Resources.act_undo_s;
			MenuItem.Click += OnActionClick;
		}

		protected override void OnActionClick(object sender, EventArgs e)
		{
			mainForm.undoStack.Undo();

			base.OnActionClick(sender, e);
		}
	}
}
