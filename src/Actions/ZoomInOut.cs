using System;
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
			mainForm.viewport.Offset *= 1.3f;

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
			mainForm.viewport.Offset /= 1.3f;

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
			float oldZoom = mainForm.viewport.Zoom;
			mainForm.viewport.Zoom = 1.0f;
			mainForm.viewport.Offset /= oldZoom;

			base.OnActionClick(sender, e);
		}
	}

	class ZoomByLayout : Action
	{
		public ZoomByLayout(MainForm form) : base(form, "Масштаб по макету", Keys.Control | Keys.NumPad0)
		{
			MenuItem = new ToolStripMenuItem(name);
			MenuItem.ShowShortcutKeys = true;
			MenuItem.ShortcutKeys = hotKeys;
			MenuItem.Click += OnActionClick;
		}

		protected override void OnActionClick(object sender, EventArgs e)
		{
			if (!mainForm.layout.IsEmpty())
			{
				float oldZoom = mainForm.viewport.Zoom;

				float ww = (mainForm.viewport.Width - 50) / mainForm.layout.Width();
				float hh = (mainForm.viewport.Height - 50) / mainForm.layout.Height();

				if (ww < hh)
					mainForm.viewport.Zoom = ww;
				else
					mainForm.viewport.Zoom = hh;

				mainForm.viewport.Offset = (mainForm.viewport.Center - mainForm.layout.CenterOfBoundingRect()) * mainForm.viewport.Zoom;
			}

			base.OnActionClick(sender, e);
		}
	}
}
