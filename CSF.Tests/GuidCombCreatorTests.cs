//
// BinaryGuidCombStrategyTests.cs
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
using Moq;
using NUnit.Framework;

namespace CSF
{
    [TestFixture,Parallelizable]
    public class GuidCombCreatorTests
    {
        [Test, AutoMoqData]
        public void GetGuid_returns_two_different_guids_if_executed_twice_with_default_options()
        {
            var sut = new GuidCombCreator();

            var guid1 = sut.GetGuid();
            var guid2 = sut.GetGuid();

            Assert.That(guid1, Is.Not.EqualTo(guid2));
        }

        [Test, AutoMoqData]
        public void GetGuid_substitutes_correct_bytes_at_beginning_of_guid_when_timestamp_part_is_3_bytes([StaticGuid] IGetsGuid wrappedCreator,
                                                                                                          [TicksFromBytes] IGetsTimestamp timestampProvider)
        {
            var sut = new GuidCombCreator(3, timestampBytesBigEndian: true, timestampProvider: timestampProvider, wrappedStrategy: wrappedCreator);

            var result = sut.GetGuid();

            Assert.That(() => result.ToByteArray(),
                        Is.EqualTo(new byte[] { 0x01, 0x02, 0x03, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF }));
        }

        [Test, AutoMoqData]
        public void GetGuid_substitutes_correct_bytes_at_beginning_of_guid_when_timestamp_part_is_6_bytes([StaticGuid] IGetsGuid wrappedCreator,
                                                                                                          [TicksFromBytes] IGetsTimestamp timestampProvider)
        {
            var sut = new GuidCombCreator(6, timestampBytesBigEndian: true, timestampProvider: timestampProvider, wrappedStrategy: wrappedCreator);

            var result = sut.GetGuid();

            Assert.That(() => result.ToByteArray(),
                        Is.EqualTo(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF }));
        }

        [Test, AutoMoqData]
        public void GetGuid_substitutes_correct_bytes_at_beginning_of_guid_when_timestamp_part_is_little_endian_6_bytes([StaticGuid] IGetsGuid wrappedCreator,
                                                                                                                        [TicksFromBytes] IGetsTimestamp timestampProvider)
        {
            var sut = new GuidCombCreator(6, timestampBytesBigEndian: false, timestampProvider: timestampProvider, wrappedStrategy: wrappedCreator);

            var result = sut.GetGuid();

            Assert.That(() => result.ToByteArray(),
                        Is.EqualTo(new byte[] { 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF }));
        }

        [Test, AutoMoqData]
        public void GetGuid_substitutes_correct_bytes_at_end_of_guid_when_timestamp_part_is_6_bytes([StaticGuid] IGetsGuid wrappedCreator,
                                                                                                                        [TicksFromBytes] IGetsTimestamp timestampProvider)
        {
            var sut = new GuidCombCreator(6, timestampBytesBigEndian: true, timestampProvider: timestampProvider, wrappedStrategy: wrappedCreator, timestampBytesAtBeginning: false);

            var result = sut.GetGuid();

            Assert.That(() => result.ToByteArray(),
                        Is.EqualTo(new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }));
        }
    }
}
