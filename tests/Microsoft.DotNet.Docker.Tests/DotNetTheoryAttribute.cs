// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.DotNet.Docker.Tests
{
    [XunitTestCaseDiscoverer("Microsoft.DotNet.Docker.Tests.DotNetTheoryDiscoverer", "Microsoft.DotNet.Docker.Tests")]
    [AttributeUsage(AttributeTargets.Method)]
    public class DotNetTheoryAttribute : TheoryAttribute
    {
    }
}
