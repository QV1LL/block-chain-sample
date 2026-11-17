
using System.Security.Cryptography;
using BlockChainSample.Models;

namespace BlockChainSample.Services;

public class TransactionService
{
  public (string? errorMessage, Transaction? transaction) CreateTransaction(Wallet sender, string from, string to, decimal ammount)
  {
    var transaction = new Transaction(from, ammount, to)
    {
      SenderPublicKey = sender.PublicKey
    };

    transaction.Signature = sender.Sign(transaction.GetDataToSign());

    return (null, transaction);
  }

  public (bool isValid, string errorMessage) ValidateTransaction(Transaction tx)
  {
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

    if (!ecdsa.VerifyData(tx.GetDataToSign(), tx.SenderPublicKey, HashAlgorithmName.SHA256))
      return (false, "Transaction signature is not correct!");

    return (true, string.Empty);
  }
}
