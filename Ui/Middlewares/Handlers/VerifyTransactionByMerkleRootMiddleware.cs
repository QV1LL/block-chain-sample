using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class VerifyTransactionByMerkleRootMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var transaction = context.Get<Transaction>(TransactionKeys.Transaction);
    var blockIndex = context.Get<int>(BlockchainKeys.Index);

    var proof = blockChain.GetMerkleProof(transaction.Id);
    var isValid = blockChain.VerifyMerkleProof(transaction, blockIndex, proof ?? []);

    Console.WriteLine($"Transaction is {(isValid ? "in" : "not in")} blockchain");

    return next(context);
  }
}

