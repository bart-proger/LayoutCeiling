using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutCeiling.Actions
{
	class DeletePoint : Action
	{
		public class DeletePointCmd : UndoCommand
		{
			MainForm mainForm;

			HashSet<int> indices;
			List<Point2> points;

			public DeletePointCmd(MainForm mainForm, HashSet<int> pointsIndices)
				: base("Удаление точек")
			{
				this.mainForm = mainForm;
				indices = new HashSet<int>(pointsIndices.OrderByDescending(i => i));

				points = new List<Point2>();

				foreach (var i in indices)
				{
					points.Add(mainForm.layout.points[i]);
				}
			}

			public override void Do()
			{
				foreach (var index in indices)
				{
					mainForm.layout.points.RemoveAt(index);
				}
				mainForm.selection.UnselectAll();
			}

			public override void Undo()
			{
				var sortedByInc = indices.OrderBy(i => i);
				int n = points.Count;
				foreach (var index in sortedByInc)
				{
					mainForm.layout.points.Insert(index, points[--n]);
				}
				mainForm.selection.SelectPoints(indices);
			}
		}

		public DeletePoint(MainForm form) : base(form, "Удалить точки", Keys.Delete)
		{
			MenuItem = new ToolStripMenuItem(name);
			MenuItem.ShowShortcutKeys = true;
			MenuItem.ShortcutKeys = hotKeys;
			MenuItem.Click += OnActionClick;
		}

		protected override void OnActionClick(object sender, EventArgs e)
		{
			if (mainForm.selection.IsEmpty())
			{
				return;
			}
			mainForm.undoStack.Push(new DeletePointCmd(mainForm, mainForm.selection.indices));

			base.OnActionClick(sender, e);
		}
	}
}
