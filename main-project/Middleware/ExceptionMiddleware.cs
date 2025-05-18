namespace main_project.Middleware
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Validation error occurred");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error_message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "unhandled error occurred");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError; 

                await context.Response.WriteAsJsonAsync(new { error_message = "something went wrong" });
            }
        }
    }
}
