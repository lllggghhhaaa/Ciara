using DSharpPlus;
using DSharpPlus.Extensions;
using Microsoft.Extensions.DependencyInjection;
using WaxMenu.Events;

namespace WaxMenu;

public static class SetupExtensionMethods
{
    public static DiscordClientBuilder UseWaxMenu(this DiscordClientBuilder builder, Action<MenuExtension> menuCallback,
        MenuExtensionConfiguration configuration) =>
        builder.ConfigureServices(services => services.AddMenuExtension(menuCallback, configuration));

    public static IServiceCollection AddMenuExtension(this IServiceCollection services,
        Action<MenuExtension> menuCallback, MenuExtensionConfiguration configuration)
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