namespace BlockChainSample.Models;

public class Block
{
  public int Index { get; set; }
  public DateTime TimeStamp { get; set; }
  public List<Transaction> Transactions { get; set; }
  public string Author { get; set; }
  public string Hash { get; set; }
  public string PreviousHash { get; set; }
  public int Nonce { get; set; }
  public string MerkleRoot { get; set; }
  public double MiningDuration { get; set; }
  public int DifficultyAtMining { get; set; }

  public override string ToString()
  {
    return $"""
    Index: {Index}
    Timestamp: {TimeStamp}
    Previous Hash: {PreviousHash}
    Hash: {Hash}
    Nonce: {Nonce}
    Merkle root: {MerkleRoot}
    Difficulty at mining: {DifficultyAtMining}
    Mining duration: {MiningDuration} seconds
    Transactions:
    {string.Join('\n', Transactions.Select(t => $" --- {t}"))}
    """;
  }
}
