/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Tags.cs                                                 |
|                                                          |
|  hprose tags class for C#.                               |
|                                                          |
|  LastModified: Jul 2, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO {
    public static class Tags {
        /* Serialize Tags */
        public const byte TagInteger = (byte)'i';
        public const byte TagLong = (byte)'l';
        public const byte TagDouble = (byte)'d';
        public const byte TagNull = (byte)'n';
        public const byte TagEmpty = (byte)'e';
        public const byte TagTrue = (byte)'t';
        public const byte TagFalse = (byte)'f';
        public const byte TagNaN = (byte)'N';
        public const byte TagInfinity = (byte)'I';
        public const byte TagDate = (byte)'D';
        public const byte TagTime = (byte)'T';
        public const byte TagUTC = (byte)'Z';
        public const byte TagBytes = (byte)'b';
        public const byte TagUTF8Char = (byte)'u';
        public const byte TagString = (byte)'s';
        public const byte TagGuid = (byte)'g';
        public const byte TagList = (byte)'a';
        public const byte TagMap = (byte)'m';
        public const byte TagClass = (byte)'c';
        public const byte TagObject = (byte)'o';
        public const byte TagRef = (byte)'r';
        /* Serialize Marks */
        public const byte TagPos = (byte)'+';
        public const byte TagNeg = (byte)'-';
        public const byte TagSemicolon = (byte)';';
        public const byte TagOpenbrace = (byte)'{';
        public const byte TagClosebrace = (byte)'}';
        public const byte TagQuote = (byte)'"';
        public const byte TagPoint = (byte)'.';
        /* Protocol Tags */
        public const byte TagHeader = (byte)'H';
        public const byte TagCall = (byte)'C';
        public const byte TagResult = (byte)'R';
        public const byte TagError = (byte)'E';
        public const byte TagEnd = (byte)'z';

        public static string ToString(int tag) => tag switch {
            '0' => "Int32",
            '1' => "Int32",
            '2' => "Int32",
            '3' => "Int32",
            '4' => "Int32",
            '5' => "Int32",
            '6' => "Int32",
            '7' => "Int32",
            '8' => "Int32",
            '9' => "Int32",
            TagInteger => "Int32",
            TagLong => "BigInteger",
            TagDouble => "Double",
            TagNull => "Null",
            TagEmpty => "Empty String",
            TagTrue => "Boolean True",
            TagFalse => "Boolean False",
            TagNaN => "NaN",
            TagInfinity => "Infinity",
            TagDate => "DateTime",
            TagTime => "DateTime",
            TagBytes => "Byte[]",
            TagUTF8Char => "Char",
            TagString => "String",
            TagGuid => "Guid",
            TagList => "IList",
            TagMap => "IDictionary",
            TagClass => "Class",
            TagObject => "Object",
            TagRef => "Reference",
            TagError => "Error",
            _ => "Unexpected Tag: 0x" + (tag & 0xff).ToString("x2"),
        };
    }
}