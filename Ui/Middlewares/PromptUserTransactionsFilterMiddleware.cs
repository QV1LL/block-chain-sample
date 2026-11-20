using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class PromptUserTransactionsFilterMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    Console.WriteLine("Enter username: ");
    var user = Console.ReadLine() ?? string.Empty;

    Console.WriteLine("Enter filter (from, to or leave empty): ");
    var filterBy = Console.ReadLine() ?? string.Empty;

    if (!new[] { "from", "to", "" }.Contains(filterBy))
      return Task.FromResult(MenuResult.Fail($"Incorrect filter by value: {filterBy}"));

    context.Set(TransactionKeys.Username, user);
    context.Set(TransactionKeys.FilterBy, filterBy);
    return next(context);
  }
}

