using System.Security.Cryptography;

namespace CleanArc.Infrastructure.Identity.Identity.DataProtection;

public enum ProtectorAlgorithm
{
    Aes256Hmac512 = 1
}

public static class ProtectorAlgorithmHelper
{
    public static ProtectorAlgorithm DefaultAlgorithm
    {
        get { return ProtectorAlgorithm.Aes256Hmac512; }
    }

    public static void GetAlgorithms(
        ProtectorAlgorithm algorithmId,
        out SymmetricAlgorithm encryptionAlgorithm,
        out KeyedHashAlgorithm signingAlgorithm,
        out int keyDerivationIterationCount)
    {
        switch (algorithmId)
        {
            case ProtectorAlgorithm.Aes256Hmac512:
                encryptionAlgorithm = Aes.Create();
                encryptionAlgorithm.KeySize = 256;
                signingAlgorithm = new HMACSHA512();
                keyDerivationIterationCount = 10000;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(algorithmId));
        }
    }
}