using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Actions
{
	class ZoomIn : Action
	{
		public ZoomIn(MainForm form) : base(form, "Увеличить масштаб", Keys.Control | Keys.Add) 
		{
			MenuItem = new ToolStripMenuItem(name);
			MenuItem.ShowShortcutKeys = true;
			MenuItem.ShortcutKeys = hotKeys;
			MenuItem.Click += OnActionClick;
		}

		protected override void OnActionClick(object sender, EventArgs e)
		{
			mainForm.viewport.Zoom *= 1.3f;

			base.OnActionClick(sender, e);
		}
	}

	class ZoomOut : Action
	{
		public ZoomOut(MainForm form) : base(form, "Уменьшить масштаб", Keys.Control | Keys.Subtract)
		{
			MenuItem = new ToolStripMenuItem(name);
			MenuItem.ShowShortcutKeys = true;
			MenuItem.ShortcutKeys = hotKeys;
			MenuItem.Click += OnActionClick;
		}

		protected override void OnActionClick(object sender, EventArgs e)
		{
			mainForm.viewport.Zoom /= 1.3f;

			base.OnActionClick(sender, e);
		}
	}

	class Zoom100 : Action
	{
		public Zoom100(MainForm form) : base(form, "Масштаб 100%", Keys.Control | Keys.NumPad1)
		{
			MenuItem = new ToolStripMenuItem(name);
			MenuItem.ShowShortcutKeys = true;
			MenuItem.ShortcutKeys = hotKeys;
			MenuItem.Click += OnActionClick;
		}

		protected override void OnActionClick(object sender, EventArgs e)
		{
			mainForm.viewport.Zoom = 1.0f;

			base.OnActionClick(sender, e);
		}
	}
}
