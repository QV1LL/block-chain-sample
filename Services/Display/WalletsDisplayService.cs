using BlockChainSample.Models;

namespace BlockChainSample.Services.Display;

public static class WalletsDisplayService
{
  public static void DisplayWallet(Wallet wallet)
  {
    Console.WriteLine($"Name: {wallet.Name}");
    Console.WriteLine($"Address: {wallet.Address}");
    Console.WriteLine($"Public Key: {string.Concat(Convert.ToBase64String(wallet.PublicKey))}");
  }
  public static void DisplayWallets(IEnumerable<Wallet> wallets)
  {
    Console.WriteLine("Wallets:");
    foreach (var wallet in wallets)
    {
      DisplayWallet(wallet);
      Console.WriteLine(new string('-', 50));
    }
  }

  public static void PrintTransactionHistory(BlockChain blockChain, Wallet wallet)
  {
    var transactions = blockChain.GetAllUserTransactions(wallet.Address);

    foreach (var transaction in transactions)
    {
      Console.WriteLine($"Id: {transaction.Id}");
      Console.WriteLine($"Timestamp: {transaction.TimeStamp}");
      Console.WriteLine($"From: {transaction.From}");
      Console.WriteLine($"To: {transaction.To}");
      Console.WriteLine($"Ammount: {transaction.Ammount}");
      Console.WriteLine(new string('-', 50));
    }
  }
}
