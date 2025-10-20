// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dotnet.Docker;

/// <summary>
/// Helper class to manage a stack of directories for changing the current
/// working directory.
/// </summary>
internal static class DirectoryStack
{
    private static readonly Stack<string> s_directoryStack = new();

    /// <summary>
    /// Changes the current working directory to <paramref name="path"/>, and
    /// pushes the previous working directory onto the stack. Equivalent to
    /// bash's "pushd".
    /// </summary>
    public static void Push(string path)
    {
        s_directoryStack.Push(Directory.GetCurrentDirectory());
        Directory.SetCurrentDirectory(path);
    }

    /// <summary>
    /// Restores the previous working directory by popping it from the stack.
    /// If no directory was previously pushed, nothing happens. Equivalent to
    /// bash's "popd".
    /// </summary>
    public static void Pop()
    {
        if (s_directoryStack.Count > 0)
        {
            string previousDirectory = s_directoryStack.Pop();
            Directory.SetCurrentDirectory(previousDirectory);
        }
        else
        {
            // Stack is empty; nothing to do.
            return;
        }
    }
}
