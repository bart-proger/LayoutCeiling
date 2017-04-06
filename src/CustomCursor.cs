using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutCeiling
{
	public class CustomCursor
	{
		[System.Runtime.InteropServices.DllImport("User32.dll")]
		private static extern IntPtr LoadCursorFromFile(String str);

		public static System.Windows.Forms.Cursor Create(string fileName)
		{
			IntPtr hCursor = LoadCursorFromFile(fileName);
			if (!IntPtr.Zero.Equals(hCursor))
			{
				return new System.Windows.Forms.Cursor(hCursor);
			}
			else
			{
				throw new ApplicationException("Could not create cursor from file " + fileName);
			}
		}

		[System.Runtime.InteropServices.DllImport("User32.dll")]
		static extern IntPtr CreateIconFromResource(byte[] presbits, uint dwResSize, bool fIcon, uint dwVer);

		// modification here is :   byte[] resource in the call       
		public static System.Windows.Forms.Cursor Create(byte[] resource)
		{
			IntPtr hCursor = CreateIconFromResource(resource, (uint)resource.Length, false, 0x00030000);
			if (!IntPtr.Zero.Equals(hCursor))
			{
				return new System.Windows.Forms.Cursor(hCursor);
			}
			else
			{  
				throw new ApplicationException("Could not create cursor from Embedded resource ");
			}
		}

		public static System.Windows.Forms.Cursor Create2(byte[] resource)
		{
			using (var memoryStream = new System.IO.MemoryStream(resource))
			{
				return new System.Windows.Forms.Cursor(memoryStream);
			}
		}
	}
}
