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
 * RawReader.cs                                           *
 *                                                        *
 * RawReader class for C#.                                *
 *                                                        *
 * LastModified: May 2, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Buffers;
using System.IO;
using System.Runtime.Serialization;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    public static class RawReader {
        private static void Copy(Stream stream, Stream ostream, byte[] buffer, int len) {
            int off = 0;
            while (off < len) {
                off += stream.Read(buffer, off, len - off);
            }
            ostream.Write(buffer, 0, len);
        }
        private static void ReadNumberRaw(Stream stream, Stream ostream) {
            int tag;
            do {
                tag = stream.ReadByte();
                ostream.WriteByte((byte)tag);
            } while (tag != TagSemicolon);
        }
        private static void ReadDateTimeRaw(Stream stream, Stream ostream) {
            int tag;
            do {
                tag = stream.ReadByte();
                ostream.WriteByte((byte)tag);
            } while (tag != TagSemicolon && tag != TagUTC);
        }
        private static void ReadUTF8CharRaw(Stream stream, Stream ostream) {
            int tag = stream.ReadByte();
            switch (tag >> 4) {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7: {
                        // 0xxx xxxx
                        ostream.WriteByte((byte)tag);
                        break;
                    }
                case 12:
                case 13: {
                        // 110x xxxx   10xx xxxx
                        ostream.WriteByte((byte)tag);
                        ostream.WriteByte((byte)stream.ReadByte());
                        break;
                    }
                case 14: {
                        // 1110 xxxx  10xx xxxx  10xx xxxx
                        ostream.WriteByte((byte)tag);
                        ostream.WriteByte((byte)stream.ReadByte());
                        ostream.WriteByte((byte)stream.ReadByte());
                        break;
                    }
                default:
                    throw ValueReader.BadEncoding(tag);
            }
        }
        private static void ReadBytesRaw(Stream stream, Stream ostream) {
            int len = 0;
            int tag = '0';
            do {
                len *= 10;
                len += tag - '0';
                tag = stream.ReadByte();
                ostream.WriteByte((byte)tag);
            } while (tag != TagQuote);
            byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
            try {
                Copy(stream, ostream, buffer, len);
            }
            finally {
                ArrayPool<byte>.Shared.Return(buffer, false);
            }
            ostream.WriteByte((byte)stream.ReadByte());
        }
        private static void ReadGuidRaw(Stream stream, Stream ostream) {
            const int len = 38;
            byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
            try {
                Copy(stream, ostream, buffer, len);
            }
            finally {
                ArrayPool<byte>.Shared.Return(buffer, false);
            }
        }
        private static void ReadStringRaw(Stream stream, Stream ostream) {
            int count = 0;
            int tag = '0';
            do {
                count *= 10;
                count += tag - '0';
                tag = stream.ReadByte();
                ostream.WriteByte((byte)tag);
            } while (tag != TagQuote);
            for (int i = 0; i < count; ++i) {
                tag = stream.ReadByte();
                switch (tag >> 4) {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7: {
                            // 0xxx xxxx
                            ostream.WriteByte((byte)tag);
                            break;
                        }
                    case 12:
                    case 13: {
                            // 110x xxxx   10xx xxxx
                            ostream.WriteByte((byte)tag);
                            ostream.WriteByte((byte)stream.ReadByte());
                            break;
                        }
                    case 14: {
                            // 1110 xxxx  10xx xxxx  10xx xxxx
                            ostream.WriteByte((byte)tag);
                            ostream.WriteByte((byte)stream.ReadByte());
                            ostream.WriteByte((byte)stream.ReadByte());
                            break;
                        }
                    case 15: {
                            // 1111 0xxx  10xx xxxx  10xx xxxx  10xx xxxx
                            if ((tag & 0xf) <= 4) {
                                ostream.WriteByte((byte)tag);
                                ostream.WriteByte((byte)stream.ReadByte());
                                ostream.WriteByte((byte)stream.ReadByte());
                                ostream.WriteByte((byte)stream.ReadByte());
                                ++i;
                                break;
                            }
                            goto default;
                            // no break here!! here need throw exception.
                        }
                    default:
                        throw ValueReader.BadEncoding(tag);
                }
            }
            ostream.WriteByte((byte)stream.ReadByte());
        }
        private static void ReadComplexRaw(Stream stream, Stream ostream) {
            int tag;
            do {
                tag = stream.ReadByte();
                ostream.WriteByte((byte)tag);
            } while (tag != TagOpenbrace);
            while ((tag = stream.ReadByte()) != TagClosebrace) {
                ReadRaw(stream, ostream, tag);
            }
            ostream.WriteByte((byte)tag);
        }
        private static void ReadRaw(Stream stream, Stream ostream) {
            ReadRaw(stream, ostream, stream.ReadByte());
        }
        public static void ReadRaw(Stream stream, Stream ostream, int tag) {
            ostream.WriteByte((byte)tag);
            switch (tag) {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case TagNull:
                case TagEmpty:
                case TagTrue:
                case TagFalse:
                case TagNaN:
                    break;
                case TagInfinity:
                    ostream.WriteByte((byte)stream.ReadByte());
                    break;
                case TagInteger:
                case TagLong:
                case TagDouble:
                case TagRef:
                    ReadNumberRaw(stream, ostream);
                    break;
                case TagDate:
                case TagTime:
                    ReadDateTimeRaw(stream, ostream);
                    break;
                case TagUTF8Char:
                    ReadUTF8CharRaw(stream, ostream);
                    break;
                case TagBytes:
                    ReadBytesRaw(stream, ostream);
                    break;
                case TagString:
                    ReadStringRaw(stream, ostream);
                    break;
                case TagGuid:
                    ReadGuidRaw(stream, ostream);
                    break;
                case TagList:
                case TagMap:
                case TagObject:
                    ReadComplexRaw(stream, ostream);
                    break;
                case TagClass:
                    ReadComplexRaw(stream, ostream);
                    ReadRaw(stream, ostream);
                    break;
                case TagError:
                    ReadRaw(stream, ostream);
                    break;
                case -1:
                    throw new EndOfStreamException();
                default:
                    throw new SerializationException("Unexpected serialize tag '" + (char)tag + "' in stream");
            }
        }
    }
}
