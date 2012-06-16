using System;
using System.Security.Cryptography;
using System.Text;
namespace RedCorona.Cryptography
{
	public class PKCSKeyGenerator
	{
		private byte[] key = new byte[8];
		private byte[] iv = new byte[8];
		private DESCryptoServiceProvider des = new DESCryptoServiceProvider();
		public byte[] Key
		{
			get
			{
				return this.key;
			}
		}
		public byte[] IV
		{
			get
			{
				return this.iv;
			}
		}
		public ICryptoTransform Encryptor
		{
			get
			{
				return this.des.CreateEncryptor(this.key, this.iv);
			}
		}
		public PKCSKeyGenerator()
		{
		}
		public PKCSKeyGenerator(string keystring, byte[] salt, int md5iterations, int segments)
		{
			this.Generate(keystring, salt, md5iterations, segments);
		}
		public ICryptoTransform Generate(string keystring, byte[] salt, int md5iterations, int segments)
		{
			int num = 16;
			byte[] array = new byte[num * segments];
			byte[] bytes = Encoding.UTF8.GetBytes(keystring);
			byte[] array2 = new byte[bytes.Length + salt.Length];
			Array.Copy(bytes, array2, bytes.Length);
			Array.Copy(salt, 0, array2, bytes.Length, salt.Length);
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array3 = null;
			byte[] array4 = new byte[num + array2.Length];
			for (int i = 0; i < segments; i++)
			{
				if (i == 0)
				{
					array3 = array2;
				}
				else
				{
					Array.Copy(array3, array4, array3.Length);
					Array.Copy(array2, 0, array4, array3.Length, array2.Length);
					array3 = array4;
				}
				for (int j = 0; j < md5iterations; j++)
				{
					array3 = mD.ComputeHash(array3);
				}
				Array.Copy(array3, 0, array, i * num, array3.Length);
			}
			Array.Copy(array, 0, this.key, 0, 8);
			Array.Copy(array, 8, this.iv, 0, 8);
			return this.Encryptor;
		}
	}
}
