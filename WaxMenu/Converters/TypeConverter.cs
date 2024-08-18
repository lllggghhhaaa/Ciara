using DSharpPlus.Entities;
using WaxMenu.Context;

namespace WaxMenu.Converters;

public static class TypeConverter
{
    public static Dictionary<Type, Func<ConversionContext, Task<object>>> Converters = new()
    {
        // Primitives
        { typeof(bool), PrimitiveConverters.BoolConverter },
        { typeof(byte), PrimitiveConverters.ByteConverter },
        { typeof(sbyte), PrimitiveConverters.SByteConverter },
        { typeof(char), PrimitiveConverters.CharConverter },
        { typeof(decimal), PrimitiveConverters.DecimalConverter },
        { typeof(double), PrimitiveConverters.DoubleConverter },
        { typeof(float), PrimitiveConverters.FloatConverter },
        { typeof(int), PrimitiveConverters.IntConverter },
        { typeof(uint), PrimitiveConverters.UIntConverter },
        { typeof(long), PrimitiveConverters.LongConverter },
        { typeof(ulong), PrimitiveConverters.ULongConverter },
        { typeof(short), PrimitiveConverters.ShortConverter },
        { typeof(ushort), PrimitiveConverters.UShortConverter },
        { typeof(string), PrimitiveConverters.StringConverter },
        
        // Discord
        { typeof(DiscordChannel), DiscordConverters.ChannelConverter },
        { typeof(DiscordUser), DiscordConverters.UserConverter },
        { typeof(DiscordGuild), DiscordConverters.GuildConverter }
    };
}