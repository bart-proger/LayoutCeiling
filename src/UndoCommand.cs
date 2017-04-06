using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutCeiling
{
	public abstract class UndoCommand
	{
		public string Text { set; get; }

		public UndoCommand(string text)
		{
			Text = text;
		}

		public abstract void Do();
		public abstract void Undo();
	}
}
