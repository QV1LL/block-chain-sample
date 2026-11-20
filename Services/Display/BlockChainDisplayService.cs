using BlockChainSample.Models;

namespace BlockChainSample.Services.Display;

public static class BlockChainDisplayService
{
  public static void DisplayInfo(string text)
  {
    Console.WriteLine($"[{DateTimeOffset.Now}] Message from blockchain: {text}");
  }

  public static void DisplayTransaction(Transaction transaction)
  {
    Console.WriteLine($" - {transaction.ToRaw()}");
  }
  public static void DisplayBlock(Block block)
  {
    Console.WriteLine($"Index: {block.Index}");
    Console.WriteLine($"Timestamp: {block.TimeStamp}");
    Console.WriteLine($"Previous Hash: {block.PreviousHash}");
    Console.WriteLine($"Hash: {block.Hash}");
    Console.WriteLine($"Nonce: {block.Nonce}");
    Console.WriteLine($"Difficulty at mining: {block.DifficultyAtMining}");
    Console.WriteLine($"Mining duration: {block.MiningDuration} seconds");
    Console.WriteLine("Transactions: ");
    foreach (var tx in block.Transactions)
    {
      DisplayTransaction(tx);
    }
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
