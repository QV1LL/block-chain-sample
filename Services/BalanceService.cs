using BlockChainSample.Models;

namespace BlockChainSample.Services;

public class BalanceService
{
  public decimal GetBalance(string address, List<Block> chain)
  {
    var balance = 0m;

    foreach (var block in chain)
    {
      foreach (var transaction in block.Transactions)
      {
        if (transaction.To == address)
          balance += transaction.Ammount;

        if (transaction.From == address)
          balance -= transaction.Ammount;
      }
    }

    return balance;
  }

  public bool HasSufficientBalance(string address, decimal ammount, List<Block> chain, IEnumerable<Transaction> transactions)
  {
    var balance = GetBalance(address, chain);

    foreach (var transaction in transactions)
      if (transaction.From == address)
        balance -= Math.Max(transaction.Ammount, 0);

    Console.WriteLine($"Balance: {balance}, ammount: {ammount}");

    return balance >= ammount;
  }
}
