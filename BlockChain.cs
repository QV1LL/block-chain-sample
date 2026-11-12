namespace BlockChainSample;

public class BlockChain
{
  public List<Block> Chain { get; set; }
  private int Difficulty { get; set; } = 1;
  private readonly HashingService _hashingService;
  private readonly MiningService _miningService;
  private readonly int _adjustmentInterval = 3;
  private readonly int _targetBlockTime = 1;

  public BlockChain()
  {
    Chain = [];
    _hashingService = new();
    _miningService = new(_hashingService);
    AddGenesysBlock();
  }

  private void AddGenesysBlock()
  {
    var genesysBlock = new Block
    {
      Index = 0,
      TimeStamp = DateTime.UtcNow,
      Data = "",
      PreviousHash = "0",
      DifficultyAtMining = Difficulty
    };

    _miningService.MineBlock(genesysBlock, Difficulty);
    Chain.Add(genesysBlock);
  }

  public void AddBlock(string data)
  {
    Console.WriteLine($"Current difficulty {Difficulty}");
    var lastBlock = Chain[^1];
    var newBlock = new Block
    {
      Index = lastBlock.Index + 1,
      TimeStamp = DateTime.UtcNow,
      Data = data,
      PreviousHash = lastBlock.Hash,
      DifficultyAtMining = Difficulty
    };

    _miningService.MineBlock(newBlock, Difficulty);
    Chain.Add(newBlock);

    if (newBlock.Index % _adjustmentInterval == 0)
      AdjustDifficulty();
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
      if (currentBlock.Hash.Substring(0, currentBlock.DifficultyAtMining) != new string('0', currentBlock.DifficultyAtMining))
        return false;
    }
    return true;
  }

  public (Block, string)? FindFirstInvalidBlock()
  {
    for (int i = 1; i < Chain.Count; i++)
    {
      var currentBlock = Chain[i];
      var previousBlock = Chain[i - 1];

      if (currentBlock.Data == string.Empty)
        continue;

      if (currentBlock.Hash != _hashingService.ComputeHash(currentBlock))
        return (currentBlock, "Invalid hash");

      if (currentBlock.PreviousHash != previousBlock.Hash)
        return (currentBlock, "Incorrect previous hash");

      if (currentBlock.Hash.Substring(0, currentBlock.DifficultyAtMining) != new string('0', currentBlock.DifficultyAtMining))
        return (currentBlock, "Incorrect difficulty");
    }
    return null;
  }

  // TODO
  public void RepairChain(int startIndex)
  {
    var chainToRepair = Chain[startIndex..];


  }

  private void AdjustDifficulty()
  {
    var recent = Chain.Where(b => b.Index > 0).TakeLast(_adjustmentInterval).ToList();

    if (recent.Count == 0) return;

    var avgTime = recent.Average(b => b.MiningDuration);

    if (avgTime < _targetBlockTime)
    {
      Difficulty++;
    }
    else if (avgTime > _targetBlockTime)
    {
      Difficulty = Math.Max(1, Difficulty - 1);
    }
  }

  public Block? FindByIndex(int index)
  {
    if (index < 0 || Chain.Count <= index)
    {
      return null;
    }

    return Chain[index];
  }

  public Block? FindByEvent(string text)
  {
    foreach (Block block in Chain)
      if (block.Data.Contains(text))
        return block;

    return null;
  }
}
