using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Cryptography;

namespace ShopMobile.utils
{
	public class HandlePasswrod
	{
		public static string hashPassword(string password, byte[]? salt)
		{
			// Tạo một salt ngẫu nhiên nếu không có
			if (salt == null)
			{
				salt = new byte[128 / 8];
				using (var rng = RandomNumberGenerator.Create())
				{
					rng.GetBytes(salt);
				}
			}

			// Băm mật khẩu với salt
			string hashed = Convert.ToBase64String(
				KeyDerivation.Pbkdf2(
					password,
					salt,
					KeyDerivationPrf.HMACSHA256,
					iterationCount: 10000,
					numBytesRequested: 256 / 8
				)
			);

			// Lưu trữ salt cùng với mật khẩu đã băm
			return $"{Convert.ToBase64String(salt)}:{hashed}";
		}

		public static bool verifyPassword(string hashedPassword, string userPassword)
		{
			// Trích xuất salt và hash từ chuỗi đã lưu trữ
			var parts = hashedPassword.Split(':');
			if (parts.Length != 2) return false;
			var salt = Convert.FromBase64String(parts[0]);
			var hash = parts[1];

			// Băm mật khẩu người dùng và so sánh với hash đã lưu
			string userHashed = hashPassword(userPassword, salt);
			return userHashed == hashedPassword;
		}

	
	}
}