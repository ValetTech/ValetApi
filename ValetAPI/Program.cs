using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Newtonsoft.Json.Converters;
using ValetAPI.Data;
using ValetAPI.Hubs;
using ValetAPI.Models;
using ValetAPI.Services;

namespace ValetAPI;
/// <summary>
/// Entry to the program
/// </summary>
public static class Program
{
    /// <summary>
    /// Main
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR();

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        
        
        // builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseSqlServer(connectionString));
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));



        //
        //
        // var mySqlConnectionString = builder.Configuration.GetConnectionString("MySQLConnection") ??
        //                        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        // builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseMySql(mySqlConnectionString));


        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers()
            .AddNewtonsoftJson()
            .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new StringEnumConverter()));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo {Title = "ValetAPI", Version = "v1"});
            options.SchemaFilter<SwaggerSchemaFilter>();
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        builder.Services.AddSwaggerGenNewtonsoftSupport();

        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new MediaTypeApiVersionReader();
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
        });

        builder.Services.AddAutoMapper(options => options.AddProfile<MapperProfile>());


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowMyApp", policy => policy.AllowAnyOrigin());
            // options.AddPolicy("AllowMyApp", policy => policy.WithOrigins("https://example.com"));
        });

        builder.Services.AddScoped<IAreaService, DefaultAreaService>();
        builder.Services.AddScoped<ICustomerService, DefaultCustomerService>();
        builder.Services.AddScoped<IReservationService, DefaultReservationService>();
        builder.Services.AddScoped<ISittingService, DefaultSittingService>();
        builder.Services.AddScoped<ITableService, DefaultTableService>();
        builder.Services.AddScoped<IVenueService, DefaultVenueService>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errorResponse = new ApiError(context.ModelState);
                return new BadRequestObjectResult(errorResponse);
            };
        });

        var app = builder.Build();

        app.UseDefaultFiles();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseCors("AllowMyApp");


        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });



        app.MapRazorPages();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
            options.DefaultModelExpandDepth(-1);
            options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
        });
        app.UseSwagger(options =>
        {
            options.SerializeAsV2 = true;
        });

        app.MapControllers();

        app.MapHub<ReservationHub>("/hubs/reservation");

        app.Run();
    }
}

internal class SwaggerSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var keys = new List<string>();
        const string prefix = "_";
        const string suffix = "Entity";
        foreach (var key in context.SchemaRepository.Schemas.Keys)
        {
            if (key.EndsWith(suffix) || key.StartsWith(prefix))
            {
                keys.Add(key);
            }
        }
        foreach (var key in keys)
        {
            context.SchemaRepository.Schemas.Remove(key);
        }
    }
}