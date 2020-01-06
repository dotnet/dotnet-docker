// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public abstract class ProductImageTests
    {
        protected ProductImageTests(ITestOutputHelper outputHelper)
        {
            DockerHelper = new DockerHelper(outputHelper);
            OutputHelper = outputHelper;
        }
        
        protected DockerHelper DockerHelper { get; }
        protected ITestOutputHelper OutputHelper { get; }
        protected abstract DotNetImageType ImageType { get; }

        protected void VerifyCommonInsecureFiles(ProductImageData imageData)
        {
            if (imageData.Version < new Version("3.1") ||
                (imageData.OS.Contains("alpine") && imageData.IsArm))
            {
                return;
            }

            string worldWritableDirectoriesWithoutStickyBitCmd = @"find / -xdev -type d \( -perm -0002 -a ! -perm -1000 \)";
            string worldWritableFilesCmd = "find / -xdev -type f -perm -o+w";
            string noUserOrGroupFilesCmd;
            if (imageData.OS.Contains("alpine"))
            {
                // BusyBox in Alpine doesn't support the more convenient -nouser and -nogroup options for the find command
                noUserOrGroupFilesCmd = @"find / -xdev -exec stat -c %U-%n {} \+ | { grep ^UNKNOWN || true; }";
            }
            else
            {
                noUserOrGroupFilesCmd = @"find / -xdev \( -nouser -o -nogroup \)";
            }

            string command = $"/bin/sh -c \"{worldWritableDirectoriesWithoutStickyBitCmd} && {worldWritableFilesCmd} && {noUserOrGroupFilesCmd}\"";

            string output = DockerHelper.Run(
                    image: imageData.GetImage(ImageType, DockerHelper),
                    name: imageData.GetIdentifier($"InsecureFiles-{ImageType}"),
                    command: command
                );

            Assert.Empty(output);
        }

        protected static IEnumerable<EnvironmentVariableInfo> GetCommonEnvironmentVariables()
        {
            yield return new EnvironmentVariableInfo("DOTNET_RUNNING_IN_CONTAINER", "true");
        }
    }
}
