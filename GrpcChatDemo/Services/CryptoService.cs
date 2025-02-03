using System.Text.Json;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.DataProtection;

namespace GrpcChatDemo.Services;

public interface ICryptoService
{
    Task<string> EncryptData(string data);
    Task<string> DecryptData(string data);
}

public class CryptoService(IAmazonSecretsManager secretsManager, IDataProtectionProvider  dataProtectionProvider) : ICryptoService
{
    private const string SecretName = "dev/chatapp/secret";

    public async Task<string> EncryptData(string data)
    {
        var awsKey = await GetSecretValue(SecretName, "");
        var protector = dataProtectionProvider.CreateProtector(awsKey);
        return protector.Protect(data);
    }

    public async Task<string> DecryptData(string data)
    {
        var awsKey = await GetSecretValue(SecretName, "");
        var protector = dataProtectionProvider.CreateProtector(awsKey);
        return protector.Unprotect(data);
    }

    private async Task<string> GetSecretValue(string secretName, string secretParameter)
    {
        var request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
        };

        try
        {
            var secret = await secretsManager.GetSecretValueAsync(request);
            if (string.IsNullOrEmpty(secret.SecretString))
                throw new ConfigurationException("AWS secret issue.");
            
            return JsonSerializer.Deserialize<CryptoSecretModel>(secret.SecretString)?.CryptoSecret;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private class CryptoSecretModel
    {
        public string CryptoSecret { get; set; }
    }
}