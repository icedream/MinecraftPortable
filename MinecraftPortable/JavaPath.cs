using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
namespace MinecraftPortable
{
	public static class JavaPath
	{
		public static Process CreateJava(params string[] parameters)
		{
			return JavaPath.ApplyProcessInfo("java.exe", parameters);
		}
		public static Process CreateJavaW(params string[] parameters)
		{
			return JavaPath.ApplyProcessInfo("javaw.exe", parameters);
		}
		private static Process ApplyProcessInfo(string binary, string[] parameters)
		{
			return new Process
			{
				StartInfo = 
				{
					FileName = Path.Combine(JavaPath.GetJavaBinaryPath(), binary),
					Arguments = JavaPath.CombineParameters(parameters),
					UseShellExecute = false,
					WindowStyle = ProcessWindowStyle.Hidden,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					RedirectStandardInput = true
				}
			};
		}
		private static string CombineParameters(string[] parameters)
		{
			return "\"" + string.Join("\" \"", parameters) + "\"";
		}
		public static string GetJavaBinaryPath()
		{
			object obj = JavaPath.GetJavaRegistry().OpenSubKey(JavaPath.GetJavaVersion());
			if (obj == null)
			{
				throw new JavaNotFoundException();
			}
			obj = ((RegistryKey)obj).GetValue("JavaHome", null);
			if (obj == null)
			{
				throw new JavaNotFoundException();
			}
			obj = Path.Combine(obj.ToString(), "bin");
			if (!Directory.Exists(obj.ToString()))
			{
				throw new JavaNotFoundException();
			}
			return obj.ToString();
		}
		public static string GetJavaHome()
		{
			object obj = JavaPath.GetJavaRegistry().OpenSubKey(JavaPath.GetJavaVersion());
			if (obj == null)
			{
				throw new JavaNotFoundException();
			}
			obj = ((RegistryKey)obj).GetValue("JavaHome", null);
			if (obj == null)
			{
				throw new JavaNotFoundException();
			}
			return obj.ToString();
		}
		public static RegistryKey GetJavaRegistry()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("JavaSoft").OpenSubKey("Java Development Kit");
			if (registryKey == null)
			{
				registryKey = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("JavaSoft").OpenSubKey("Java Runtime Environment");
			}
			if (Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Wow6432Node") != null)
			{
				if (registryKey == null)
				{
					registryKey = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Wow6432Node").OpenSubKey("JavaSoft").OpenSubKey("Java Development Kit");
				}
				if (registryKey == null)
				{
					registryKey = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Wow6432Node").OpenSubKey("JavaSoft").OpenSubKey("Java Runtime Environment");
				}
			}
			if (registryKey == null)
			{
				throw new JavaNotFoundException();
			}
			return registryKey;
		}
		public static string GetJavaVersion()
		{
			object value = JavaPath.GetJavaRegistry().GetValue("CurrentVersion", null);
			if (value == null)
			{
				throw new JavaNotFoundException();
			}
			return value.ToString();
		}
	}
}
