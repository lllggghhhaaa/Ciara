using DSharpPlus;
using DSharpPlus.Extensions;
using Microsoft.Extensions.DependencyInjection;
using WaxMenu.Events;

namespace WaxMenu;

public static class ServiceCollectionExtensionMethods
{
    public static IServiceCollection AddMenuExtension
    (
        this IServiceCollection services,
        Action<MenuExtension> menuCallback,
        MenuExtensionConfiguration configuration
    )
    {
        services.ConfigureEventHandlers(builder => builder.AddEventHandlers<ComponentInteractionCreated>());
        services.AddSingleton<MenuExtension>(provider =>
        {
            DiscordClient client = provider.GetRequiredService<DiscordClient>();
            var menu = new MenuExtension(configuration, client.ServiceProvider);
            
            menuCallback(menu);
            return menu;
        });
        
        return services;
    }
}