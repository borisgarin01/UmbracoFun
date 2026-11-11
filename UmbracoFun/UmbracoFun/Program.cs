using Microsoft.OpenApi;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Include XML comments if you have them
            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // options.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddControllers();

        builder.CreateUmbracoBuilder()
            .AddDeliveryApi()
            .AddBackOffice()
            .AddWebsite()
            .AddComposers()
            .Build();

        WebApplication app = builder.Build();

        await app.BootUmbracoAsync();

        app.UseUmbraco()
            .WithMiddleware(u =>
            {
                u.UseBackOffice();
                u.UseWebsite();
            })
            .WithEndpoints(u =>
            {
                u.UseDeliveryApiEndpoints();
                u.UseBackOfficeEndpoints();
                u.UseWebsiteEndpoints();
            });

        await app.RunAsync();
    }
}