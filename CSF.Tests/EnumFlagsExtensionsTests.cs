//
// EnumFlagsExtensionsTests.cs
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
    [TestFixture, Parallelizable]
    public class EnumFlagsExtensionsTests
    {
        [Test, AutoMoqData]
        public void AssertIsFlagsEnum_throws_if_type_is_non_flags_enum()
        {
            Assert.That(() => EnumFlagsExtensions.AssertIsFlagsEnum<AnEnum>(), Throws.InstanceOf<MustBeFlagsEnumException>());
        }

        [Test, AutoMoqData]
        public void AssertIsFlagsEnum_does_not_throw_if_type_is_a_flags_enum()
        {
            Assert.That(() => EnumFlagsExtensions.AssertIsFlagsEnum<AFlagsEnum>(), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void AssertIsFlagsEnum_throws_if_type_is_not_enum()
        {
            Assert.That(() => EnumFlagsExtensions.AssertIsFlagsEnum<NotAnEnum>(), Throws.InstanceOf<MustBeEnumException>());
        }

        [Test, AutoMoqData]
        public void AssertIsFlagsEnum_non_generic_throws_if_type_is_null()
        {
            Assert.That(() => EnumFlagsExtensions.AssertIsFlagsEnum(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void AssertIsFlagsEnum_non_generic_throws_if_type_is_non_flags_enum()
        {
            Assert.That(() => EnumFlagsExtensions.AssertIsFlagsEnum(typeof(AnEnum)), Throws.InstanceOf<MustBeFlagsEnumException>());
        }

        [Test, AutoMoqData]
        public void AssertIsFlagsEnum_non_generic_does_not_throw_if_type_is_a_flags_enum()
        {
            Assert.That(() => EnumFlagsExtensions.AssertIsFlagsEnum(typeof(AFlagsEnum)), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void AssertIsFlagsEnum_non_generic_throws_if_type_is_not_enum()
        {
            Assert.That(() => EnumFlagsExtensions.AssertIsFlagsEnum(typeof(NotAnEnum)), Throws.InstanceOf<MustBeEnumException>());
        }

        [Test, AutoMoqData]
        public void GetBase2FlagValues_returns_correct_values_for_integer_flags()
        {
            var value = AFlagsEnum.One | AFlagsEnum.Four;
            Assert.That(() => value.GetBase2FlagValues(), Is.EqualTo(new[] { AFlagsEnum.One, AFlagsEnum.Four }));
        }

        [Test, AutoMoqData]
        public void GetBase2FlagValues_skips_powers_of_two_which_are_not_enum_constants()
        {
            var value = (AFlagsEnum) 26; // 2 + 8 + 16
            Assert.That(() => value.GetBase2FlagValues(), Is.EqualTo(new[] { AFlagsEnum.Two, AFlagsEnum.Sixteen }));
        }

        [Test, AutoMoqData]
        public void GetBase2FlagValues_returns_correct_values_for_byte_flags()
        {
            var value = AByteFlagsEnum.One | AByteFlagsEnum.Four;
            Assert.That(() => value.GetBase2FlagValues(), Is.EqualTo(new[] { AByteFlagsEnum.One, AByteFlagsEnum.Four }));
        }

        [Test, AutoMoqData]
        public void GetBase2FlagValues_skips_powers_of_two_which_are_not_enum_constants_for_byte_flags()
        {
            var value = (AByteFlagsEnum) 26; // 2 + 8 + 16
            Assert.That(() => value.GetBase2FlagValues(), Is.EqualTo(new[] { AByteFlagsEnum.Two, AByteFlagsEnum.Sixteen }));
        }

        #region contained types

        enum AnEnum
        {
            One = 1,
            Two,
            Three
        }

        struct NotAnEnum
        {
            public int ANumber { get; set; }
        }

        [Flags]
        enum AFlagsEnum
        {
            One = 1,
            Two = 2,
            Four = 4,
            // Eight is intentionally missing
            Sixteen = 16
        }

        [Flags]
        enum AByteFlagsEnum : byte
        {
            One = 1,
            Two = 2,
            Four = 4,
            // Eight is intentionally missing
            Sixteen = 16
        }

        #endregion
    }
}
