// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.Docker.Tests
{
    public static class StringExtensions
    {
        public static string TrimEnd(this string source, string trimString)
        {
            while (source.EndsWith(trimString))
            {
                source = source.Substring(0, source.Length - trimString.Length);
            }

            return source;
        }
    }
}
