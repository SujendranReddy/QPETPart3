using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Data.Entities;
using QPET.Services;
using QPET.Services.Interfaces;

var builder =
    WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews();

var connectionString =
    builder.Configuration
        .GetConnectionString(
            "DefaultConnection"
        );

if (
    builder.Environment
        .IsProduction())
{
    var productionConnectionString =
        Environment.GetEnvironmentVariable(
            "ConnectionStrings__DefaultConnection"
        );

    if (
        string.IsNullOrWhiteSpace(
            productionConnectionString
        ))
    {
        throw new InvalidOperationException(
            "A production database connection string must be provided through configuration."
        );
    }

    connectionString =
        productionConnectionString;
}

if (
    string.IsNullOrWhiteSpace(
        connectionString
    ))
{
    throw new InvalidOperationException(
        "The database connection string is missing."
    );
}

builder.Services
    .AddDbContext<QpetDbContext>(
        options =>
            options.UseSqlServer(
                connectionString
            )
    );

builder.Services.AddScoped<
    IProductService,
    ProductService
>();

builder.Services.AddScoped<
    IBranchService,
    BranchService
>();

builder.Services.AddScoped<
    IEnquiryService,
    EnquiryService
>();

builder.Services.AddScoped<
    IReviewService,
    ReviewService
>();

builder.Services.AddScoped<
    IEnquiryFileStorageService,
    EnquiryFileStorageService
>();

builder.Services.AddScoped<
    IPasswordHasher<AdminUser>,
    PasswordHasher<AdminUser>
>();

builder.Services.AddScoped<
    IAdminAuthService,
    AdminAuthService
>();

builder.Services.AddScoped<
    AdminAccountSeeder
>();

builder.Services.Configure<
    FormOptions>(
        options =>
        {
            options
                .MultipartBodyLengthLimit =
                    30L *
                    1024L *
                    1024L;
        }
    );

builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.Limits
                .MaxRequestBodySize =
                    30L *
                    1024L *
                    1024L;
        }
    );

builder.Services
    .AddAuthentication(
        "QPETAdmin"
    )
    .AddCookie(
        "QPETAdmin",
        options =>
        {
            options.LoginPath =
                "/Admin/Login";

            options.AccessDeniedPath =
                "/Admin/Login";

            options.Cookie.Name =
                "QPET.Admin";

            options.Cookie.HttpOnly =
                true;

            options.Cookie.SecurePolicy =
                CookieSecurePolicy.Always;

            options.Cookie.SameSite =
                SameSiteMode.Strict;

            options.Cookie.IsEssential =
                true;

            options.ExpireTimeSpan =
                TimeSpan.FromHours(8);

            options.SlidingExpiration =
                true;

            options.Events
                .OnRedirectToLogin =
                    context =>
                    {
                        if (
                            context.Request.Path
                                .StartsWithSegments(
                                    "/api"
                                ))
                        {
                            context.Response
                                .StatusCode =
                                    StatusCodes
                                        .Status401Unauthorized;

                            return Task
                                .CompletedTask;
                        }

                        context.Response
                            .Redirect(
                                context.RedirectUri
                            );

                        return Task
                            .CompletedTask;
                    };

            options.Events
                .OnRedirectToAccessDenied =
                    context =>
                    {
                        if (
                            context.Request.Path
                                .StartsWithSegments(
                                    "/api"
                                ))
                        {
                            context.Response
                                .StatusCode =
                                    StatusCodes
                                        .Status403Forbidden;

                            return Task
                                .CompletedTask;
                        }

                        context.Response
                            .Redirect(
                                context.RedirectUri
                            );

                        return Task
                            .CompletedTask;
                    };
        }
    );

builder.Services
    .AddAuthorization();

builder.Services
    .AddHsts(
        options =>
        {
            options.MaxAge =
                TimeSpan.FromDays(60);

            options.IncludeSubDomains =
                false;

            options.Preload =
                false;
        }
    );

var app =
    builder.Build();

using (
    var scope =
        app.Services
            .CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<
                QpetDbContext>();

    await DatabaseSeeder
        .SeedAsync(
            context
        );

    var adminSeeder =
        scope.ServiceProvider
            .GetRequiredService<
                AdminAccountSeeder>();

    await adminSeeder
        .SeedAsync();
}

if (
    app.Environment
        .IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(
        "/Home/Error"
    );

    app.UseHsts();
}

app.UseHttpsRedirection();

app.Use(
    async (
        context,
        next) =>
    {
        context.Response.Headers[
            "X-Content-Type-Options"
        ] = "nosniff";

        context.Response.Headers[
            "X-Frame-Options"
        ] = "DENY";

        context.Response.Headers[
            "Referrer-Policy"
        ] = "no-referrer";

        await next();
    }
);

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern:
        "{controller=Home}/{action=Index}/{id?}"
)
.WithStaticAssets();

app.Run();

public partial class Program
{
}