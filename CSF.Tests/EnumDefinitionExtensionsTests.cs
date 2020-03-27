//
// EnumDefinitionExtensionsTests.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2020 Craig Fowler
//AssertIsEnum
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
    public class EnumDefinitionExtensionsTests
    {
        [Test, AutoMoqData]
        public void AssertIsEnum_does_not_throw_if_type_is_enum()
        {
            Assert.That(() => EnumDefinitionExtensions.AssertIsEnum<AnEnum>(), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void AssertIsEnum_does_not_throw_if_type_is_a_flags_enum()
        {
            Assert.That(() => EnumDefinitionExtensions.AssertIsEnum<AFlagsEnum>(), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void AssertIsEnum_throws_if_type_is_not_enum()
        {
            Assert.That(() => EnumDefinitionExtensions.AssertIsEnum<NotAnEnum>(), Throws.InstanceOf<MustBeEnumException>());
        }

        [Test, AutoMoqData]
        public void AssertIsEnum_non_generic_throws_if_type_is_null()
        {
            Assert.That(() => EnumDefinitionExtensions.AssertIsEnum(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void AssertIsEnum_non_generic_does_not_throw_if_type_is_enum()
        {
            Assert.That(() => EnumDefinitionExtensions.AssertIsEnum(typeof(AnEnum)), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void AssertIsEnum_non_generic_does_not_throw_if_type_is_a_flags_enum()
        {
            Assert.That(() => EnumDefinitionExtensions.AssertIsEnum(typeof(AFlagsEnum)), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void AssertIsEnum_non_generic_throws_if_type_is_not_enum()
        {
            Assert.That(() => EnumDefinitionExtensions.AssertIsEnum(typeof(NotAnEnum)), Throws.InstanceOf<MustBeEnumException>());
        }

        [Test, AutoMoqData]
        public void IsDefined_returns_true_for_defined_value()
        {
            var val = AnEnum.Three;
            Assert.That(() => val.IsDefined(), Is.True);
        }

        [Test, AutoMoqData]
        public void IsDefined_returns_false_for_undefined_value()
        {
            var val = (AnEnum) 999; // This value doesn't exist
            Assert.That(() => val.IsDefined(), Is.False);
        }

        [Test, AutoMoqData]
        public void AssertIsDefined_returns_true_for_defined_value()
        {
            var val = AnEnum.Three;
            Assert.That(() => val.AssertIsDefined(), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void AssertIsDefined_throws_for_undefined_value()
        {
            var val = (AnEnum)999; // This value doesn't exist
            Assert.That(() => val.AssertIsDefined(), Throws.InstanceOf<MustBeDefinedException>());
        }

        [Test, AutoMoqData]
        public void AssertIsDefined_includes_name_in_message__if_provided_when_it_throws()
        {
            var val = (AnEnum)999; // This value doesn't exist
            Assert.That(() => val.AssertIsDefined("FooBarBaz"), Throws.InstanceOf<MustBeDefinedException>().With.Message.Matches(@"'FooBarBaz':"));
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
            Four = 4
        }

        #endregion
    }
}
