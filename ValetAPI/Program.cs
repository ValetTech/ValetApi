using System.Configuration;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.SwaggerGen;
using ValetAPI.Data;
using ValetAPI.Filters;
using ValetAPI.Hubs;
using ValetAPI.Models;
using ValetAPI.Services;

namespace ValetAPI;

/// <summary>
///     Entry to the program
/// </summary>
public static class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR();

        
        // DATABASE
        #region DATABASE
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        #endregion


        
        // AUTHENTICATION 
        #region AUTHENICATION
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        builder.Services.AddControllersWithViews();

        builder.Services.AddAuthorization();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
            };
        });;
        
        // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        //         options => builder.Configuration.Bind("JwtSettings", options))
        //     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        //         options => builder.Configuration.Bind("CookieSettings", options));
        
        // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));
        
        // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddMicrosoftIdentityWebApi(options =>
        //         {
        //             builder.Configuration.Bind("AzureAdB2C", options);
        //
        //             options.TokenValidationParameters.NameClaimType = "name";
        //         },
        //         options => { builder.Configuration.Bind("AzureAdB2C", options); });
        
        // builder.Services.AddAuthentication("Bearer")
        //     .AddJwtBearer("Bearer", options =>
        //     {
        //         options.Authority = "https://localhost:5001";
        //
        //         options.Audience = "scope1";
        //
        //         options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        //         {
        //             ValidateAudience = false
        //         };
        //
        //     });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowMyApp", policy => policy.AllowAnyOrigin());
            // options.AddPolicy("AllowMyApp", policy => policy.WithOrigins("https://example.com"));
        });
        #endregion
        
       

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })
            .AddNewtonsoftJson()
            .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new StringEnumConverter()));

        builder.Services.AddControllers(options =>
            {
                options.Filters.Add<JsonExceptionFilter>();
                options.Filters.Add<HttpResponseExceptionFilter>();
            }
            );
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        // API

        #region API

        builder.Services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions((apiDescriptions )=>apiDescriptions.First());
            options.SwaggerDoc("v1", new OpenApiInfo {Title = "ValetAPI", Version = "v1", Description = "Version 1 of Valet API"});
            options.SwaggerDoc("v2", new OpenApiInfo {Title = "ValetAPI", Version = "v2", Description = "Version 2 of Valet API"});
            options.SchemaFilter<SwaggerSchemaFilter>();
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            
        });
        builder.Services.AddSwaggerGenNewtonsoftSupport();

        builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;

            options.DefaultApiVersion = new ApiVersion(1,0);

            options.AssumeDefaultVersionWhenUnspecified = true;
            // options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("X-Version"),
                new MediaTypeApiVersionReader("ver"));
            options.ApiVersionSelector = new DefaultApiVersionSelector(options);
            // options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
        });
        
        builder.Services.AddEndpointsApiExplorer();

        

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errorResponse = new ApiError(context.ModelState);
                return new BadRequestObjectResult(errorResponse);
            };
        });
        #endregion
        
        

        builder.Services.AddAutoMapper(options => options.AddProfile<MapperProfile>());
        
        builder.Services.AddScoped<IAreaService, DefaultAreaService>();
        builder.Services.AddScoped<ICustomerService, DefaultCustomerService>();
        builder.Services.AddScoped<IReservationService, DefaultReservationService>();
        builder.Services.AddScoped<ISittingService, DefaultSittingService>();
        builder.Services.AddScoped<ITableService, DefaultTableService>();
        builder.Services.AddScoped<IVenueService, DefaultVenueService>();


        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();
        
        app.UseRouting();



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapSwagger();
            app.UseDeveloperExceptionPage();
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

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowMyApp");

        app.UseHttpsRedirection();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name : "areas",
                pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
        });


        app.MapRazorPages();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
            options.SwaggerEndpoint("swagger/v2/swagger.json", "v2");
            options.RoutePrefix = "";
            options.DefaultModelExpandDepth(-1);
            options.InjectStylesheet("../swagger-ui/SwaggerDark.css");

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
            if (key.EndsWith(suffix) || key.StartsWith(prefix))
                keys.Add(key);
        foreach (var key in keys) context.SchemaRepository.Schemas.Remove(key);
    }
}