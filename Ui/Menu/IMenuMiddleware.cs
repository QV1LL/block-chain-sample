namespace BlockChainSample.Ui.Menu;

public delegate Task<MenuResult> MenuDelegate(MenuContext context);
public interface IMenuMiddleware
{
  Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next);
}

