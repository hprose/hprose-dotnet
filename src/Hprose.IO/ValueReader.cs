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
 * ValueReader.cs                                         *
 *                                                        *
 * ValueReader class for C#.                              *
 *                                                        *
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;
using System.Numerics;
using System.Text;
using static System.DateTimeKind;

namespace Hprose.IO {
    using static Tags;

    public static class ValueReader {
        public static int ReadInt(Stream stream, int tag) {
            int result = 0;
            int i = stream.ReadByte();
            if (i == tag) {
                return result;
            }
            bool neg = false;
            switch (i) {
                case '-':
                    neg = true;
                    goto case '+';
                case '+':
                    i = stream.ReadByte();
                    break;
            }
            if (neg) {
                while ((i != tag) && (i != -1)) {
                    result = result * 10 - (i - '0');
                    i = stream.ReadByte();
                }
            }
            else {
                while ((i != tag) && (i != -1)) {
                    result = result * 10 + (i - '0');
                    i = stream.ReadByte();
                }
            }
            return result;
        }

        public static long ReadLong(Stream stream, int tag) {
            long result = 0;
            int i = stream.ReadByte();
            if (i == tag) {
                return result;
            }
            bool neg = false;
            switch (i) {
                case '-':
                    neg = true;
                    goto case '+';
                case '+':
                    i = stream.ReadByte();
                    break;
            }
            if (neg) {
                while ((i != tag) && (i != -1)) {
                    result = result * 10 - (i - '0');
                    i = stream.ReadByte();
                }
            }
            else {
                while ((i != tag) && (i != -1)) {
                    result = result * 10 + (i - '0');
                    i = stream.ReadByte();
                }
            }
            return result;
        }

        public static float ReadIntAsSingle(Stream stream) {
            float result = 0;
            int i = stream.ReadByte();
            if (i == TagSemicolon) {
                return result;
            }
            bool neg = false;
            switch (i) {
                case '-':
                    neg = true;
                    goto case '+';
                case '+':
                    i = stream.ReadByte();
                    break;
            }
            if (neg) {
                while ((i != TagSemicolon) && (i != -1)) {
                    result = result * 10 - (i - '0');
                    i = stream.ReadByte();
                }
            }
            else {
                while ((i != TagSemicolon) && (i != -1)) {
                    result = result * 10 + (i - '0');
                    i = stream.ReadByte();
                }
            }
            return result;
        }

        public static double ReadIntAsDouble(Stream stream) {
            double result = 0;
            int i = stream.ReadByte();
            if (i == TagSemicolon) {
                return result;
            }
            bool neg = false;
            switch (i) {
                case '-':
                    neg = true;
                    goto case '+';
                case '+':
                    i = stream.ReadByte();
                    break;
            }
            if (neg) {
                while ((i != TagSemicolon) && (i != -1)) {
                    result = result * 10 - (i - '0');
                    i = stream.ReadByte();
                }
            }
            else {
                while ((i != TagSemicolon) && (i != -1)) {
                    result = result * 10 + (i - '0');
                    i = stream.ReadByte();
                }
            }
            return result;
        }

        public static decimal ReadIntAsDecimal(Stream stream) {
            decimal result = 0;
            int i = stream.ReadByte();
            if (i == TagSemicolon) {
                return result;
            }
            bool neg = false;
            switch (i) {
                case '-':
                    neg = true;
                    goto case '+';
                case '+':
                    i = stream.ReadByte();
                    break;
            }
            if (neg) {
                while ((i != TagSemicolon) && (i != -1)) {
                    result = result * 10 - (i - '0');
                    i = stream.ReadByte();
                }
            }
            else {
                while ((i != TagSemicolon) && (i != -1)) {
                    result = result * 10 + (i - '0');
                    i = stream.ReadByte();
                }
            }
            return result;
        }

        public static int ReadLength(Stream stream) {
            return ReadInt(stream, TagQuote);
        }

