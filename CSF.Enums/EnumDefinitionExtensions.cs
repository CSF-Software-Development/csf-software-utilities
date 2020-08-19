//
// EnumExtensions.cs
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
using System.Reflection;

namespace CSF
{
    /// <summary>
    /// Static and extension methods related to detecting &amp; asserting when the value of an enum is a defined
    /// constant.
    /// </summary>
    public static class EnumDefinitionExtensions
    {
        /// <summary>
        /// Asserts that the specified <c>System.Type</c> is an enum type and throws an exception if it is not.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <exception cref="MustBeEnumException">If <typeparamref name="T"/> is not an enum</exception>
        public static void AssertIsEnum<T>() where T : struct => AssertIsEnum(typeof(T));

        /// <summary>
        /// Asserts that the specified <c>System.Type</c> is an enum type and throws an exception if it is not.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="type"/> is <c>null</c></exception>
        /// <exception cref="MustBeEnumException">If <paramref name="type"/> is not an enum</exception>
        public static void AssertIsEnum(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!type.GetTypeInfo().IsEnum) throw new MustBeEnumException($"The type {type.FullName} must be an enum.");
        }

        /// <summary>
        /// Determines whether the given enumeration value is a defined value of its parent enumeration.
        /// </summary>
        /// <c>true</c> if the given value is a defined value of its associated enumeration; otherwise, <c>false</c>.
        /// <param name="value">The enumeration value to analyse.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <exception cref="MustBeEnumException">If <typeparamref name="T"/> is not an enum</exception>
        public static bool IsDefined<T>(this T value) where T : struct
        {
            AssertIsEnum<T>();
            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Asserts that the given enumeration value is a defined constant of the specified enum type and raises an
        /// exception if it is not.
        /// </summary>
        /// <param name="value">The enumeration value upon which to perform the assertion.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <exception cref="MustBeEnumException">If <typeparamref name="T"/> is not an enum</exception>
        /// <exception cref="MustBeDefinedException">If the <paramref name="value"/> is not a defined constant of the enum type <typeparamref name="T"/></exception>
        public static void AssertIsDefined<T>(this T value) where T : struct
        {
            if (!IsDefined(value))
                throw new MustBeDefinedException($"The value {value} must be a defined constant of the enum {typeof(T).Name}.");
        }

        /// <summary>
        /// Asserts that the given enumeration value is a defined constant of the specified enum type and raises an
        /// exception if it is not.
        /// </summary>
        /// <param name="value">The enumeration value upon which to perform the assertion.</param>
        /// <param name="name">A name identifying the value, to provide improved context when identifying the problematic value.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <exception cref="MustBeEnumException">If <typeparamref name="T"/> is not an enum</exception>
        /// <exception cref="MustBeDefinedException">If the <paramref name="value"/> is not a defined constant of the enum type <typeparamref name="T"/></exception>
        public static void AssertIsDefined<T>(this T value, string name) where T : struct
        {
            if (!IsDefined(value))
                throw new MustBeDefinedException($"The value of '{name}': {value} must be a defined constant of the enum {typeof(T).Name}.");
        }
    }
}

