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

			Point2 center = new Point2(mainForm.viewport.Width / 2f, mainForm.viewport.Height / 2f);
			Point2 locOff = mainForm.viewport.Offset - center;

			mainForm.viewport.Offset = locOff * 1.3f + center;

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

			Point2 center = new Point2(mainForm.viewport.Width / 2f, mainForm.viewport.Height / 2f);
			Point2 locOff = mainForm.viewport.Offset - center;

			mainForm.viewport.Offset = locOff / 1.3f + center;
//			p.X = (float)(scaleX * localX + center.X);
//			p.Y = (float)(scaleY * localY + center.Y);

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
			float zoom = mainForm.viewport.Zoom;
			mainForm.viewport.Zoom = 1.0f;

			Point2 center = new Point2(mainForm.viewport.Width / 2f, mainForm.viewport.Height / 2f);
			Point2 locOff = mainForm.viewport.Offset - center;

			mainForm.viewport.Offset = locOff / zoom + center;

			base.OnActionClick(sender, e);
		}
	}
}
