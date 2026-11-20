using System.Security.Cryptography;

namespace BlockChainSample.Models;

public class Wallet(string name, string address, byte[] publicKey, byte[] privateKey)
{
  public string Name { get; set; } = name;
  public string Address { get; set; } = address;
  public byte[] PublicKey { get; set; } = publicKey;
  public byte[] PrivateKey { get; set; } = privateKey;

  public byte[] Sign(byte[] data)
  {
    using var ecdsa = ECDsa.Create();
    ecdsa.ImportECPrivateKey(PrivateKey, out _);

    return ecdsa.SignData(data, HashAlgorithmName.SHA256);
  }

  public override string ToString()
  {
    return $"""
    Name: {Name}
    Address: {Address}
    Public key: {PublicKey}
    """;
  }
}
