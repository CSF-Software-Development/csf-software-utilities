//
// GuidExtensions.cs
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
using System.Linq;

namespace CSF
{
    /// <summary>
    /// Extension methods for the <c>System.Guid</c> type.
    /// </summary>
    public static class GuidExtensions
    {
        static readonly bool IsLittleEndian = BitConverter.IsLittleEndian;
        static readonly int[] ReorderingMap = new[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };

        /// <summary>
        /// Returns a byte array representing the specified <see cref="Guid"/> in an RFC-4122 compliant format.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The rationale for this method (and the reason for requiring it) is because Microsoft internally represent the
        /// GUID structure in a manner which does not comply with RFC-4122's definition of a UUID.  The first three blocks
        /// of data (out of 4 total) are stored using the machine's native endianness.  The RFC defines that these three
        /// blocks should be represented in big-endian format.  This does not cause a problem when getting a
        /// string-representation of the GUID, since the framework automatically converts to big-endian format before
        /// formatting as a string.  When getting a byte array equivalent of the GUID though, it can cause issues if the
        /// recipient of that byte array expects a standards-compliant UUID.
        /// </para>
        /// <para>
        /// This method checks the architecture of the current machine.  If it is little-endian then - before returning a
        /// value - the byte-order of the first three blocks of data are reversed.  If the machine is big-endian then the
        /// bytes are left untouched (since they are already correct).
        /// </para>
        /// <para>
        /// For more information, see https://en.wikipedia.org/wiki/Globally_unique_identifier#Binary_encoding
        /// </para>
        /// </remarks>
        /// <returns>
        /// A byte array representation of the <see cref="Guid"/>, in RFC-4122 compliant form.
        /// </returns>
        /// <param name='guid'>
        /// The <see cref="Guid"/> for which to get the byte array.
        /// </param>
        public static byte[] ToRFC4122ByteArray(this Guid guid) => ReorderBytesForRFC4122(guid.ToByteArray());

        /// <summary>
        /// Returns a <see cref="Guid"/>, created from the specified RFC-4122 compliant byte array.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The rationale for this method (and the reason for requiring it) is because Microsoft internally represent the
        /// GUID structure in a manner which does not comply with RFC-4122's definition of a UUID.  The first three blocks
        /// of data (out of 4 total) are stored using the machine's native endianness.  The RFC defines that these three
        /// blocks should be represented in big-endian format.  This does not cause a problem when getting a
        /// string-representation of the GUID, since the framework automatically converts to big-endian format before
        /// formatting as a string.  When getting a byte array equivalent of the GUID though, it can cause issues if the
        /// recipient of that byte array expects a standards-compliant UUID.
        /// </para>
        /// <para>
        /// This method checks the architecture of the current machine.  If it is little-endian then - before returning a
        /// value - the byte-order of the first three blocks of data are reversed.  If the machine is big-endian then the
        /// bytes are left untouched (since they are already correct).
        /// </para>
        /// <para>
        /// For more information, see https://en.wikipedia.org/wiki/Globally_unique_identifier#Binary_encoding
        /// </para>
        /// </remarks>
        /// <returns>
        /// A <see cref="Guid"/>, created from the given byte array.
        /// </returns>
        /// <param name='guidBytes'>
        /// A byte array representing a <see cref="Guid"/>, in RFC-4122 compliant form.
        /// </param>
        public static Guid ToRFC4122Guid(this byte[] guidBytes) => new Guid(ReorderBytesForRFC4122(guidBytes));

        /// <summary>
        /// Gets a copy of a 16-byte array, where the bytes in blocks 0-3, 4-5 &amp; 6-7 are
        /// reversed.  The bytes in the block 8-15 are left as they are.  This allows us to transform
        /// a <see cref="Guid"/> into an RFC-4122 UUID where we are executing on a little-endian environment.
        /// </summary>
        /// <returns>
        /// A copy of the original byte array, with the modifications.
        /// </returns>
        /// <param name='bytes'>
        /// A byte array of length 16, representing a <see cref="Guid"/>.
        /// </param>
        /// <param name="forceReorder">If <c>true</c> the the reordering of bytes is forced, even on a big-endian architecture.</param>
        internal static byte[] ReorderBytesForRFC4122(byte[] bytes, bool forceReorder = false)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length != 16)
                throw new ArgumentException($"A {nameof(Guid)} must have precisely 16 bytes.", nameof(bytes));

            // No reordering needed on big-endian environments
            if (!IsLittleEndian && !forceReorder) return bytes;

            return bytes
                .Select((val, idx) => {
                    var mappedPosition = ReorderingMap[idx];
                    return bytes[mappedPosition];
                })
                .ToArray();
        }
    }
}

