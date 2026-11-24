using System.Security.Cryptography;
using System.Text;
using BlockChainSample.Models;

namespace BlockChainSample.Services;

public class HashingService
{
  public string ComputeHash(Block block, int? nonce = null)
  {
    var blockData = $"{block.Index}{block.TimeStamp}{block.DifficultyAtMining}{block.MerkleRoot}{string.Join('|', block.TransactionHashesAtAdd)}{block.PreviousHash}{nonce ?? block.Nonce}";

    return ComputeHash(blockData);
  }

  public string ComputeHash(string data)
  {
    using var sha256 = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(data);
    var hashBytes = sha256.ComputeHash(bytes);

    return Convert.ToBase64String(hashBytes).Replace("-", "").ToLower();
  }

  public List<(string Hash, bool IsLeft)>? GetMerkleProof(List<string> hashes, Transaction transaction)
  {
    var targetIndex = hashes.FindIndex(h => h == ComputeHash(transaction.ToRaw()));

    if (targetIndex == -1) return null;

    var proof = new List<(string Hash, bool IsLeft)>();

    while (hashes.Count > 1)
    {
      var newHashes = new List<string>();

      for (int i = 0; i < hashes.Count; i += 2)
      {
        if (i + 1 < hashes.Count)
        {
          if (i == targetIndex)
            proof.Add((hashes[i + 1], false));
          else if (i + 1 == targetIndex)
            proof.Add((hashes[i], true));

          var combinedHash = ComputeHash(hashes[i] + hashes[i + 1]);
          newHashes.Add(combinedHash);
        }
        else
        {
          newHashes.Add(hashes[i]);
        }
      }

      targetIndex /= 2;
      hashes = newHashes;
    }

    return proof;
  }
  public string ComputeMerkleRoot(List<Transaction> transactions)
  {
    if (transactions == null || transactions.Count == 0)
      return string.Empty;

    var hashes = transactions.Select(t => ComputeHash(t.ToRaw())).ToList();

    while (hashes.Count > 1)
    {
      var newHashes = new List<string>();

      for (int i = 0; i < hashes.Count; i += 2)
      {
        if (i + 1 < hashes.Count)
        {
          var combinedHash = ComputeHash(hashes[i] + hashes[i + 1]);
          newHashes.Add(combinedHash);
        }
        else
          newHashes.Add(hashes[i]);
      }

      hashes = newHashes;
    }

    return hashes[0];
  }
}
