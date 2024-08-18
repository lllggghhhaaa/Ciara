using WaxMenu.Context;

namespace WaxMenu.Converters;

public static class PrimitiveConverters
{
    public static bool BoolConverter(ConversionContext ctx) => ctx.Content == "true";
    public static byte ByteConverter(ConversionContext ctx) => byte.Parse(ctx.Content);
    public static sbyte SByteConverter(ConversionContext ctx) => sbyte.Parse(ctx.Content);
    public static char CharConverter(ConversionContext ctx) => ctx.Content[0];
    public static decimal DecimalConverter(ConversionContext ctx) => decimal.Parse(ctx.Content);
    public static double DoubleConverter(ConversionContext ctx) => double.Parse(ctx.Content);
    public static float FloatConverter(ConversionContext ctx) => float.Parse(ctx.Content);
    public static int IntConverter(ConversionContext ctx) => int.Parse(ctx.Content);
    public static uint UIntConverter(ConversionContext ctx) => uint.Parse(ctx.Content);
    public static long LongConverter(ConversionContext ctx) => long.Parse(ctx.Content);
    public static ulong ULongConverter(ConversionContext ctx) => ulong.Parse(ctx.Content);
    public static short ShortConverter(ConversionContext ctx) => short.Parse(ctx.Content);
    public static ushort UShortConverter(ConversionContext ctx) => ushort.Parse(ctx.Content);
}