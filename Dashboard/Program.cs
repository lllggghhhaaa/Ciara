using System.Security.Claims;
using Ciara.Shared.Database;
using Dashboard.Components;
using Dashboard.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MessageHistoryContext>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
    })
    .AddDiscord(options =>
    {
        options.ClientId = builder.Configuration["Discord:ClientId"] ?? throw new InvalidOperationException();
        options.ClientSecret = builder.Configuration["Discord:ClientSecret"] ?? throw new InvalidOperationException();
        options.CallbackPath = "/callback/login/discord";
        options.Scope.Add("guilds");
        options.ClaimActions.MapJsonKey(DiscordConstants.DisplayNameClaim, "global_name");
        options.ClaimActions.MapJsonKey(DiscordConstants.BannerColor, "accent_color", ClaimValueTypes.Integer32);
        options.ClaimActions.MapJsonKey(DiscordConstants.Flags, "flags");
        options.ClaimActions.MapJsonKey(DiscordConstants.Banner, "banner");
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();