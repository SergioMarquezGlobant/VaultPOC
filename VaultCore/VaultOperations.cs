using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Util;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Amazon.SecurityToken.Model.Internal.MarshallTransformations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AWS;

namespace VaultCore
{
    public class VaultOperations
    {
        private string GetEncodedHeaders()
        {
            var amazonSecurityTokenServiceConfig = new AmazonSecurityTokenServiceConfig();
            var chain = new CredentialProfileStoreChain("/home/app/.aws/credentials");
            if (!chain.TryGetAWSCredentials("sdc-horizontal:Developer", out var awsCredentials))
                return string.Empty;

            // use awsCredentials
            var iamRequest = GetCallerIdentityRequestMarshaller.Instance.Marshall(new GetCallerIdentityRequest());

            iamRequest.Endpoint = new Uri(amazonSecurityTokenServiceConfig.DetermineServiceURL());
            iamRequest.ResourcePath = "/";

            iamRequest.Headers.Add("X-Amz-Security-Token", awsCredentials.GetCredentials().Token);
            iamRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");

            new AWS4Signer()
                .Sign(
                    iamRequest,
                    amazonSecurityTokenServiceConfig,
                    new RequestMetrics(),
                    awsCredentials.GetCredentials().AccessKey,
                    awsCredentials.GetCredentials().SecretKey
                );

            var iamStsRequestHeaders = iamRequest.Headers;

            var base64EncodedIamRequestHeaders =
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(iamStsRequestHeaders)));

            return base64EncodedIamRequestHeaders;
        }

        public async Task<IEnumerable<KeyValuePair<string, object>>> GetKVSecrets(string path)
        {
            try
            {
                IAuthMethodInfo authMethod = new IAMAWSAuthMethodInfo(GetEncodedHeaders(), roleName: "horizontal_developer");
                var vaultClientSettings = new VaultClientSettings("https://vault-nonprod.smileco.cloud:8200", authMethod);

                IVaultClient vaultClient = new VaultClient(vaultClientSettings);

                var kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("dotnet", mountPoint: "kv");

                return kv2Secret.Data.Data;
            }
            catch (Exception ex)
            {
                //log here
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
