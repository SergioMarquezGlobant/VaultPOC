using System;
using System.Linq;
using VaultCore;

namespace VaultConsolePOC
{
    class Program
    {
        static void Main(string[] args)
        {
            VaultOperations vaultOperations = new VaultOperations();
            var secrets =  vaultOperations.GetKVSecrets("dotnet").Result;

            Console.WriteLine(secrets.ToList());
        }
    }
}
