public class UnauthorizedRedirectMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            switch (context.Response.StatusCode)
            {
                case 401:
                    context.Response.Redirect("/Auth/Login");
                    break;

                case 403: // Không có quyền
                    if (!context.User.Identity?.IsAuthenticated ?? true)
                    {
                        context.Response.Redirect("/Auth/Login");
                        break;
                    }

                    // Kiểm tra role và chuyển hướng
                    if (context.User.IsInRole("Admin"))
                        context.Response.Redirect("/Auth/Logout");
                    else if (context.User.IsInRole("Customer")) context.Response.Redirect("/Admin/Logout");
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            context.Response.Redirect("/Error");
        }
    }
}