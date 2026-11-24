
using System.Security.Cryptography;
using BlockChainSample.Models;

namespace BlockChainSample.Services;

public static class TransactionService
{
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

    if (Convert.ToBase64String(tx.SenderPublicKey)[^10..] != tx.From)
      return (false, "Transaction from must contains public key");

    return (true, string.Empty);
  }
}
