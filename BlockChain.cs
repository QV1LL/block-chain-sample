using BlockChainSample;

namespace BlockChainP32;

public class BlockChain
{
  public List<Block> Chain { get; set; }

  private readonly HashingService _hashingService;

  public BlockChain()
  {
    Chain = new List<Block>();
    _hashingService = new HashingService();
    AddGenesysBlock();
  }

  private void AddGenesysBlock()
  {
    var genesysBlock = new Block
    {
      Index = 0,
      TimeStamp = DateTime.UtcNow,
      Data = "",
      PreviousHash = "0"
    };
    genesysBlock.Hash = _hashingService.ComputeHash(genesysBlock);
    Chain.Add(genesysBlock);
  }

  public void AddBlock(string data)
  {
    var lastBlock = Chain[Chain.Count - 1];
    var newBlock = new Block
    {
      Index = lastBlock.Index + 1,
      TimeStamp = DateTime.UtcNow,
      Data = data,
      PreviousHash = lastBlock.Hash
    };
    newBlock.Hash = _hashingService.ComputeHash(newBlock);
    Chain.Add(newBlock);
  }

  public bool IsChainValid()
  {
    for (int i = 1; i < Chain.Count; i++)
    {
      var currentBlock = Chain[i];
      var previousBlock = Chain[i - 1];
      if (currentBlock.Hash != _hashingService.ComputeHash(currentBlock))
      {
        return false;
      }
      if (currentBlock.PreviousHash != previousBlock.Hash)
      {
        return false;
      }
    }
    return true;
  }

  public Block? FindByIndex(int index)
  {
    if (index < 0 || Chain.Count <= index)
    {
      return null;
    }

    return Chain[index];
  }

  public Block? FindByEvent(string text) {
    foreach (Block block in Chain)
        if (block.Data.Contains(text))
          return block;

    return null;
  }
}
