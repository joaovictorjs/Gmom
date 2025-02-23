using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Gmom.Domain.Constants;
using Gmom.Domain.Interface;

namespace Gmom.Infrastructure.Services;

public class ConnectionFileService(IPostgresConnectionStore connectionStore)
    : IConnectionFileService
{
    public void Write()
    {
        var data = new Dictionary<string, string>
        {
            { "Endereço", connectionStore.Value.Host },
            { "Porta", connectionStore.Value.Port },
            { "Base de dados", connectionStore.Value.Database },
            { "Usuário", Protect(connectionStore.Value.Username) },
            { "Senha", Protect(connectionStore.Value.Password) },
        };

        File.WriteAllLines(Filenames.Connection, data.Select(it => $"{it.Key}={it.Value}"));
    }

    [SuppressMessage("Interoperability", "CA1416:Validar a compatibilidade da plataforma")]
    private static string Protect(string content)
    {
        var encoded = Encoding.UTF8.GetBytes(content);
        var hash = ProtectedData.Protect(encoded, null, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(hash);
    }

    public void Read()
    {
        var data = File.ReadAllLines(Filenames.Connection)
            .Select(it => it.Split('=', 2))
            .Where(it => it.Length == 2)
            .ToDictionary(it => it[0], it => it[1]);

        connectionStore.Value.Host = data["Endereço"];
        connectionStore.Value.Port = data["Porta"];
        connectionStore.Value.Database = data["Base de dados"];
        connectionStore.Value.Username = Unprotect(data["Usuário"]);
        connectionStore.Value.Password = Unprotect(data["Senha"]);
    }

    [SuppressMessage("Interoperability", "CA1416:Validar a compatibilidade da plataforma")]
    private static string Unprotect(string content)
    {
        var hash = Convert.FromBase64String(content);
        var unprotected = ProtectedData.Unprotect(hash, null, DataProtectionScope.CurrentUser);
        return Encoding.UTF8.GetString(unprotected);
    }
}
