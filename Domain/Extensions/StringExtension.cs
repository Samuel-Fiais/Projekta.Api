using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Extensions;

public static class StringExtension
{
    public static string Encrypt(this string? str)
    {
        UnicodeEncoding encoding = new();
        byte[] hashBytes;
        hashBytes = SHA1.HashData(encoding.GetBytes(str!));

        var value = new StringBuilder(hashBytes.Length * 2);
        foreach (var b in hashBytes)
            value.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);

        return value.ToString();
    }
}
