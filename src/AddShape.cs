using System.Windows.Forms;

namespace LayoutCeiling
{
	public abstract class AddShape : Tool
	{
		public AddShape(MainForm mainForm, string name, Keys hotKeys = Keys.None) : base(mainForm, name, hotKeys)
		{
			Group = ToolGroup.AddTools;
		}
	}
}
