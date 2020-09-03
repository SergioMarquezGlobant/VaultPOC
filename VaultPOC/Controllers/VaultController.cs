using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaultCore;

namespace VaultPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaultController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KeyValuePair<string, object>>>> GetKeys()
        {
            //IAuthMethodInfo authMethod = new TokenAuthMethodInfo("s.4wWvlidgQDHRoceOmXqxlatN");

            //var vaultClientSettings = new VaultClientSettings("https://vault-nonprod.smileco.cloud:8200", authMethod);

            //IVaultClient vaultClient = new VaultClient(vaultClientSettings);


            //var kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("dotnet", mountPoint:"kv");
            ////Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("dotnet");

            //return kv2Secret.Data.Data.ToList();
            VaultOperations vaultOperations = new VaultOperations();
            var secrets = await vaultOperations.GetKVSecrets("dotnet");

            return secrets.ToList();
        }
    }
}