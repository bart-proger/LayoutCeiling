using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutCeiling
{
	public class UndoStack
	{
		int index;
		int limit;
		public List<UndoCommand> history;

		public UndoStack(int limit = 0)
		{
			this.index = -1;
			this.limit = limit;
			this.history = new List<UndoCommand>();
		}

		public void Push(UndoCommand cmd)
		{
			cmd.Do();

			if (CanRedo())
				history.RemoveRange(index+1, history.Count - (index+1));

			history.Add(cmd);
			if (limit > 0 && history.Count > limit)
				history.RemoveAt(0);
			else
				++index;
		}

		public void Undo()
		{
			if (CanUndo())
			{
				history[index].Undo();
				--index;
			}
		}

		public void Redo()
		{
			if (CanRedo())
			{
				++index;
				history[index].Do();
			}
		}

		public void Clear()
		{
			index = -1;
			history.Clear();
		}

		public bool CanUndo()
		{
			return (index > -1);
		}

		public bool CanRedo()
		{
			return (index < history.Count-1);
		}

		public int Index()
		{
			return index;
		}
	}
}
