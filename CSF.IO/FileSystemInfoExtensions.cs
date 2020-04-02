//
// FileSystemInfoExtensions.cs
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
using System.IO;

namespace CSF
{
    /// <summary>
    /// Extension methods for <see cref="FileSystemInfo"/> (most commonly <see cref="FileInfo"/>
    /// and <see cref="DirectoryInfo"/>.
    /// </summary>
    public static class FileSystemInfoExtensions
    {
#if NETSTANDARD1_3
        const StringComparison Comparison = StringComparison.Ordinal;
#else
        const StringComparison Comparison = StringComparison.InvariantCulture;
#endif

        /// <summary>
        /// <para>
        /// Determines whether the specified <see cref="FileSystemInfo"/> is a child of (contained within) the
        /// specified <see cref="DirectoryInfo"/>.
        /// </para>
        /// <para>
        /// Note that this is known to work for Windows or POSIX-style filesystems, but may fail on other
        /// more esoteric platforms.
        /// </para>
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is child of the specified directory; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='info'>
        /// An instance of <see cref="FileSystemInfo"/> to test whether or not it is contained within
        /// the <paramref name="directory"/>.
        /// </param>
        /// <param name='directory'>
        /// The directory for which to test whether the <paramref name="info"/> is contained.
        /// </param>
        /// <exception cref='ArgumentNullException'>If either <paramref name="info"/> or <paramref name="directory"/> are null.</exception>
        public static bool IsContainedWithin(this FileSystemInfo info, DirectoryInfo directory)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            return info.FullName.StartsWith(directory.FullName, Comparison);
        }

        /// <summary>
        /// <para>
        /// Gets a relative path that represents the current instance's relative path from a
        /// specified <paramref name="root"/> directory.
        /// </para>
        /// <para>
        /// Note that this is known to work for Windows or POSIX-style filesystems, but may fail on other
        /// more esoteric platforms.
        /// </para>
        /// </summary>
        /// <returns>
        /// The relative path component.
        /// </returns>
        /// <param name='info'>
        /// The current <see cref="FileSystemInfo"/> instance.
        /// </param>
        /// <param name='root'>
        /// The root directory from which to create the output
        /// </param>
        /// <exception cref='ArgumentNullException'>If either <paramref name="info"/> or <paramref name="root"/> are null.</exception>
        /// <exception cref='ArgumentException'>If <paramref name="info"/> is not contained within <paramref name="root"/>.</exception>
        public static string GetRelativePath(this FileSystemInfo info, DirectoryInfo root)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            if (!info.IsContainedWithin(root))
            {
                var message = $"The item '{info.FullName}' must be a child of the root directory '{root.FullName}'";
                throw new ArgumentException(message, nameof(info));
            }

            return info.FullName.Substring(root.FullName.Length);
        }

        /// <summary>
        /// Gets the parent of the specified <see cref="FileSystemInfo"/>, or null if the
        /// object is the root of its filesystem.
        /// </summary>
        /// <returns>
        /// The parent directory.
        /// </returns>
        /// <param name='info'>A <see cref="FileSystemInfo"/></param>
        /// <exception cref='ArgumentNullException'>If <paramref name="info"/> is null.</exception>
        public static DirectoryInfo GetParentDirectory(this FileSystemInfo info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (info is DirectoryInfo dir) return dir.Parent;
            if (info is FileInfo file) return file.Directory;

            throw new ArgumentException($"The filesystem info must be either a {nameof(FileInfo)} or a {nameof(DirectoryInfo)}.");
        }
    }
}

