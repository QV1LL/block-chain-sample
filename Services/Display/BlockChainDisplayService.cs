using BlockChainSample.Models;

namespace BlockChainSample.Services.Display;

public static class BlockChainDisplayService
{
  public static void DisplayDeletedOverdueTransactions(IEnumerable<Transaction> transactions)
  {
    Console.WriteLine("Deleted overdue transactions: ");

    foreach (var transaction in transactions)
    {
      Console.WriteLine(transaction);
    }
  }
  public static void DisplayInfo(string text)
  {
    Console.WriteLine($"[{DateTimeOffset.Now}] Message from blockchain: {text}");
  }

  public static void DisplayTransaction(Transaction transaction)
  {
    Console.WriteLine($" - {transaction.ToRaw()}");
  }

  public static void DisplayCorruptedTransaction(Guid id, int position, string hashAtAdd, string currentHash)
  {
    Console.WriteLine($" --- Founded corrupted transaction with id {id} at {position}: ");
    Console.WriteLine($"Hash at add: {hashAtAdd}, current hash: {currentHash}");
  }
  public static void DisplayBlock(Block block)
  {
    Console.WriteLine(block);
  }
  public static void DisplayChain(BlockChain blockChain)
  {
    Console.WriteLine("Blockchain:");
    foreach (var block in blockChain.Chain)
    {
      DisplayBlock(block);
      Console.WriteLine(new string('-', 50));
    }
  }

  public static void DisplayDifficultyHistory(BlockChain blockChain)
  {
    foreach (Block block in blockChain.Chain)
    {
      Console.WriteLine($"\nBlock {block.Index}");
      Console.WriteLine($"Difficulty: {block.DifficultyAtMining}");
      Console.WriteLine($"Attempts: {block.Nonce}");
      Console.WriteLine($"MiningDuration: {block.MiningDuration}");
    }
  }
}
