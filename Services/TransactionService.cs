
using System.Security.Cryptography;
using BlockChainSample.Models;

namespace BlockChainSample.Services;

public class TransactionService
{
  private readonly List<Transaction> _pendingTransactions = [];

  public int PendingCount => _pendingTransactions.Count;

  public List<Transaction> GetPendingTransactions()
  {
    return [.. _pendingTransactions];
  }

  public void AddPending(Transaction transaction)
  {
    ArgumentNullException.ThrowIfNull(transaction);
    _pendingTransactions.Add(transaction);
  }

  public (bool isAdded, string? errorMessage) CreateAndAddPending(Wallet sender, string from, string to, decimal amount)
  {
    var (errorMessage, transaction) = CreateTransaction(sender, from, to, amount);

    if (errorMessage != null || transaction == null)
      return (false, errorMessage ?? "Failed to create transaction.");

    _pendingTransactions.Add(transaction);
    return (true, null);
  }

  public bool RemovePending(Guid transactionId)
  {
    var transaction = _pendingTransactions.FirstOrDefault(t => t.Id == transactionId);
    if (transaction == null)
      return false;

    return _pendingTransactions.Remove(transaction);
  }

  public void ClearPending()
  {
    _pendingTransactions.Clear();
  }

  public static (string? errorMessage, Transaction? transaction) CreateTransaction(Wallet sender, string from, string to, decimal ammount)
  {
    var transaction = new Transaction(from, ammount, to)
    {
      SenderPublicKey = sender.PublicKey
    };

    transaction.Signature = sender.Sign(transaction.GetDataToSign());

    return (null, transaction);
  }

  public static (bool isValid, string errorMessage) ValidateTransaction(Transaction tx)
  {
    if (tx.IsCoinBase)
      return (true, string.Empty);

    if (string.IsNullOrEmpty(tx.From))
      return (false, "Invalid sender address");

    if (string.IsNullOrEmpty(tx.To))
      return (false, "Invalid recipient address");

    if (tx.Ammount <= 0)
      return (false, "Invalid transaction amount");

    if (tx.Signature == null || tx.SenderPublicKey == null)
      return (false, "Transaction must have a signature and sender public key");

    using var ecdsa = ECDsa.Create();
    ecdsa.ImportSubjectPublicKeyInfo(tx.SenderPublicKey, out _);

    if (!ecdsa.VerifyData(tx.GetDataToSign(), tx.Signature, HashAlgorithmName.SHA256))
      return (false, "Transaction signature is not correct!");

    return (true, string.Empty);
  }
}
