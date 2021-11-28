using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OtpNet;

namespace TotpCodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string totpPath = Path.Combine(Path.GetTempPath(), "totp.txt");
            string secret = @"secret";

            var dictionary = args.Select(a => a.Split(new[] { '=' }, 2))
                     .GroupBy(a => a[0], a => a.Length == 2 ? a[1] : null)
                     .ToDictionary(g => g.Key, g => g.FirstOrDefault());

            if (dictionary.ContainsKey("path"))
                totpPath = dictionary["path"];
            if (dictionary.ContainsKey("secret"))
                secret = dictionary["secret"];
            var totp = new Totp(Base32Encoding.ToBytes(secret));
            var totpToken = totp.ComputeTotp();

#if DEBUG
            Console.WriteLine($"Writing 2fa code to: {totpPath}");
            Console.WriteLine($"Using secret: {secret}");
            Console.WriteLine($"Totp Token = {totpToken}");
            Console.ReadKey();
#endif

            File.WriteAllText(totpPath, totpToken);
        }
    }
}
