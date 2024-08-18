using System.Reflection;
using WaxMenu.Attributes;
using WaxMenu.Context;

namespace WaxMenu.Builders;

public class MenuActionBuilder
{
    public string Name;
    public MethodInfo Method;
    public Type[] ArgsType;

    public MenuActionBuilder(MethodInfo method, MenuActionAttribute menuActionAttribute)
    {
        Name = menuActionAttribute.Name;
        Method = method;
        
        var parameters = method.GetParameters();
        if (parameters.Length == 0 || !parameters[0].ParameterType.IsAssignableTo(typeof(MenuContext)))
            throw new ArgumentException(
                $"The first parameter of the method {method.DeclaringType?.FullName}.{method.Name} must be a type of {nameof(MenuContext)}.",
                nameof(method));

        ArgsType = new Type[parameters.Length - 1];

        for (int i = 1; i < parameters.Length; i++)
            ArgsType[i - 1] = parameters[i].ParameterType;
    }
}