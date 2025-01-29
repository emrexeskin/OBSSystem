    using OBSSystem.Application.Services;

    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenBlacklistMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                using (var scope = _serviceScopeFactory.CreateScope()) // Yeni Scope oluştur
                {
                    var authService = scope.ServiceProvider.GetRequiredService<AuthService>(); // Scoped servisi burada çöz

                    if (authService.IsTokenBlacklisted(token))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token has been invalidated.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
