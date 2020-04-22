// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Docker.Tests
{
    public static class FileHelper
    {
        public static TempFolderContext UseTempFolder()
        {
            return new TempFolderContext();
        }

        /// <summary>
        /// Returns a value indicating whether the two lists are equivalent (order does not matter).
        /// </summary>
        public static bool CompareLists(IList<string> expectedItems, IList<string> items)
        {
            if (items.Count != expectedItems.Count)
            {
                return false;
            }

            items = items
                .OrderBy(p => p)
                .ToList();
            expectedItems = expectedItems
                .OrderBy(p => p)
                .ToList();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != expectedItems[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class TempFolderContext : IDisposable
    {
        public TempFolderContext()
        {
            do
            {
                Path = System.IO.Path.Combine(
                    System.IO.Path.GetTempPath(),
                    Guid.NewGuid().ToString());
            }
            while (Directory.Exists(Path));

            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            Directory.Delete(Path, true);
        }
    }
}
