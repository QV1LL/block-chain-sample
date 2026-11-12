namespace BlockChainSample;

public class BlockChainDisplayService
{
  public void DisplayChain(BlockChain blockChain)
  {
    Console.WriteLine("Blockchain:");
    foreach (var block in blockChain.Chain)
    {
      Console.WriteLine($"Index: {block.Index}");
      Console.WriteLine($"Timestamp: {block.TimeStamp}");
      Console.WriteLine($"Data: {block.Data}");
      Console.WriteLine($"Previous Hash: {block.PreviousHash}");
      Console.WriteLine($"Hash: {block.Hash}");
      Console.WriteLine($"Nonce: {block.Nonce}");
      Console.WriteLine(new string('-', 50));
    }
  }

  public void DisplayDifficultyHistory(BlockChain blockChain)
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
