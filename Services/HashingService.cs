using System.Security.Cryptography;
using System.Text;
using BlockChainSample.Models;

namespace BlockChainSample.Services;

public class HashingService
{
  public string ComputeHash(Block block, int? nonce = null)
  {
    var transactionRawData = string.Concat(block.Transactions.Select(t => t.ToRaw()));
    var blockData = $"{block.Index}{block.TimeStamp}{transactionRawData}{block.PreviousHash}{nonce ?? block.Nonce}";

    return ComputeHash(blockData);
  }

  public string ComputeHash(string data)
  {
    using var sha256 = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(data);
    var hashBytes = sha256.ComputeHash(bytes);

    return Convert.ToBase64String(hashBytes).Replace("-", "").ToLower();
  }
}
