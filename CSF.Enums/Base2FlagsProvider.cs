//
// Base2FlagsProvider.cs
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

namespace CSF
{
    interface IGetsBase2NumericValues
    {
        IEnumerable<object> GetBase2FlagValues(object numericValue);
    }

    class Base2FlagsProvider<T> : IGetsBase2NumericValues where T : struct
    {
        readonly Type enumType;

        public IEnumerable<T> GetBase2FlagValues(T numericValue)
        {
            var maxExponent = GetMaximumExponent();
            var ulongNumeric = GetUlong(numericValue);

            for (var exponent = 0; exponent <= maxExponent && ulongNumeric > 0; exponent++)
            {
                var candidateValue = (T) Convert.ChangeType(Math.Pow(2, exponent), typeof(T));

                // If this candidate numeric value doesn't correspond to a defined enum constant then skip it
                if (!Enum.IsDefined(enumType, candidateValue))
                    continue;

                var ulongCandidate = GetUlong(candidateValue);
                if ((ulongNumeric & ulongCandidate) == ulongCandidate)
                {
                    ulongNumeric -= ulongCandidate;
                    yield return candidateValue;
                }
            }
        }

        IEnumerable<object> IGetsBase2NumericValues.GetBase2FlagValues(object numericValue)
            => GetBase2FlagValues((T)numericValue).Cast<object>();

        static ulong GetUlong(T number)
        {
            if (typeof(T) == typeof(ulong))
                return (ulong) Convert.ChangeType(number, typeof(ulong));

            // Any number that's not a ulong can be converted to long, then
            // we can convert the absolute value of that to ulong
            var longNumber = (long)Convert.ChangeType(number, typeof(long));
            return (ulong)Math.Abs(longNumber);
        }

        static int GetMaximumExponent()
        {
            if (typeof(T) == typeof(byte))
                return 7;
            if (typeof(T) == typeof(ushort))
                return 15;
            if (typeof(T) == typeof(uint))
                return 31;
            if (typeof(T) == typeof(ulong))
                return 63;
            if (typeof(T) == typeof(sbyte))
                return 6;
            if (typeof(T) == typeof(short))
                return 14;
            if (typeof(T) == typeof(int))
                return 30;
            if (typeof(T) == typeof(long))
                return 62;
            return 0;
        }

        public Base2FlagsProvider(Type enumType)
        {
            this.enumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
        }
    }
}
