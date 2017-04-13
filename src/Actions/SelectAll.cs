using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Actions
{
	class SelectAll : Action
	{
		public SelectAll(MainForm form) : base(form, "Выделить все", Keys.Control | Keys.A)
		{
			MenuItem = new ToolStripMenuItem(name);
			MenuItem.ShowShortcutKeys = true;
			MenuItem.ShortcutKeys = hotKeys;
			MenuItem.Click += OnActionClick;
		}

		protected override void OnActionClick(object sender, EventArgs e)
		{
			if (!mainForm.selection.ContainsShape())
				return;
			HashSet<int> indices = new HashSet<int>();
			for (int i = 0; i < mainForm.layout.Shapes[mainForm.selection.ShapeIndex].Points.Count; ++i)
			{
				indices.Add(i);
			}
			mainForm.undoStack.Push(new Tools.Select.SelectCmd(mainForm, Tools.Select.SelectMode.New, indices));

			base.OnActionClick(sender, e);
		}
	}
}
