namespace ReUnited_Backend.Middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Middleware running");
            try
            {
                await next(context);
            }

            catch (Exception e)
            {
                await HandleException(context, e);
            }

            Console.WriteLine("Middleware finished");
        }

        private async Task HandleException(HttpContext context, Exception e)
        {
            if (e is Exception)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }

            await context.Response.WriteAsJsonAsync(new { message = $"Error: {e.Message}" });
        }
    }
}
