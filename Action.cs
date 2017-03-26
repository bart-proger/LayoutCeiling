using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling
{
	class Action
	{
		protected MainForm mainForm;
		protected string name;
		protected string hint;
		protected Keys hotKeys;

		public ToolStripButton ToolButton { set; get; }
		public ToolStripMenuItem MenuItem { set; get; }

		public Action(MainForm mainForm, string name, Keys hotKeys = Keys.None)
		{
			this.mainForm = mainForm;
			this.name = name;
			this.hint = name;
			this.hotKeys = hotKeys;
			ToolButton = null;
			MenuItem = null;
		}

		protected virtual void OnActionClick(object sender, EventArgs e)
		{
			// TODO: check-uncheck
			mainForm.viewport.Draw();
		}
	}
}
