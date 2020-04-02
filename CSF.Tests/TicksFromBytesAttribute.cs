//
// TicksFromBytesAttribute.cs
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
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using Moq;

namespace CSF
{
    /// <summary>
    /// Applies a customization which always creates the same <see cref="DateTime"/> instance using a predictable
    /// byte array (representing the ticks in the created date/time).
    /// </summary>
    public class TicksFromBytesAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new TicksFromBytesCustomization(parameter);
    }

    class TicksFromBytesCustomization : ICustomization
    {
        readonly ParameterInfo parameter;

        public void Customize(IFixture fixture)
        {
            var builder = new FilteringSpecimenBuilder(new TicksFromBytesSpecimenBuilder(),
                                                       new ParameterSpecification(parameter.ParameterType, parameter.Name));
            fixture.Customizations.Add(builder);
        }

        public TicksFromBytesCustomization(ParameterInfo parameter)
        {
            this.parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }
    }

    class TicksFromBytesSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            return GetTimestampBuilder();
        }

        IGetsTimestamp GetTimestampBuilder()
        {
            return Mock.Of<IGetsTimestamp>(x => x.GetTimestamp() == GetDateTimeFromBytes());
        }

        DateTime GetDateTimeFromBytes()
        {
            var timeSource = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, };
            if (BitConverter.IsLittleEndian) Array.Reverse(timeSource);
            var ticks = BitConverter.ToInt64(timeSource, 0);
            return DateTime.FromBinary(ticks);
        }
    }
}
