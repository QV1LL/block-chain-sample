using BlockChainSample.Models;

namespace BlockChainSample.Services.Display;

public class WalletsDisplayService
{
  public void DisplayWallet(Wallet wallet)
  {
    Console.WriteLine($"Name: {wallet.Name}");
    Console.WriteLine($"Address: {wallet.Address}");
    Console.WriteLine($"Public Key: {string.Concat(Convert.ToBase64String(wallet.PublicKey))}");
  }
  public void DisplayWallets(IEnumerable<Wallet> wallets)
  {
    Console.WriteLine("Wallets:");
    foreach (var wallet in wallets)
    {
      DisplayWallet(wallet);
      Console.WriteLine(new string('-', 50));
    }
  }
}
