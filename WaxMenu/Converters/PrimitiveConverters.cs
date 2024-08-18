using WaxMenu.Context;

namespace WaxMenu.Converters;

public static class PrimitiveConverters
{
    public static Task<object> BoolConverter(ConversionContext ctx) => Task.FromResult<object>(ctx.Content == "true");
    public static Task<object> ByteConverter(ConversionContext ctx) => Task.FromResult<object>(byte.Parse(ctx.Content));
    public static Task<object> SByteConverter(ConversionContext ctx) => Task.FromResult<object>(sbyte.Parse(ctx.Content));
    public static Task<object> CharConverter(ConversionContext ctx) => Task.FromResult<object>(ctx.Content[0]);
    public static Task<object> DecimalConverter(ConversionContext ctx) => Task.FromResult<object>(decimal.Parse(ctx.Content));
    public static Task<object> DoubleConverter(ConversionContext ctx) => Task.FromResult<object>(double.Parse(ctx.Content));
    public static Task<object> FloatConverter(ConversionContext ctx) => Task.FromResult<object>(float.Parse(ctx.Content));
    public static Task<object> IntConverter(ConversionContext ctx) => Task.FromResult<object>(int.Parse(ctx.Content));
    public static Task<object> UIntConverter(ConversionContext ctx) => Task.FromResult<object>(uint.Parse(ctx.Content));
    public static Task<object> LongConverter(ConversionContext ctx) => Task.FromResult<object>(long.Parse(ctx.Content));
    public static Task<object> ULongConverter(ConversionContext ctx) => Task.FromResult<object>(ulong.Parse(ctx.Content));
    public static Task<object> ShortConverter(ConversionContext ctx) => Task.FromResult<object>(short.Parse(ctx.Content));
    public static Task<object> UShortConverter(ConversionContext ctx) => Task.FromResult<object>(ushort.Parse(ctx.Content));
    public static Task<object> StringConverter(ConversionContext ctx) => Task.FromResult<object>(ctx.Content);
}