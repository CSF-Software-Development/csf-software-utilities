//
// BinaryGuidCombStrategy.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace CSF
{
    /// <summary>
    /// A strategy for creating <see cref="Guid"/> instances which uses the COMB algorithm.
    /// </summary>
    /// <remarks>
    /// <para>
    /// COMB stands for "Combined GUID", it is a strategy whereby a few bytes of the GUID's data (16 bytes total) are
    /// generated using a timestamp-based mechanism instead of being truly random.  In a way this means that the
    /// GUIDs generated using this algorithm are semi-sequential.  This is particularly useful where the generated
    /// GUID is going to be used in a hash table or database index.  By using semi-sequential objects, fragmentation
    /// of the hash table/index is avoided and performance may be improved.
    /// </para>
    /// <para>
    /// This work is mainly based on the article found at https://www.informit.com/articles/article.aspx?p=25862
    /// and particularly this page: https://www.informit.com/articles/article.aspx?p=25862&seqNum=7
    /// </para>
    /// </remarks>
    public class BinaryGuidCombStrategy : IGetsGuid
    {
        readonly IGetsTimestamp timestampProvider;
        readonly int timestampByteCount;
        readonly bool
            timestampBytesBigEndian,
            timestampBytesAtBeginning,
            systemIsLittleEndian = BitConverter.IsLittleEndian,
            useRFC4122ByteOrder;

        /// <summary>
        /// Gets a GUID, based upon the COMB algorithm.
        /// </summary>
        /// <returns>The GUID.</returns>
        public Guid GetGuid()
        {
            var timestampBytes = GetMostSignificantTimestampBytes();
            var guid = Guid.NewGuid();
            var guidBytes = useRFC4122ByteOrder? guid.ToRFC4122ByteArray() : guid.ToByteArray();

            var timestampBytesStartPosition = timestampBytesAtBeginning ? 0 : 16 - timestampByteCount;
            Array.Copy(timestampBytes, 0, guidBytes, timestampBytesStartPosition, timestampByteCount);

            return useRFC4122ByteOrder ? guidBytes.ToRFC4122Guid() : new Guid(guidBytes);
        }

        byte[] GetMostSignificantTimestampBytes()
        {
            const int TicksBytes = 8;

            var timestampBytes = BitConverter.GetBytes(timestampProvider.GetTimestamp().Ticks);
            var copyStartPosition = systemIsLittleEndian ? TicksBytes - timestampByteCount : 0;

            var output = new byte[timestampByteCount];
            Array.Copy(timestampBytes, copyStartPosition, output, 0, timestampByteCount);

            if (ShouldReverseTimestampBytes())
                Array.Reverse(output);

            return output;
        }

        bool ShouldReverseTimestampBytes() => timestampBytesBigEndian == systemIsLittleEndian;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryGuidCombStrategy"/> class.
        /// </summary>
        /// <param name="timestampByteCount">The number of bytes of the timestamp to use, must be between 3 and 6. The default is 4.</param>
        /// <param name="timestampBytesAtBeginning">
        /// A value indicating whether or not the timestamp-based bytes should be placed at the beginning or end of the generated <see cref="Guid"/>.
        /// The default is true.
        /// </param>
        /// <param name="timestampBytesBigEndian">
        /// A value indicating whether the timestamp-based bytes should be stored in a big or little endian format.
        /// The default is to use the 'endianness' of the current environment.
        /// See <seealso cref="BitConverter.IsLittleEndian"/>
        /// </param>
        /// <param name="timestampProvider">
        /// A service used to get the current timestamp.
        /// The default is a timestamp-provider based on the current UTC time.
        /// </param>
        /// <param name="useRFC4122ByteOrder">
        /// A value indicating whether or not to use the RFC 4122 ordering for bytes (compliant with the UUID specification).
        /// The default is false.
        /// </param>
        public BinaryGuidCombStrategy(int timestampByteCount = 4,
                                      bool timestampBytesAtBeginning = true,
                                      bool? timestampBytesBigEndian = null,
                                      IGetsTimestamp timestampProvider = null,
                                      bool useRFC4122ByteOrder = false)
        {
            if (timestampByteCount < 3 || timestampByteCount > 6)
                throw new ArgumentOutOfRangeException(nameof(timestampByteCount), "The number of timestamp-based bytes must be between 3 and 6");

            this.timestampByteCount = timestampByteCount;
            this.timestampBytesAtBeginning = timestampBytesAtBeginning;
            this.timestampBytesBigEndian = timestampBytesBigEndian ?? !BitConverter.IsLittleEndian;
            this.timestampProvider = timestampProvider ?? UtcTimestampProvider.Default;
            this.useRFC4122ByteOrder = useRFC4122ByteOrder;
        }
    }
}

