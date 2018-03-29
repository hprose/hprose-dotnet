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
 * HproseTags.cs                                          *
 *                                                        *
 * hprose tags class for C#.                              *
 *                                                        *
 * LastModified: Mar 28, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO {
    public static class HproseTags {
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
        public const byte TagFunctions = (byte)'F';
        public const byte TagCall = (byte)'C';
        public const byte TagResult = (byte)'R';
        public const byte TagArgument = (byte)'A';
        public const byte TagError = (byte)'E';
        public const byte TagEnd = (byte)'z';
    }
}