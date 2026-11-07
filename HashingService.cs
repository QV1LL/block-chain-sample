using System.Security.Cryptography;
using System.Text;

namespace BlockChainSample;

public class HashingService
{
  public string ComputeHash(Block block)
  {
    var blockData = $"{block.Index}{block.TimeStamp}{block.Data}{block.Author}{block.PreviousHash}";

    return ComputeHash(blockData);
  }

  public string ComputeHash(string data)
  {
    using var sha256 = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(data);
    var hashBytes = sha256.ComputeHash(bytes);

    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
  }
}
