//
// FileSystemInfoExtensionsTests.cs
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
using System.IO;
using NUnit.Framework;

namespace CSF
{
    [TestFixture,Parallelizable]
    public class FileSystemInfoExtensionsTests
    {
        [Test]
        public void IsContainedWithin_returns_true_for_a_subdirectory()
        {
            var baseDirectory = new DirectoryInfo(@"C:\RootFolder\");
            var subDirectory = new DirectoryInfo(@"C:\RootFolder\Subdirectory\");

            Assert.That(() => subDirectory.IsContainedWithin(baseDirectory), Is.True);
        }

        [Test]
        public void IsContainedWithin_returns_true_for_a_grandchild_directory()
        {
            var baseDirectory = new DirectoryInfo(@"C:\RootFolder\");
            var subDirectory = new DirectoryInfo(@"C:\RootFolder\Subdirectory\GrandchildDirectory\");

            Assert.That(() => subDirectory.IsContainedWithin(baseDirectory), Is.True);
        }

        [Test]
        public void IsContainedWithin_returns_true_for_a_grandchild_file()
        {
            var baseDirectory = new DirectoryInfo(@"C:\RootFolder\");
            var subDirectory = new FileInfo(@"C:\RootFolder\Subdirectory\GrandchildFile.txt");

            Assert.That(() => subDirectory.IsContainedWithin(baseDirectory), Is.True);
        }

        [Test]
        public void IsContainedWithin_returns_false_for_file_on_a_different_drive()
        {
            var baseDirectory = new DirectoryInfo(@"C:\RootFolder\");
            var subDirectory = new FileInfo(@"F:\RootFolder\Subdirectory\GrandchildFile.txt");

            Assert.That(() => subDirectory.IsContainedWithin(baseDirectory), Is.False);
        }

        [Test]
        public void GetRelativePath_returns_correct_relative_path_for_granchild_file()
        {
            var baseDirectory = new DirectoryInfo(@"C:\RootFolder\");
            var subDirectory = new FileInfo(@"C:\RootFolder\Subdirectory\GrandchildFile.txt");

            Assert.That(() => subDirectory.GetRelativePath(baseDirectory), Is.EqualTo(@"Subdirectory\GrandchildFile.txt"));
        }

        [Test]
        public void GetRelativePath_throws_for_file_on_a_different_drive()
        {
            var baseDirectory = new DirectoryInfo(@"C:\RootFolder\");
            var subDirectory = new FileInfo(@"F:\RootFolder\Subdirectory\GrandchildFile.txt");

            Assert.That(() => subDirectory.GetRelativePath(baseDirectory), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void GetParentDirectory_gets_correct_directory_for_a_file()
        {
            var filePath = Path.Join(TestContext.CurrentContext.WorkDirectory, "SampleFile");
            var fileInfo = new FileInfo(filePath);

            Assert.That(() => fileInfo.GetParentDirectory(), Is.EqualTo(new DirectoryInfo(TestContext.CurrentContext.WorkDirectory)));
        }

        [Test]
        public void GetParentDirectory_gets_correct_directory_for_a_directory()
        {
            var dirPath = Path.Join(TestContext.CurrentContext.WorkDirectory, "SampleDirectory");
            var dirInfo = new DirectoryInfo(dirPath);

            Assert.That(() => dirInfo.GetParentDirectory(), Is.EqualTo(new DirectoryInfo(TestContext.CurrentContext.WorkDirectory)));
        }
    }
}
