using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Services;

var builder =
    WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QpetDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection"
            )
        )
);

builder.Services.AddScoped<
    PrototypeDataService
>();

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
                SameSiteMode.Lax;

            options.ExpireTimeSpan =
                TimeSpan.FromHours(8);

            options.SlidingExpiration =
                true;
        }
    );

builder.Services.AddAuthorization();

var app =
    builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Home/Error"
    );

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern:
        "{controller=Home}/{action=Index}/{id?}"
)
.WithStaticAssets();

app.Run();