using System.Security.Cryptography;

namespace Ciara.Shared.Utils;

public static class Hash
{
    public static byte[] HashId(ulong id)
    {
        var bytes = BitConverter.GetBytes(id);
        return SHA256.HashData(bytes);
    }
}