        public static int ReadCount(Stream stream) {
            return ReadInt(stream, TagOpenbrace);
        }

        public static StringBuilder ReadUntil(Stream stream, int tag) {
            StringBuilder sb = new StringBuilder();
            int i = stream.ReadByte();
            while ((i != tag) && (i != -1)) {
                sb.Append((char)i);
                i = stream.ReadByte();
            }
            return sb;
        }
        public static void SkipUntil(Stream stream, int tag) {
            int i = stream.ReadByte();
            while ((i != tag) && (i != -1)) {
                i = stream.ReadByte();
            }
        }
        public static char[] ReadChars(Stream stream) {
            int len = ReadLength(stream);
            char[] buf = new char[len];
            int b1, b2, b3, b4;
            for (int i = 0; i < len; ++i) {
                b1 = stream.ReadByte();
                switch (b1 >> 4) {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        // 0xxx xxxx
                        buf[i] = (char)b1;
                        break;
                    case 12:
                    case 13:
                        // 110x xxxx   10xx xxxx
                        b2 = stream.ReadByte();
                        buf[i] = (char)(((b1 & 0x1f) << 6) |
                                         (b2 & 0x3f));
                        break;
                    case 14:
                        b2 = stream.ReadByte();
                        b3 = stream.ReadByte();
                        buf[i] = (char)(((b1 & 0x0f) << 12) |
                                        ((b2 & 0x3f) << 6) |
                                         (b3 & 0x3f));
                        break;
                    case 15:
                        // 1111 0xxx  10xx xxxx  10xx xxxx  10xx xxxx
                        if ((b1 & 0xf) <= 4) {
                            b2 = stream.ReadByte();
                            b3 = stream.ReadByte();
                            b4 = stream.ReadByte();
                            int s = (((b1 & 0x07) << 18) |
                                     ((b2 & 0x3f) << 12) |
                                     ((b3 & 0x3f) << 6) |
                                      (b4 & 0x3f)) - 0x10000;
                            if (0 <= s && s <= 0xfffff) {
                                buf[i] = (char)(((s >> 10) & 0x03ff) | 0xd800);
                                buf[++i] = (char)((s & 0x03ff) | 0xdc00);
                                break;
                            }
                        }
                        goto default;
                    default:
                        throw BadEncoding(b1);
                }
            }
            stream.ReadByte();
            return buf;
        }
        public static char ReadChar(Stream stream) {
            char u;
            int b1 = stream.ReadByte(), b2, b3;
            switch (b1 >> 4) {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    u = (char)b1;
                    break;
                case 12:
                case 13:
                    b2 = stream.ReadByte();
                    u = (char)(((b1 & 0x1f) << 6) |
                                 (b2 & 0x3f));
                    break;
                case 14:
                    b2 = stream.ReadByte();
                    b3 = stream.ReadByte();
                    u = (char)(((b1 & 0x0f) << 12) |
                                ((b2 & 0x3f) << 6) |
                                 (b3 & 0x3f));
                    break;
                default:
                    throw BadEncoding(b1);
            }
            return u;
        }

        public static int ReadInt(Stream stream) {
            return ReadInt(stream, TagSemicolon);
        }

        public static long ReadLong(Stream stream) {
            return ReadLong(stream, TagSemicolon);
        }

        public static BigInteger ReadBigInteger(Stream stream) {
            return BigInteger.Parse(ReadUntil(stream, TagSemicolon).ToString());
        }

        public static float ReadSingle(Stream stream) {
            return float.Parse(ReadUntil(stream, TagSemicolon).ToString());
        }

        public static double ReadDouble(Stream stream) {
            return double.Parse(ReadUntil(stream, TagSemicolon).ToString());
        }

        public static decimal ReadDecimal(Stream stream) {
            return decimal.Parse(ReadUntil(stream, TagSemicolon).ToString());
        }

