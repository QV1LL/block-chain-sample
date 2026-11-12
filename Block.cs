namespace BlockChainSample;

public class Block
{
  public int Index { get; set; }
  public DateTime TimeStamp { get; set; }
  public string Data { get; set; }
  public string Author { get; set; }
  public string Hash { get; set; }
  public string PreviousHash { get; set; }
  public int Nonce { get; set; }
  public double MiningDuration { get; set; }
  public int DifficultyAtMining { get; set; }
}
