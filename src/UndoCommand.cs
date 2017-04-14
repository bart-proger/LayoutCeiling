using System.Collections.Generic;

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

	public class UndoCommandList : UndoCommand
	{
		private List<UndoCommand> commands;

		public UndoCommandList(string text, List<UndoCommand> commands) : base(text)
		{
			this.commands = new List<UndoCommand>(commands);
		}

		public override void Do()
		{
			foreach (var cmd in commands)
				cmd.Do();
		}

		public override void Undo()
		{
			for (int i = commands.Count - 1; i >= 0; --i)
				commands[i].Undo();
		}
	}
}
