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
 * TypeEnum.cs                                            *
 *                                                        *
 * type enum for C#.                                      *
 *                                                        *
 * LastModified: Sep 3, 2016                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
namespace Hprose.IO {
    using System;
    internal enum TypeEnum {
        Null,
        Object,
#if !Core
        DBNull,
#endif
        Boolean,
        Char,
        SByte,
        Byte,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        IntPtr,
        Single,
        Double,
        Decimal,
        DateTime,
        String,
        StringBuilder,
        Guid,
        BigInteger,
        TimeSpan,

        MemoryStream,
        Stream,

        ObjectArray,
        BooleanArray,
        CharArray,
        SByteArray,
        ByteArray,
        Int16Array,
        UInt16Array,
        Int32Array,
        UInt32Array,
        Int64Array,
        UInt64Array,
        IntPtrArray,
        SingleArray,
        DoubleArray,
        DecimalArray,
        DateTimeArray,
        StringArray,
        StringBuilderArray,
        GuidArray,
        BigIntegerArray,
        TimeSpanArray,
        CharsArray,
        BytesArray,

        ICollection,
        IDictionary,
        IList,
        Dictionary,
        List,
#if !dotNETMF
        BitArray,
#endif
        OtherType,
        OtherTypeArray,
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
        ArrayList,
        HashMap,
        Hashtable,
        Queue,
        Stack,
#endif
#if !(dotNET10 || dotNET11 || dotNETCF10 || dotNETMF)

        NullableBoolean,
        NullableChar,
        NullableSByte,
        NullableByte,
        NullableInt16,
        NullableUInt16,
        NullableInt32,
        NullableUInt32,
        NullableInt64,
        NullableUInt64,
        NullableIntPtr,
        NullableSingle,
        NullableDouble,
        NullableDecimal,
        NullableDateTime,
        NullableGuid,
        NullableBigInteger,
        NullableTimeSpan, 
        
        GenericList,
        GenericDictionary,
        GenericQueue,
        GenericStack,
        GenericHashMap,
        GenericICollection,
        GenericIDictionary,
        GenericIList,

        ObjectList,
        BooleanList,
        CharList,
        SByteList,
        ByteList,
        Int16List,
        UInt16List,
        Int32List,
        UInt32List,
        Int64List,
        UInt64List,
        IntPtrList,
        SingleList,
        DoubleList,
        DecimalList,
        DateTimeList,
        StringList,
        StringBuilderList,
        GuidList,
        BigIntegerList,
        TimeSpanList,
        CharsList,
        BytesList,

        ObjectIList,
        BooleanIList,
        CharIList,
        SByteIList,
        ByteIList,
        Int16IList,
        UInt16IList,
        Int32IList,
        UInt32IList,
        Int64IList,
        UInt64IList,
        IntPtrIList,
        SingleIList,
        DoubleIList,
        DecimalIList,
        DateTimeIList,
        StringIList,
        StringBuilderIList,
        GuidIList,
        BigIntegerIList,
        TimeSpanIList,
        CharsIList,
        BytesIList,

        StringObjectHashMap,
        ObjectObjectHashMap,
        IntObjectHashMap,
        StringObjectDictionary,
        ObjectObjectDictionary,
        IntObjectDictionary,
#endif
        Enum,
        UnSupportedType,
    }
}