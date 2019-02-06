/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Converter.cs                                            |
|                                                          |
|  hprose Converter class for C#.                          |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO.Converters;
using System;
using System.Text;

namespace Hprose.IO {
    class Converter<TInput, TOutput> {
        public static volatile Func<TInput, TOutput> convert;
    }

    public static class Converter<TOutput> {
        internal static System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(TOutput));

        static Converter() {
            if (Converter<object, TOutput>.convert == null) {
                Converter<object, TOutput>.convert = ConvertFrom;
            }
            if (Converter<char[], TOutput>.convert == null) {
                Converter<char[], TOutput>.convert = ConvertFromChars;
            }
            if (Converter<StringBuilder, TOutput>.convert == null) {
                Converter<StringBuilder, TOutput>.convert = ConvertFromStringBuilder;
            }
            Converter.Initialize();
        }

        public static TOutput Convert<TInput>(TInput value) {
            switch (value) { case TOutput result: return result; }
            var convert = Converter<TInput, TOutput>.convert;
            if (convert != null) {
                return convert(value);
            }
            Type type = typeof(TOutput);
            if (type == typeof(string)) {
                return (TOutput)(object)StringConverter.Convert(value);
            }
            if (type == typeof(char[])) {
                return (TOutput)(object)CharsConverter.Convert(value);
            }
            if (type == typeof(StringBuilder)) {
                return (TOutput)(object)StringBuilderConverter.Convert(value);
            }
            return ConvertFromObject(value);
        }

        internal static TOutput ConvertFromChars(char[] value) => (TOutput)converter.ConvertFrom(new string(value));

        internal static TOutput ConvertFromStringBuilder(StringBuilder value) => (TOutput)converter.ConvertFrom(value.ToString());

        internal static TOutput ConvertFromObject(object value) => (TOutput)converter.ConvertFrom(value);

        internal static TOutput ConvertFrom(object value) {
            switch (value) {
                case TOutput obj:
                    return obj;
                case char[] chars:
                    return (TOutput)converter.ConvertFrom(new string(chars));
                case StringBuilder sb:
                    return (TOutput)converter.ConvertFrom(sb.ToString());
                default:
                    return (TOutput)converter.ConvertFrom(value);
            }
        }
    }

    public static class Converter {
        static Converter() {
            BoolConverter.Initialize();
            CharConverter.Initialize();
            ByteConverter.Initialize();
            SByteConverter.Initialize();
            Int16Converter.Initialize();
            UInt16Converter.Initialize();
            Int32Converter.Initialize();
            UInt32Converter.Initialize();
            Int64Converter.Initialize();
            UInt64Converter.Initialize();
            SingleConverter.Initialize();
            DoubleConverter.Initialize();
            DecimalConverter.Initialize();
            BigIntegerConverter.Initialize();
            BytesConverter.Initialize();
            CharsConverter.Initialize();
            StringConverter.Initialize();
            StringBuilderConverter.Initialize();
            DateTimeConverter.Initialize();
            TimeSpanConverter.Initialize();
            GuidConverter.Initialize();
            MemoryStreamConverter.Initialize();
            StreamConverter.Initialize();
        }
        internal static void Initialize() { }
        public static void Register<TInput, TOutput>(Func<TInput, TOutput> convert) => Converter<TInput, TOutput>.convert = convert;
    }
}
