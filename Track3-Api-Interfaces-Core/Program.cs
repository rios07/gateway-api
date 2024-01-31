using Application.interfaces;
using Infraestruture.DAO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Infraestructure.Cache;
using Application;
using Track3_Api_Interfaces_Core.Application.Interfaces;
using Track3_Api_Interfaces_Core.Application.ApplicationMain;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Track3_Api_Interfaces_Core.Handlers;
using Ocelot.Values;
using Track3_Api_Interfaces_Core.Infraestructure.Interfaces;
using Track3_Api_Interfaces_Core.Infraestructure;
using Track3_Api_Interfaces_Core.Application.Services;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();


Infraestructure.Infraestructura._configuration = provider.GetRequiredService<IConfiguration>();
// Add services to the container.
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Clave.ClaveSecreta))
                    };
                    option.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            Microsoft.Extensions.Primitives.StringValues accessToken = context.Request.Query["access_token"];

                            PathString path = context.HttpContext.Request.Path;

                            return System.Threading.Tasks.Task.CompletedTask;
                        }
                    };
                });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0.0",
        Title = "Track3 api Interfaces Core"
    });
    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer TOKEN",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                        {
                                            {securityScheme, new string[] { }}
                                        });
});

builder.Services.AddCors(o => o.AddPolicy("CorsAll", x => x.AllowAnyHeader()
                                                           .AllowAnyMethod()
                                                           .AllowCredentials()
                                                           .SetIsOriginAllowed(isOriginAllowed: _ => true)
                                                           .AllowCredentials()
                                          ));
#region Patrones
builder.Services.AddSingleton<ICacheServices, CacheServices>();
builder.Services.AddScoped<IOracleDao, OracleDao>();
builder.Services.AddScoped<IProcesosRepository, ProcesosRepository>();
builder.Services.AddScoped<IRedirectionsManager, RedirectionsManager>();
builder.Services.AddScoped<IRedirectionApp, RedirectionApp>();
//builder.Services.AddScoped<ISqlDao, SqlDao>();
builder.Services.AddScoped<IServiceHelper, ServiceHelper>();
//builder.Services.AddScoped<IMappingFormularios, ProfileForm>();

#endregion 

builder.Services.AddHttpClient();
builder.Services.AddMvc().ConfigureApiBehaviorOptions(options => {
    // options.InvalidModelStateResponseFactory = ActionContext => { return CustomErrorResponse(ActionContext); };
})
                         .AddNewtonsoftJson(opt => {
                             opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                             opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                         });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOcelot()
    .AddDelegatingHandler<LogHandler>(true);

builder.Configuration.AddJsonFile("configuration.json");
builder.Services.AddHttpContextAccessor();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsAll");

app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

//app.MapControllers();

app.UseOcelot().Wait();

app.Run();
public static class Clave
{
    public static string ClaveSecreta { get; } = "clave-secreta-api";
}
