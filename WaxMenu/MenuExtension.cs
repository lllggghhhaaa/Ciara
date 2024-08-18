using System.Reflection;
using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.EventArgs;
using WaxMenu.Attributes;
using WaxMenu.Builders;
using WaxMenu.Context;
using WaxMenu.Converters;

namespace WaxMenu;

public class MenuExtension(MenuExtensionConfiguration config, IServiceProvider _serviceProvider)
{
    private List<MenuBuilder> _menus = new();

    private Regex _menuRegex = new($"^{config.Prefix}{config.Separator}(?<command>[^{config.Separator}]+){config.Separator}(?<subcommand>[^{config.Separator}]+)(?:{config.Separator}(?<args>\\w+))*$");

    public void AddMenu<T>()
    {
        var type = typeof(T);
        
        var menuAttribute = type.GetCustomAttribute<MenuAttribute>();
        if (menuAttribute is null) throw new ArgumentException($"The {type.Name} doesn't contains the Menu Attribute");
        
        _menus.Add(new MenuBuilder(menuAttribute, _serviceProvider, type));
    }

    public MenuBuilder? GetMenu(string name) => _menus.Find(builder => builder.Name == name);

    public void HandleInteraction(DiscordClient sender, ComponentInteractionCreatedEventArgs eventArgs)
    {
        var match = _menuRegex.Match(eventArgs.Id);
        
        if (!match.Success) return;

        var command = match.Groups["command"].Value;
        var subcommand = match.Groups["subcommand"].Value;
        var args = match.Groups["args"].Value.Split(config.Separator, StringSplitOptions.RemoveEmptyEntries);
        
        var menuBuilder = GetMenu(command) ?? throw new NullReferenceException($"Cannot find the menu {command}");
        var subMenuBuilder = menuBuilder.GetSubMenu(subcommand) ?? throw new NullReferenceException($"cannot find the submenu {subcommand}");
        
        var parameters = new object[args.Length + 1];
        parameters[0] = new MenuContext(sender, eventArgs);

        for (var i = 0; i < args.Length; i++)
        {
            if (!TypeConverter.Converters.ContainsKey(subMenuBuilder.ArgsType[i]))
                throw new InvalidOperationException($"The type {subMenuBuilder.ArgsType[i].Name} is not implemented on menu parameter type converter");
            parameters[i + 1] = TypeConverter.Converters[subMenuBuilder.ArgsType[i]]
                .Invoke(new ConversionContext(sender, eventArgs, args[i]));
        }

        subMenuBuilder.Method.Invoke(menuBuilder.Instance, parameters);
    }
}