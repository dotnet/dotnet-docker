# Guiding Principles

These are the guiding principles for the content, tagging and production of the official .NET Docker images. They establish a framework for the behavior customers can expect and the decision-making process when making changes to the images. They are not hard rules which are stringently enforced as there will be occasions where it is sensible to make special exceptions.

## Image Content

1. Images are intended to satisfy the common usage scenarios. They are not intended to satisfy every possible usage scenario. As a result of this, decisions will be made (e.g. components excluded, configurations made, etc.) in order to keep the image size manageable.

    It is expected there will be scenarios in which customers will need to create derived images that add the required components/settings. A few examples include:

    1. Alpine images have the .NET Invariant Mode enabled by default ([details](https://github.com/dotnet/dotnet-docker/issues/371)). Use cases that cannot tolerate the globalization invariant mode can reset the `DOTNET_SYSTEM_GLOBALIZATION_INVARIANT` environment variable.
    1. The System.Drawing.Common native dependencies are not included by default. If System.Drawing.Common is being used, the native dependencies will need to be added.

    Component authors can package their bits in ways which make them more easily consumable from Docker without including them within the .NET images. Examples include:

    1. Support xcopy installs
    1. Package as a .NET global tool
    1. Ship within their own Docker image
        1. Support standalone execution and multi-stage builds
        1. Support running within a side car image

1. Components installed within the images are required to have the same or longer support lifecycle as [.NET](https://dotnet.microsoft.com/platform/support/policy/dotnet-core). For example, if a component is included with an LTSC release of .NET, then that `major.minor` component version will need to be supported for the life of the LTSC release. Components are expected to be serviced over their lifetime as appropriate.

1. Breaking changes are not allowed within a release. This includes changes such as adding/removing components, adding/removing ENVs, and major/minor version changes to included components.

1. There should be parity within the supported image matrix. Examples include:
    1. If support for a new distro is added for a particular .NET version, then new `runtime-deps`, `runtime`, `aspnet`, and `sdk` images should all be added for the new distro.
    1. If a new component is added, it should be available across all supported OS types and architectures.

1. The matrix of [supported container operating systems and versions](supported-platforms.md) will evolve on a continuous basis.

## Image Tagging

See the [supported tags](supported-tags.md) for the tagging practices and policies used by the .NET team.

## Engineering

1. Images will be included as part of the .NET release process. The Docker images will be released at the same time as the core product.

1. Images will never be deleted from the [official Docker repositories](https://hub.docker.com/_/microsoft-dotnet/). This does not apply to the [nightly repositories](https://github.com/dotnet/dotnet-docker/blob/nightly/README.md).

1. The [Dockerfiles](https://github.com/dotnet/dotnet-docker/search?q=filename%3ADockerfile) used to produce all of the images will be publicly available. Customers will be able to take the Dockerfiles and build them to produce their own equivalent images. No special build steps or permissions should be needed to build the Dockerfiles.

1. No experimental Docker features will be utilized within the infrastructure used to produce the images. Utilizing experimental features can negatively affect the reliability of image production and introduces a risk to the integrity of the resulting artifacts.

## Image Rebuilds

Since .NET container images are used as base images, excessive image rebuilds can cause immense usage of compute and network for our customers. We strive to rebuild our images only when absolutely necessary.

Images will be rebuilt within hours of base image changes. For example, suppose a particular version of Alpine is patched. The .NET images based on this version of Alpine will be rebuilt with this new base image within hours of its release.

An image will be rebuilt to pick up fixes for CVEs when:

- We detect the image contains a CVE with a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score of "High" or "Critical".
- OR when we detect the image contains a CVE that has not been assigned a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score for more than two weeks since the CVE was disclosed.
- OR when we detect the image contains a CVE with a [CVSS](https://nvd.nist.gov/vuln-metrics/cvss) score of "None", "Low", or "Medium" and it has been more than two weeks since the CVE was disclosed.
    - This is to give ample opportunity for distro maintainers to re-build their base images with package updates so that we avoid repeated re-builds of our images.
- AND The CVE is in a package that is a direct dependency of .NET or is in a package that we explicitly install in our Dockerfiles.
- AND There is a fix for the package affected by the CVE available in the affected base image's package feed.
