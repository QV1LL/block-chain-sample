using BlockChainP32;

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
      Console.WriteLine(new string('-', 50));
    }
  }
}
