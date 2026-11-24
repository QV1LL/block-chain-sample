using System.Text;

namespace BlockChainSample.Models;

public class Transaction(string from, decimal ammount, string to)
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string From { get; set; } = from;
  public decimal Ammount { get; set; } = ammount;
  public string To { get; set; } = to;
  public byte[] SenderPublicKey { get; set; }
  public byte[]? Signature { get; set; }
  public DateTime TimeStamp { get; set; } = DateTime.Now;
  public DateTime AddedAt { get; set; }
  public bool IsCoinBase { get; set; } = false;

  public static Transaction CreateCoinbaseTransaction(string to, decimal ammount)
  {
    return new Transaction("COINBASE", ammount, to) { IsCoinBase = true };
  }

  public byte[] GetDataToSign()
  {
    var data = $"{Id}{From}{Ammount}{To}{TimeStamp}";

    return Encoding.UTF8.GetBytes(data);
  }

  public string ToRaw()
  {
    return $"{Id}\t{From}\t{Ammount}\t{To}\t{TimeStamp}";
  }

  public override string ToString()
  {
    return $"Transaction ID: {Id} | From: {From} | To: {To} | Amount: {Ammount} | Timestamp: {TimeStamp}";
  }
}
