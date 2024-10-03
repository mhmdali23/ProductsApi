
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebAppApi.Authentication;
using WebAppApi.Authorization;
using WebAppApi.Data;
using WebAppApi.Filters;
using WebAppApi.Middlewares;
using WebAppApi.Options;

namespace WebAppApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //options pattern configuration
            //1 get
            //var attachmentOptions = builder.Configuration.GetSection("Attachments").Get<AttachmentOptions>();
            //builder.Services.AddSingleton(attachmentOptions);

            //2 bind
            //var attachmentOptions = new AttachmentOptions();
            //builder.Configuration.GetSection("Attachments").Bind(attachmentOptions);
            //builder.Services.AddSingleton(attachmentOptions);

            //3

            builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));
            

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<LogActivityFilter>();// global filter that will be called with any api request
                options.Filters.Add<PermissionBasedAuthorizationFilter>();
            });
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var conn = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(conn);
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var JwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
            builder.Services.AddSingleton(JwtOptions);

            builder.Services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JwtOptions.Issuer,
                        ValidateAudience = true, 
                        ValidAudience = JwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey))
                    };
                });

            builder.Services.AddSingleton<IAuthorizationHandler,AgeAuthorizationHanlder>();
            builder.Services.AddAuthorization(options =>
            {
                //options.AddPolicy("SuperUsersOnly", opt =>
                //{
                //    opt.RequireRole("Admin", "SuperUser");
                //});   
                options.AddPolicy("AgeGreaterThan25", builder => builder.AddRequirements(new AgeGraterThan25Requirment()));
        
            });

            //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseMiddleware<ProfileMiddleware>();


            app.MapControllers();

            app.Run();
        }
    }
}
