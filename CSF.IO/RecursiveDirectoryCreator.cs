﻿//
// RecursiveDirectoryCreator.cs
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
using System.IO;

namespace CSF
{
    /// <summary>
    /// A service which can create directories, optionally doing so recursively.
    /// </summary>
    public class RecursiveDirectoryCreator : ICreatesDirectory
    {
        /// <summary>
        /// Create the specified directory.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        /// <param name="recursive">If set to <c>true</c> then this operation is recursive, creating parent directories where required.</param>
        public void Create(DirectoryInfo directory, bool recursive = false)
        {
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            if (directory == directory.Root)
                throw new IOException($"This method cannot be used to create a root directory: {directory.FullName}");
            if (recursive && !directory.Root.Exists)
                throw new IOException($"Cannot create directory recursively; its root directory must exist: {directory.Root.FullName}");

            if (!recursive)
            {
                directory.Create();
                directory.Refresh();
                return;
            }

            var current = directory;
            var toCreate = new Stack<DirectoryInfo>();

            while (!current.Exists)
            {
                toCreate.Push(current);
                current = current.Parent;
            }

            foreach (var dir in toCreate)
            {
                dir.Create();
                dir.Refresh();
            }
        }
    }
}
