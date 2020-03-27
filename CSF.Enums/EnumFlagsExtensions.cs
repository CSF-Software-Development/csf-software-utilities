//
// EnumFlagsExtensions.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF
{
    /// <summary>
    /// Static and extension methods related to enum types which have the <c>[Flags]</c> attribute applied.
    /// </summary>
    public static class EnumFlagsExtensions
    {
        /// <summary>
        /// Asserts that the specified type is an enum type which is decorated with the <c>[Flags]</c> attribute,
        /// and raises an exception if it is not.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <exception cref="MustBeEnumException">If <typeparamref name="T"/> is not an enum</exception>
        /// <exception cref="MustBeFlagsEnumException">If <typeparamref name="T"/> is not decorated with the <c>[Flags]</c> attribute</exception>
        public static void AssertIsFlagsEnum<T>() where T : struct => AssertIsFlagsEnum(typeof(T));

        /// <summary>
        /// Asserts that the specified type is an enum type which is decorated with the <c>[Flags]</c> attribute,
        /// and raises an exception if it is not.
        /// </summary>
        /// <param name="enumType">The type to check.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="enumType"/> is <c>null</c></exception>
        /// <exception cref="MustBeEnumException">If <paramref name="enumType"/> is not an enum</exception>
        /// <exception cref="MustBeFlagsEnumException">If <paramref name="enumType"/> is not decorated with the <c>[Flags]</c> attribute</exception>
        public static void AssertIsFlagsEnum(Type enumType)
        {
            EnumDefinitionExtensions.AssertIsEnum(enumType);
            if (enumType.GetTypeInfo().GetCustomAttribute<FlagsAttribute>() == null)
                throw new MustBeFlagsEnumException($"The enum type {enumType.FullName} must be decorated with {nameof(FlagsAttribute)}.");
        }

        /// <summary>
        /// For an enum type <typeparamref name="T"/> which is decorated with the <c>[Flags]</c> attribute,
        /// and which uses the recommended base-2 flags values (1, 2, 4, 8, 16 and so on), gets a collection of
        /// the individual flags constants which make up the specified <paramref name="value"/>.
        /// </summary>
        /// <returns>A collection of the individual flags constants which are present in the <paramref name="value"/>.</returns>
        /// <param name="value">The value for which to get the individual flags.</param>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <exception cref="MustBeEnumException">If <typeparamref name="T"/> is not an enum</exception>
        /// <exception cref="MustBeFlagsEnumException">If <typeparamref name="T"/> is not decorated with the <c>[Flags]</c> attribute</exception>
        public static IEnumerable<T> GetBase2FlagValues<T>(this T value) where T : struct
        {
            AssertIsFlagsEnum<T>();
            var provider = GetValuesProvider<T>();
            return provider.GetBase2FlagValues(value).Select(x => (T)x).ToList();
        }

        static IGetsBase2NumericValues GetValuesProvider<T>()
        {
            var underlying = Enum.GetUnderlyingType(typeof(T));

            if (underlying == typeof(byte))
                return new Base2FlagsProvider<byte>(typeof(T));
            if (underlying == typeof(ushort))
                return new Base2FlagsProvider<ushort>(typeof(T));
            if (underlying == typeof(uint))
                return new Base2FlagsProvider<uint>(typeof(T));
            if (underlying == typeof(ulong))
                return new Base2FlagsProvider<ulong>(typeof(T));
            if (underlying == typeof(sbyte))
                return new Base2FlagsProvider<sbyte>(typeof(T));
            if (underlying == typeof(short))
                return new Base2FlagsProvider<short>(typeof(T));
            if (underlying == typeof(int))
                return new Base2FlagsProvider<int>(typeof(T));
            if (underlying == typeof(long))
                return new Base2FlagsProvider<long>(typeof(T));

            throw new InvalidOperationException("Unsupported enum underlying type (theoretically impossible).");
        }

        static ulong GetNumericValue<T>(T enumValue) where T : struct
        {
            var underlying = Enum.GetUnderlyingType(typeof(T));
            var numeric = Convert.ChangeType(enumValue, underlying);

            if (underlying == typeof(ulong)) return (ulong)numeric;

            var longValue = (long)Convert.ChangeType(numeric, typeof(long));
            return (ulong)Math.Abs(longValue);
        }
    }
}
