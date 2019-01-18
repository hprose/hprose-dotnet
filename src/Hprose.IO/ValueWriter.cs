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
 * ValueWriter.cs                                         *
 *                                                        *
 * ValueWriter class for C#.                              *
 *                                                        *
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace Hprose.IO {
    using static Tags;

    public static class ValueWriter {
        private const int INT_SIZE = 11;
        private const int LONG_SIZE = 20;
        private static readonly byte[] digits = GetASCII("0123456789");
        private static readonly byte[] digitTens = GetASCII(
            "0000000000" +
            "1111111111" +
            "2222222222" +
            "3333333333" +
            "4444444444" +
            "5555555555" +
            "6666666666" +
            "7777777777" +
            "8888888888" +
            "9999999999");
        private static readonly byte[] digitOnes = GetASCII(
            "0123456789" +
            "0123456789" +
            "0123456789" +
            "0123456789" +
            "0123456789" +
            "0123456789" +
            "0123456789" +
            "0123456789" +
            "0123456789" +
            "0123456789");
        private static readonly byte[] minIntBuf = GetASCII("-2147483648");
        private static readonly byte[] minLongBuf = GetASCII("-9223372036854775808");
        public static readonly UTF8Encoding UTF8 = new UTF8Encoding();

#if NETCOREAPP2_1 || NETCOREAPP2_2
        public static void WriteASCII(Stream stream, string s) {
            int size = s.Length;
            Span<byte> buf = stackalloc byte[size];
            while (--size >= 0) {
                buf[size] = (byte)s[size];
            }
            stream.Write(buf);
        }
#else
        public static void WriteASCII(Stream stream, string s) {
            byte[] buf = GetASCII(s);
            stream.Write(buf, 0, buf.Length);
        }
#endif

        public static byte[] GetASCII(string s) {
            int size = s.Length;
            byte[] buf = new byte[size];
            while (--size >= 0) {
                buf[size] = (byte)s[size];
            }
            return buf;
        }

        public static void WriteInt(Stream stream, int i) {
            if ((i >= 0) && (i <= 9)) {
                stream.WriteByte(digits[i]);
            }
            else if (i == int.MinValue) {
                stream.Write(minIntBuf, 0, minIntBuf.Length);
            }
            else {
#if NETCOREAPP2_1 || NETCOREAPP2_2
                Span<byte> buf = stackalloc byte[INT_SIZE];
#else
                byte[] buf = new byte[INT_SIZE];
#endif
                int off = ToBytes((uint)((i < 0) ? -i : i), buf, INT_SIZE);
                if (i < 0) {
                    buf[--off] = (byte)'-';
                }
#if NETCOREAPP2_1 || NETCOREAPP2_2
                stream.Write(buf.Slice(off, INT_SIZE - off));
#else
                stream.Write(buf, off, INT_SIZE - off);
#endif
            }
        }

        public static void WriteInt(Stream stream, uint i) {
            if ((i >= 0) && (i <= 9)) {
                stream.WriteByte(digits[i]);
            }
            else {
#if NETCOREAPP2_1 || NETCOREAPP2_2
                Span<byte> buf = stackalloc byte[INT_SIZE];
                int off = ToBytes(i, buf, INT_SIZE);
                stream.Write(buf.Slice(off, INT_SIZE - off));
#else
                byte[] buf = new byte[INT_SIZE];
                int off = ToBytes(i, buf, INT_SIZE);
                stream.Write(buf, off, INT_SIZE - off);
#endif
            }
        }

        public static void WriteInt(Stream stream, long i) {
            if ((i >= 0) && (i <= 9)) {
                stream.WriteByte(digits[i]);
            }
            else if (i == long.MinValue) {
                stream.Write(minLongBuf, 0, minLongBuf.Length);
            }
            else {
#if NETCOREAPP2_1 || NETCOREAPP2_2
                Span<byte> buf = stackalloc byte[LONG_SIZE];
#else
                byte[] buf = new byte[LONG_SIZE];
#endif
                int off = ToBytes((ulong)((i < 0) ? -i : i), buf, LONG_SIZE);
                if (i < 0) {
                    buf[--off] = (byte)'-';
                }
#if NETCOREAPP2_1 || NETCOREAPP2_2
                stream.Write(buf.Slice(off, LONG_SIZE - off));
#else
                stream.Write(buf, off, LONG_SIZE - off);
#endif
            }
        }

        public static void WriteInt(Stream stream, ulong i) {
            if ((i >= 0) && (i <= 9)) {
                stream.WriteByte(digits[i]);
            }
            else {
#if NETCOREAPP2_1 || NETCOREAPP2_2
                Span<byte> buf = stackalloc byte[LONG_SIZE];
                int off = ToBytes(i, buf, LONG_SIZE);
                stream.Write(buf.Slice(off, LONG_SIZE - off));
#else
                byte[] buf = new byte[LONG_SIZE];
                int off = ToBytes(i, buf, LONG_SIZE);
                stream.Write(buf, off, LONG_SIZE - off);
#endif
            }
        }

#if NETCOREAPP2_1 || NETCOREAPP2_2
        private static int ToBytes(uint i, Span<byte> buf, int off) {
#else
        private static int ToBytes(uint i, byte[] buf, int off) {
#endif
            uint q, r;
            while (i >= 65536) {
                q = i / 100;
                r = i - (q * 100);
                i = q;
                buf[--off] = digitOnes[r];
                buf[--off] = digitTens[r];
            }
            for (; ; ) {
                q = (i * 52429) >> (16 + 3);
                r = i - (q * 10);
                buf[--off] = digits[r];
                i = q;
                if (i == 0) break;
            }
            return off;
        }

#if NETCOREAPP2_1 || NETCOREAPP2_2
        private static int ToBytes(ulong i, Span<byte> buf, int off) {
#else
        private static int ToBytes(ulong i, byte[] buf, int off) {
#endif
            ulong q, r;
            while (i > int.MaxValue) {
                q = i / 100;
                r = i - (q * 100);
                i = q;
                buf[--off] = digitOnes[r];
                buf[--off] = digitTens[r];
            }
            return ToBytes((uint)i, buf, off);
        }

        public static void Write(Stream stream, int i) {
            if (i >= 0 && i <= 9) {
                stream.WriteByte(digits[i]);
            }
            else {
                stream.WriteByte(TagInteger);
                WriteInt(stream, i);
                stream.WriteByte(TagSemicolon);
            }
        }

        public static void Write(Stream stream, uint i) {
            if (i >= 0 && i <= 9) {
                stream.WriteByte(digits[i]);
            }
            else {
                stream.WriteByte(TagLong);
                WriteInt(stream, i);
                stream.WriteByte(TagSemicolon);
            }
        }

        public static void Write(Stream stream, long i) {
            if (i >= 0 && i <= 9) {
                stream.WriteByte(digits[i]);
            }
            else {
                stream.WriteByte(TagLong);
                WriteInt(stream, i);
                stream.WriteByte(TagSemicolon);
            }
        }

        public static void Write(Stream stream, ulong i) {
            if (i >= 0 && i <= 9) {
                stream.WriteByte(digits[i]);
            }
            else {
                stream.WriteByte(TagLong);
                WriteInt(stream, i);
                stream.WriteByte(TagSemicolon);
            }
        }

        public static void Write(Stream stream, bool b) => stream.WriteByte(b ? TagTrue : TagFalse);

        public static void Write(Stream stream, float n) {
            if (float.IsNaN(n)) {
                stream.WriteByte(TagNaN);
            }
            else if (float.IsInfinity(n)) {
                stream.WriteByte(TagInfinity);
                stream.WriteByte(n > 0 ? TagPos : TagNeg);
            }
            else {
                stream.WriteByte(TagDouble);
                WriteASCII(stream, n.ToString("R"));
                stream.WriteByte(TagSemicolon);
            }
        }

        public static void Write(Stream stream, double n) {
            if (double.IsNaN(n)) {
                stream.WriteByte(TagNaN);
            }
            else if (double.IsInfinity(n)) {
                stream.WriteByte(TagInfinity);
                stream.WriteByte(n > 0 ? TagPos : TagNeg);
            }
            else {
                stream.WriteByte(TagDouble);
                WriteASCII(stream, n.ToString("R"));
                stream.WriteByte(TagSemicolon);
            }
        }

        public static void Write(Stream stream, decimal n) {
            stream.WriteByte(TagDouble);
            WriteASCII(stream, n.ToString());
            stream.WriteByte(TagSemicolon);
        }

        public static void Write(Stream stream, BigInteger n) {
            stream.WriteByte(TagLong);
            WriteASCII(stream, n.ToString());
            stream.WriteByte(TagSemicolon);
        }

        public static void Write(Stream stream, IntPtr n) {
            if (IntPtr.Size == 4) {
                Write(stream, n.ToInt32());
            }
            else {
                Write(stream, n.ToInt64());
            }
        }

        public static void Write(Stream stream, UIntPtr n) {
            if (UIntPtr.Size == 4) {
                Write(stream, n.ToUInt32());
            }
            else {
                Write(stream, n.ToUInt64());
            }
        }

        public static void Write(Stream stream, char c) {
            stream.WriteByte(TagUTF8Char);
            if (c < 0x80) {
                stream.WriteByte((byte)c);
            }
            else if (c < 0x800) {
                stream.WriteByte((byte)(0xc0 | (c >> 6)));
                stream.WriteByte((byte)(0x80 | (c & 0x3f)));
            }
            else {
                stream.WriteByte((byte)(0xe0 | (c >> 12)));
                stream.WriteByte((byte)(0x80 | ((c >> 6) & 0x3f)));
                stream.WriteByte((byte)(0x80 | (c & 0x3f)));
            }
        }

        public static void Write(Stream stream, char[] s) {
            int length = s.Length;
            if (length > 0) {
                WriteInt(stream, length);
            }
            stream.WriteByte(TagQuote);
            byte[] buf = UTF8.GetBytes(s);
            stream.Write(buf, 0, buf.Length);
            stream.WriteByte(TagQuote);
        }

        public static void Write(Stream stream, string s) {
            int length = s.Length;
            if (length > 0) {
                WriteInt(stream, length);
            }
            stream.WriteByte(TagQuote);
            byte[] buf = UTF8.GetBytes(s);
            stream.Write(buf, 0, buf.Length);
            stream.WriteByte(TagQuote);
        }

        public static void Write(Stream stream, DateTime datetime) {
            int year = datetime.Year;
            int month = datetime.Month;
            int day = datetime.Day;
            int hour = datetime.Hour;
            int minute = datetime.Minute;
            int second = datetime.Second;
            int millisecond = datetime.Millisecond;
            int ticks = (int)(datetime.Ticks % 10000);
            byte tag = (datetime.Kind == DateTimeKind.Utc) ? TagUTC : TagSemicolon;
            if ((hour == 0) && (minute == 0) && (second == 0) && (millisecond == 0) && (ticks == 0)) {
                WriteDate(stream, year, month, day);
                stream.WriteByte(tag);
            }
            else if ((year == 1970) && (month == 1) && (day == 1)) {
                WriteTime(stream, hour, minute, second, millisecond, ticks);
                stream.WriteByte(tag);
            }
            else {
                WriteDate(stream, year, month, day);
                WriteTime(stream, hour, minute, second, millisecond, ticks);
                stream.WriteByte(tag);
            }
        }

        public static void WriteDate(Stream stream, int year, int month, int day) {
            stream.WriteByte(TagDate);
            stream.WriteByte((byte)('0' + (year / 1000 % 10)));
            stream.WriteByte((byte)('0' + (year / 100 % 10)));
            stream.WriteByte((byte)('0' + (year / 10 % 10)));
            stream.WriteByte((byte)('0' + (year % 10)));
            stream.WriteByte((byte)('0' + (month / 10 % 10)));
            stream.WriteByte((byte)('0' + (month % 10)));
            stream.WriteByte((byte)('0' + (day / 10 % 10)));
            stream.WriteByte((byte)('0' + (day % 10)));
        }

        public static void WriteTime(Stream stream, int hour, int minute, int second, int millisecond, int ticks) {
            stream.WriteByte(TagTime);
            stream.WriteByte((byte)('0' + (hour / 10 % 10)));
            stream.WriteByte((byte)('0' + (hour % 10)));
            stream.WriteByte((byte)('0' + (minute / 10 % 10)));
            stream.WriteByte((byte)('0' + (minute % 10)));
            stream.WriteByte((byte)('0' + (second / 10 % 10)));
            stream.WriteByte((byte)('0' + (second % 10)));
            if (millisecond > 0 || ticks > 0) {
                stream.WriteByte(TagPoint);
                stream.WriteByte((byte)('0' + (millisecond / 100 % 10)));
                stream.WriteByte((byte)('0' + (millisecond / 10 % 10)));
                stream.WriteByte((byte)('0' + (millisecond % 10)));
                if (ticks > 0) {
                    stream.WriteByte((byte)('0' + (ticks / 1000 % 10)));
                    stream.WriteByte((byte)('0' + (ticks / 100 % 10)));
                    stream.WriteByte((byte)('0' + (ticks / 10 % 10)));
                    if (ticks % 10 > 0) {
                        stream.WriteByte((byte)('0' + (ticks % 10)));
                        stream.WriteByte((byte)'0');
                        stream.WriteByte((byte)'0');
                    }
                }
            }
        }

    }
}
