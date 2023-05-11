// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

public abstract class CommonAspnetImageTests : CommonRuntimeImageTests
{
    protected CommonAspnetImageTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    public abstract Task VerifyAppScenario(ProductImageData imageData);
    public abstract void VerifyEnvironmentVariables(ProductImageData imageData);
    public abstract void VerifyPackageInstallation(ProductImageData imageData);

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyInsecureFiles(ProductImageData imageData)
    {
        base.VerifyCommonInsecureFiles(imageData);
    }

    [LinuxImageTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyShellNotInstalledForDistroless(ProductImageData imageData)
    {
        base.VerifyCommonShellNotInstalledForDistroless(imageData);
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyNoSasToken(ProductImageData imageData)
    {
        base.VerifyCommonNoSasToken(imageData);
    }

    [DotNetTheory]
    [MemberData(nameof(GetImageData))]
    public void VerifyDefaultUser(ProductImageData imageData)
    {
        base.VerifyCommonDefaultUser(imageData);
    }
}
