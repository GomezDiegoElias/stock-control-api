namespace org.pos.software.Configuration
{
    public static class CorsConfig
    {

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!;

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(optionsCORS =>
                {
                    optionsCORS
                        .WithOrigins(allowedOrigins)
                        .AllowAnyMethod().AllowAnyHeader();
                });
            });

        }
    }
}
