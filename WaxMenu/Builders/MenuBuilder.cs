using System.Reflection;
using WaxMenu.Attributes;

namespace WaxMenu.Builders;

public class MenuBuilder
{
    public string Name;
    private readonly List<MenuActionBuilder> _subMenus = new();
    public object Instance;
    
    public MenuBuilder(MenuAttribute menuAttribute, IServiceProvider serviceProvider, Type type)
    {
        Name = menuAttribute.Name;
        Instance = CreateInstance(type, serviceProvider);
        
        foreach (var methodInfo in type.GetMethods())
        {
            var submenuAttribute = methodInfo.GetCustomAttribute<MenuActionAttribute>();
            
            if (submenuAttribute is null) continue;
            
            _subMenus.Add(new MenuActionBuilder(methodInfo, submenuAttribute));
        }
    }

    public MenuActionBuilder? GetSubMenu(string name) => _subMenus.Find(builder => builder.Name == name);

    private static object CreateInstance(Type type, IServiceProvider serviceProvider)
    {
        var constructor = type.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .FirstOrDefault();

        if (constructor is null) throw new InvalidOperationException($"Cannot find a public constructor for {type.FullName}");

        var parameters = constructor.GetParameters()
            .Select(param => serviceProvider.GetService(param.ParameterType))
            .ToArray();

        return constructor.Invoke(parameters);
    }
}