using System.Security.Cryptography;
using BlockChainSample.Models;

namespace BlockChainSample.Services;

public class WalletService
{
  private List<Wallet> Wallets { get; set; } = [];

  public Wallet? CreateWallet(string name)
  {
    if (Wallets.Any(w => w.Name == name)) return null;

    using var ecdsa = ECDsa.Create();
    var privateKey = ecdsa.ExportECPrivateKey();
    var publicKey = ecdsa.ExportSubjectPublicKeyInfo();
    var address = string.Concat(Convert.ToBase64String(publicKey).TakeLast(10));

    var wallet = new Wallet(name, address, publicKey, privateKey);
    Wallets.Add(wallet);

    return wallet;
  }

  public IEnumerable<Wallet> GetWallets() => [.. Wallets];

  public Wallet? FindByName(string name) => Wallets.FirstOrDefault(w => w.Name == name);

  public Wallet? FindByAddress(string address) => Wallets.FirstOrDefault(w => w.Address == address);

  public static bool VerifySignature(byte[] publicKey, byte[] data, byte[] signature)
  {
    using var ecdsa = ECDsa.Create();
    ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);

    return ecdsa.VerifyData(data, signature, HashAlgorithmName.SHA256);
  }
}
