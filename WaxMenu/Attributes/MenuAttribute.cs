namespace WaxMenu.Attributes;

public class MenuAttribute(string name) : Attribute
{
    public readonly string Name = name;
}

public class MenuActionAttribute(string name) : Attribute
{
    public string Name = name;
}