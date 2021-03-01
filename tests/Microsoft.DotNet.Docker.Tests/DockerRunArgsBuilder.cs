// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Text;

namespace Microsoft.DotNet.Docker.Tests
{
    internal class DockerRunArgsBuilder
    {
        private readonly StringBuilder _builder =
            new StringBuilder();

        private DockerRunArgsBuilder()
        {
        }

        public static DockerRunArgsBuilder Create()
        {
            return new DockerRunArgsBuilder();
        }

        public string Build()
        {
            return _builder.ToString();
        }

        /// <summary>
        /// Sets the named environment variable with the specified value.
        /// </summary>
        public DockerRunArgsBuilder EnvironmentVariable(string name, string value)
        {
            _builder.AppendFormat(CultureInfo.InvariantCulture, "-e {0}={1} ", name, value);
            return this;
        }

        /// <summary>
        /// Exposes the container port to an arbitrary port on the host.
        /// </summary>
        public DockerRunArgsBuilder ExposePort(int port)
        {
            _builder.AppendFormat(CultureInfo.InvariantCulture, "-p {0} ", port);
            return this;
        }

        /// <summary>
        /// Mounts a volume in the container at the specified path.
        /// </summary>
        public DockerRunArgsBuilder VolumeMount(string name, string path)
        {
            _builder.AppendFormat(CultureInfo.InvariantCulture, "-v {0}:{1} ", name, path);
            return this;
        }
    }
}
