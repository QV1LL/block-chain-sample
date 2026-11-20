using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class AddTransactionMiddleware(TransactionService transactionService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var fromWallet = context.Get<Wallet>(WalletKeys.FromWallet);
    var toWallet = context.Get<Wallet>(WalletKeys.ToWallet);
    var amount = context.Get<decimal>(TransactionKeys.Amount);

    var (errorMessage, transaction) = TransactionService.CreateTransaction(
      fromWallet,
      fromWallet.Address,
      toWallet.Address.ToString(),
      amount
    );

    if (errorMessage != null)
      return Task.FromResult(MenuResult.Fail($"Error while adding transaction: {errorMessage}"));

    transactionService.AddPending(transaction!);

    return Task.FromResult(MenuResult.Ok("Transaction added to pending list."));
  }
}

