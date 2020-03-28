//
// StaticGuidAttribute.cs
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
    /// Applies a customization which always creates the same <see cref="Guid"/> instance using a predictable byte array.
    /// </summary>
    public class StaticGuidAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new StaticGuidCustomization(parameter);
    }

    class StaticGuidCustomization : ICustomization
    {
        readonly ParameterInfo parameter;

        public void Customize(IFixture fixture)
        {
            var builder = new FilteringSpecimenBuilder(new StaticGuidSpecimenBuilder(),
                                                       new ParameterSpecification(parameter.ParameterType, parameter.Name));
            fixture.Customizations.Add(builder);
        }

        public StaticGuidCustomization(ParameterInfo parameter)
        {
            this.parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }
    }

    class StaticGuidSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            return GetGuidBuilder();
        }

        IGetsGuid GetGuidBuilder()
        {
            var guid = new Guid(new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF });
            return Mock.Of<IGetsGuid>(x => x.GetGuid() == guid);
        }
    }
}
