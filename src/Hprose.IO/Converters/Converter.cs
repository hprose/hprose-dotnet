/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * Converter.cs                                           *
 *                                                        *
 * hprose Converter class for C#.                         *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Hprose.IO.Converters {
    public interface IConverter { }
    public class Converter<T> : IConverter {
        static Converter() => Converter.Initialize();
        private static volatile Converter<T> _instance;
        public static Converter<T> Instance {
            get {
                if (_instance == null) {
                    _instance = Converter.GetInstance(typeof(T)) as Converter<T>;
                }
                return _instance;
            }
        }
        private static TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
        public virtual T Convert(object obj) {
            return (T)typeConverter.ConvertFrom(obj);
        }
    }

    public class Converter : Converter<object> {
        public override object Convert(object obj) {
            return obj;
        }
        static readonly ConcurrentDictionary<Type, Lazy<IConverter>> _converters = new ConcurrentDictionary<Type, Lazy<IConverter>>();
        static Converter() {
            Register(() => new Converter());
            Register(() => new BaseConverter<bool>());
            Register(() => new BaseConverter<byte>());
            Register(() => new BaseConverter<sbyte>());
            Register(() => new BaseConverter<short>());
            Register(() => new BaseConverter<ushort>());
            Register(() => new BaseConverter<int>());
            Register(() => new BaseConverter<uint>());
            Register(() => new BaseConverter<ulong>());
            Register(() => new BaseConverter<float>());
            Register(() => new BaseConverter<double>());
            Register(() => new BaseConverter<decimal>());
            Register(() => new Int64Converter());
            Register(() => new BigIntegerConverter());
            Register(() => new TimeSpanConverter());
        }

        public static void Initialize() { }

        public static void Register<T>(Func<Converter<T>> ctor) {
            _converters[typeof(T)] = new Lazy<IConverter>(ctor);
        }

        private static Type GetConverterType(Type type) {
            return typeof(Converter<>).MakeGenericType(type);
        }

        internal static IConverter GetInstance(Type type) {
            return _converters.GetOrAdd(type, t => new Lazy<IConverter>(
                () => Activator.CreateInstance(GetConverterType(t)) as IConverter
            )).Value;
        }
    }
}
