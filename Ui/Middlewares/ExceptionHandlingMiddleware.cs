using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class ExceptionHandlingMiddleware : IMenuMiddleware
{
  public async Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    try
    {
      return await next(context);
    }
    catch (Exception ex)
    {
      var lines = new List<string>
    {
      $"Unexpected error ({ex.GetType().Name}): {ex.Message}"
    };

      if (ex.InnerException != null)
        lines.Add($"Inner error ({ex.InnerException.GetType().Name}): {ex.InnerException.Message}");

      var site = ex.TargetSite != null
        ? $"{ex.TargetSite.DeclaringType?.Name}.{ex.TargetSite.Name}"
        : "Unknown";
      lines.Add($"Source: {site}");

      return MenuResult.Fail(string.Join(Environment.NewLine, lines));
    }
  }
}

