using System;
using System.Windows.Forms;
namespace MinecraftPortable
{
	internal static class Program
	{
		public static string MinecraftPath;
		public static string BinPath;
		public static string NativesPath;
		public static string LoginUsername;
		public static string LoginPasswort;
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new UnpackWindow());
			Application.Run(new LoginWindow());
		}
	}
}
