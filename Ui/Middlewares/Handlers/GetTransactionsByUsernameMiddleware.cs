using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class GetTransactionsByUsernameMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var user = context.Get<string>(TransactionKeys.Username);
    var filterBy = context.Get<string>(TransactionKeys.FilterBy);

    var userTransactions = blockChain.GetAllUserTransactions(user, filterBy);
    var filterByString = filterBy == "" ? string.Empty : $" (Filter by {filterBy}) ";

    Console.WriteLine($"User transactions{filterByString}:");
    foreach (var tx in userTransactions)
      Console.WriteLine(tx);

    return Task.FromResult(MenuResult.Ok());
  }
}