        public static float ReadSingleInfinity(Stream stream) {
            return (stream.ReadByte() == TagNeg) ? float.NegativeInfinity : float.PositiveInfinity;
        }

        public static double ReadInfinity(Stream stream) {
            return (stream.ReadByte() == TagNeg) ? double.NegativeInfinity : double.PositiveInfinity;
        }

        public static string ReadString(Stream stream) {
            return new string(ReadChars(stream));
        }

        public static string ReadUTF8Char(Stream stream) {
            return new string(ReadChar(stream), 1);
        }

        public static byte[] ReadBytes(Stream stream) {
            int len = ReadLength(stream);
            int off = 0;
            byte[] b = new byte[len];
            while (len > 0) {
                int size = stream.Read(b, off, len);
                off += size;
                len -= size;
            }
            stream.ReadByte();
            return b;
        }

        public static Guid ReadGuid(Stream stream) {
            char[] buf = new char[38];
            for (int i = 0; i < 38; ++i) {
                buf[i] = (char)stream.ReadByte();
            }
            return new Guid(new String(buf));
        }

        public static DateTime ReadTime(Stream stream) {
            int hour = Read2Digit(stream);
            int minute = Read2Digit(stream);
            int second = Read2Digit(stream);
            int millisecond = 0;
            int ticks = 0;
            int tag = stream.ReadByte();
            if (tag == TagPoint) {
                tag = ReadMillisecond(stream, out millisecond, out ticks);
            }
            return new DateTime(1970, 1, 1, hour, minute, second, millisecond, (tag == TagUTC ? Utc : Local)).AddTicks(ticks);
        }

        public static DateTime ReadDateTime(Stream stream) {
            int year = Read4Digit(stream);
            int month = Read2Digit(stream);
            int day = Read2Digit(stream);
            int tag = stream.ReadByte();
            int hour = 0;
            int minute = 0;
            int second = 0;
            int millisecond = 0;
            int ticks = 0;
            if (tag == TagTime) {
                hour = Read2Digit(stream);
                minute = Read2Digit(stream);
                second = Read2Digit(stream);
                tag = stream.ReadByte();
                if (tag == TagPoint) {
                    tag = ReadMillisecond(stream, out millisecond, out ticks);
                }
            }
            return new DateTime(year, month, day, hour, minute, second, millisecond, (tag == TagUTC ? Utc : Local)).AddTicks(ticks);
        }

        private static int Read4Digit(Stream stream) {
            int n = stream.ReadByte() - '0';
            n = n * 10 + stream.ReadByte() - '0';
            n = n * 10 + stream.ReadByte() - '0';
            return n * 10 + stream.ReadByte() - '0';
        }

        private static int Read2Digit(Stream stream) {
            int n = stream.ReadByte() - '0';
            return n * 10 + stream.ReadByte() - '0';
        }

        private static int ReadMillisecond(Stream stream, out int millisecond, out int ticks) {
            millisecond = stream.ReadByte() - '0';
            millisecond = millisecond * 10 + (stream.ReadByte() - '0');
            millisecond = millisecond * 10 + (stream.ReadByte() - '0');
            ticks = 0;
            int tag = stream.ReadByte();
            if (tag >= '0' && tag <= '9') {
                ticks += (tag - '0') * 1000;
                ticks += (stream.ReadByte() - '0') * 100;
                ticks += (stream.ReadByte() - '0') * 10;
                tag = stream.ReadByte();
                if (tag >= '0' && tag <= '9') {
                    ticks += (tag - '0');
                    stream.ReadByte();
                    stream.ReadByte();
                    tag = stream.ReadByte();
                }
            }
            return tag;
        }

        internal static IOException BadEncoding(int b) {
            if (b == -1) return new EndOfStreamException();
            return new IOException("Bad UTF-8 encoding at 0x" + (b & 0xff).ToString("x2"));
        }
    }
}
