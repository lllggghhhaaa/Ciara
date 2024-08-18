using WaxMenu.Context;

namespace WaxMenu.Converters;

public static class TypeConverter
{
    public static Dictionary<Type, Func<ConversionContext, object>> Converters = new()
    {
        // Primitives
        { typeof(bool), ctx => PrimitiveConverters.BoolConverter(ctx) },
        { typeof(byte), ctx => PrimitiveConverters.ByteConverter(ctx) },
        { typeof(sbyte), ctx => PrimitiveConverters.SByteConverter(ctx) },
        { typeof(char), ctx => PrimitiveConverters.CharConverter(ctx) },
        { typeof(decimal), ctx => PrimitiveConverters.DecimalConverter(ctx) },
        { typeof(double), ctx => PrimitiveConverters.DoubleConverter(ctx) },
        { typeof(float), ctx => PrimitiveConverters.FloatConverter(ctx) },
        { typeof(int), ctx => PrimitiveConverters.IntConverter(ctx) },
        { typeof(uint), ctx => PrimitiveConverters.UIntConverter(ctx) },
        { typeof(long), ctx => PrimitiveConverters.LongConverter(ctx) },
        { typeof(ulong), ctx => PrimitiveConverters.ULongConverter(ctx) },
        { typeof(short), ctx => PrimitiveConverters.ShortConverter(ctx) },
        { typeof(ushort), ctx => PrimitiveConverters.UShortConverter(ctx) },
        { typeof(string), ctx => ctx.Content }
    };
}