// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Docker.UpdateDependencies;

/// <summary>
/// Helper class to manage a stack of directories for changing the current
/// working directory.
/// </summary>
internal static class DirectoryStack
{
    /// <summary>
    /// Pushes the specified <paramref name="path"/> onto the top of the
    /// directory stack and returns an <see cref="IDisposable"/> that
    /// restores the previous working directory when disposed.
    /// </summary>
    /// <remarks>
    /// This method is not safe to use in multi-threaded scenarios. It affects
    /// the current working directory of the entire process.
    /// </remarks>
    public static IDisposable Push(string path) => new DirectoryContext(path);

    /// <summary>
    /// Restores (pops) the previous working directory off of the stack when disposed.
    /// </summary>
    private sealed class DirectoryContext : IDisposable
    {
        private static readonly Stack<string> DirectoryStack = new();

        private bool _disposed;

        public DirectoryContext(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path);
            Push(path);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Pop();
                _disposed = true;
            }
        }

        /// <summary>
        /// Changes the current working directory to <paramref name="path"/>, and
        /// pushes the previous working directory onto the stack. Equivalent to
        /// bash's "pushd".
        /// </summary>
        private static void Push(string path)
        {
            DirectoryStack.Push(Directory.GetCurrentDirectory());
            Directory.SetCurrentDirectory(path);
        }

        /// <summary>
        /// Restores the previous working directory by popping it from the stack.
        /// If no directory was previously pushed, nothing happens. Equivalent to
        /// bash's "popd".
        /// </summary>
        private static void Pop()
        {
            if (DirectoryStack.Count > 0)
            {
                string previousDirectory = DirectoryStack.Pop();
                Directory.SetCurrentDirectory(previousDirectory);
            }
            else
            {
                // Stack is empty; nothing to do.
                return;
            }
        }
    }
}
