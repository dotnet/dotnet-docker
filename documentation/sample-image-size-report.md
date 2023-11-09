# .NET 8.0 Container image size report

.NET offers a variety of deployment options for applications, which pair with container images that we offer. It's possible to produce very small container images. This document summarizes the available options to help you make the best choice for your apps and environment.

All images were produced from the ["releasesapi" sample](https://github.com/dotnet/dotnet-docker/tree/main/samples/releasesapi) using Ubuntu 22.04 ("jammy") images for amd64. Alpine images will be similar. The Baseline image is a standard [framework-dependent](https://learn.microsoft.com/en-us/dotnet/core/deploying/#publish-framework-dependent) deployment on the ASP.NET runtime image.
This is the largest image with the most functionality and flexibility.
However, the new [Ubuntu Chiseled](https://ubuntu.com/engage/chiselled-ubuntu-images-for-containers) .NET base images can provide significantly smaller and more secure deployments for your application as demonstrated below.

### Framework-dependent deployment

| Image Kind | Base Image | Uncompressed Image  | Compressed Image | % Size savings over Baseline[^1] |
| --- | --- |--- | --- | --- |
| Baseline | `aspnet:8.0-jammy`| 217 MB | 90.9 MB | 0% |
| Distroless | `aspnet:8.0-jammy-chiseled`| 111 MB | 49.3 MB | 46% |
| ASP.NET Composite Runtime | `aspnet:8.0-jammy-chiseled-composite`| 103 MB | 40.8 MB | 55% |

### Self-contained + Trimming deployment

[Self-contained](https://learn.microsoft.com/en-us/dotnet/core/deploying/#publish-self-contained) deployments bundle the .NET Runtime with your app so that it's able to run without the full .NET Runtime installed.
[IL Trimming](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained) for self-contained apps removes unused code from the .NET Runtime and libraries to reduce application size.
And [Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) deployment produces an app that is completely compiled to native code at build time for the smallest deployment size that .NET allows for.

| Image Kind | Base Image | Image Size on Disk | Compressed Image Size | % Size savings over Baseline |
| --- | --- |--- | --- | --- |
| Self-contained + Trimming | `runtime-deps:8.0-jammy` | 146 MB | 57.9 MB | 36% |
| Self-contained + Trimming + Distroless | `runtime-deps:8.0-jammy-chiseled`| 39.3 MB | 16.4 MB | 82% |
| Native AOT | `runtime-deps:8.0-jammy-chiseled`| 27.7 MB | 12.4 MB | 86% |
| Native AOT (preview image)[^2] | `runtime-deps:8.0-jammy-chiseled-aot`| 25.4 MB | 11.6 MB | 87% |

## Notes

Please note that the recorded image sizes are a snapshot of deployment sizes at .NET 8.0 GA, and that image sizes will vary due to base image updates and updated package installations. This document will not be updated over time. The key takeaway is the size *difference* between the different models.

For more information about new images for .NET 8, please see the [.NET image variants documentation (temporary link to PR)](https://github.com/dotnet/dotnet-docker/pull/4979) and ["Announcement: New approach for differentiating .NET 8+ images"](https://github.com/dotnet/dotnet-docker/discussions/4821).

[^1]: Percentage of size savings is based on compressed image size.

[^2]: Native AOT is fully supported for .NET 8, but our AOT-specific container images are in preview and thus only available in the `mcr.microsoft.com/dotnet/nightly/` repos. Please try them out!
