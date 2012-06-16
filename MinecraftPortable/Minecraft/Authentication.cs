using RedCorona.Cryptography;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
namespace Minecraft
{
	public class Authentication
	{
		public Uri LoginApiUri
		{
			get;
			set;
		}
		public string Username
		{
			get;
			set;
		}
		public string Password
		{
			get;
			set;
		}
		public X509CertificateCollection Certificates
		{
			get;
			set;
		}
		public Exception LastError
		{
			get;
			set;
		}
		public int Timeout
		{
			get;
			set;
		}
		public string LatestVersion
		{
			get;
			set;
		}
		private string TicketID
		{
			get;
			set;
		}
		public string CaseCorrectUsername
		{
			get;
			set;
		}
		public string SessionId
		{
			get;
			set;
		}
		public Authentication()
		{
			this.__construct();
		}
		public Authentication(Uri api)
		{
			this.__construct();
			this.LoginApiUri = api;
		}
		public Authentication(string user, string password)
		{
			this.__construct();
			this.Username = user;
			this.Password = password;
		}
		public Authentication(string user, string password, Uri api)
		{
			this.__construct();
			this.Username = user;
			this.Password = password;
			this.LoginApiUri = api;
		}
		private void __construct()
		{
			this.Certificates = new X509CertificateCollection();
			this.Timeout = 3000;
			this.LoginApiUri = new Uri("https://login.minecraft.net");
		}
		public bool Login(string user, string password)
		{
			this.Username = user;
			this.Password = password;
			return this.Login();
		}
		public bool Login()
		{
			try
			{
				if (string.IsNullOrEmpty(this.Username))
				{
					throw new ArgumentNullException("Username");
				}
				if (string.IsNullOrEmpty(this.Password))
				{
					throw new ArgumentNullException("Password");
				}
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.LoginApiUri);
				httpWebRequest.Timeout = (httpWebRequest.ReadWriteTimeout = this.Timeout);
				httpWebRequest.UserAgent = "MP/" + Assembly.GetExecutingAssembly().GetName().Version;
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
				streamWriter.Write("?");
				streamWriter.Write("&user=" + Uri.EscapeDataString(this.Username));
				streamWriter.Write("&password=" + Uri.EscapeDataString(this.Password));
				streamWriter.Write("&version=13");
				streamWriter.Flush();
				streamWriter.Close();
				WebResponse response = httpWebRequest.GetResponse();
				StreamReader streamReader = new StreamReader(response.GetResponseStream());
				char[] array = new char[response.ContentLength];
				streamReader.Read(array, 0, (int)response.ContentLength);
				response.Close();
				string[] array2 = new string(array).Split(new char[]
				{
					':'
				});
				if (array2.Length < 4)
				{
					throw new Exception(string.Join(":", array2));
				}
				this.LatestVersion = array2[0];
				this.TicketID = array2[1];
				this.CaseCorrectUsername = array2[2];
				this.SessionId = array2[3];
				return true;
			}
			catch (Exception lastError)
			{
				this.LastError = lastError;
			}
			return false;
		}
		public void SaveVersion(string where)
		{
			File.WriteAllText(where, this.LatestVersion);
		}
		public void SaveLogin(string where)
		{
			byte[] array = new byte[8];
			new Random(43287234).NextBytes(array);
			using (ICryptoTransform cryptoTransform = new PKCSKeyGenerator().Generate("passwordfile", array, 5, 1))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(this.Username + "\n");
				byte[] bytes2 = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
				File.WriteAllBytes(where, bytes2);
			}
		}
	}
}
