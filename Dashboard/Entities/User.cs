using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using Dashboard.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Entities;

public record struct User(
    ulong Id,
    string Username,
    string DisplayName,
    string AvatarUrl,
    string BannerColor,
    string? Banner,
    DiscordFlags Flags)
{
    public static async Task<User?> FromAuthStateProvider(AuthenticationStateProvider auth)
    {
        var authState = await auth.GetAuthenticationStateAsync();
        var userClaims = authState.User;

        if (userClaims.Identity is null || !userClaims.Identity.IsAuthenticated) return null;

        string id = userClaims.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        string avatarHash = userClaims.Claims.First(claim => claim.Type == DiscordAuthenticationConstants.Claims.AvatarHash).Value;
        
        foreach (var claim in userClaims.Claims)
        {
            Console.WriteLine($"{claim.Type} : {claim.Value}");
        }

        string username = userClaims.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        string displayname = userClaims.Claims.First(claim => claim.Type == DiscordConstants.DisplayNameClaim).Value;
        string avatarUrl = $"https://cdn.discordapp.com/avatars/{id}/{avatarHash}.webp";
        int accentColor = int.Parse(userClaims.Claims.FirstOrDefault(claim => claim.Type == DiscordConstants.BannerColor)?.Value ?? "0");
        string bannerColor = Convert.ToString(accentColor, 16);

        string? bannerHash = userClaims.Claims.FirstOrDefault(claim => claim.Type == DiscordConstants.Banner)?.Value;
        string? banner = bannerHash is null ? null : $"https://cdn.discordapp.com/banners/{id}/{bannerHash}.webp";

        var flags = (DiscordFlags)int.Parse(userClaims.Claims.FirstOrDefault(claim => claim.Type == DiscordConstants.Flags)?.Value ?? "0");

        return new User(ulong.Parse(id) ,username, displayname, avatarUrl, bannerColor, banner, flags);
    }
}