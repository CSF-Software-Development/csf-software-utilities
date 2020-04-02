//
// GuidExtensionsTests.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2020 Craig Fowler
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
using NUnit.Framework;

namespace CSF
{
    [TestFixture,Parallelizable]
    public class GuidExtensionsTests
    {
        [Test]
        public void ToRFC4122ByteArray_gets_correct_byte_array_for_a_guid()
        {
            var guid = new Guid("4761b60e-e29c-41b9-bf96-98ec7a549712");
            Assert.That(() => guid.ToRFC4122ByteArray(),
                        Is.EqualTo(new byte[] { 71, 97, 182, 14, 226, 156, 65, 185, 191, 150, 152, 236, 122, 84, 151, 18 }));
        }

        [Test]
        public void ToRFC4122Guid_gets_correct_guid_from_byte_array()
        {
            var array = new byte[] { 71, 97, 182, 14, 226, 156, 65, 185, 191, 150, 152, 236, 122, 84, 151, 18 };
            Assert.That(() => array.ToRFC4122Guid(),
                        Is.EqualTo(new Guid("4761b60e-e29c-41b9-bf96-98ec7a549712")));
        }

        [Test]
        public void ReorderBytesForRFC4122_executed_twice_will_return_the_original_byte_array()
        {
            byte[] array = { 14, 182, 97, 71, 156, 226, 185, 65, 191, 150, 152, 236, 122, 84, 151, 18 };

            var reordered = GuidExtensions.ReorderBytesForRFC4122(array, true);
            var reorderedBack = GuidExtensions.ReorderBytesForRFC4122(reordered, true);

            Assert.That(reorderedBack, Is.EqualTo(array));
        }

        [Test]
        public void ReorderBytesForRFC4122_orders_the_array_correctly()
        {
            byte[]
                original = { 14, 182, 97, 71, 156, 226, 185, 65, 191, 150, 152, 236, 122, 84, 151, 18 },
                expected = { 71, 97, 182, 14, 226, 156, 65, 185, 191, 150, 152, 236, 122, 84, 151, 18 };

            var result = GuidExtensions.ReorderBytesForRFC4122(original, true);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
