# .NET 8.0 Container image size report

.NET offers a variety of deployment options for applications, which pair with container images that we offer. It's possible to produce very small container images. This document summarizes the available options to help you make the best choice for your apps and environment.

All images were produced from the ["releasesapi" sample](https://github.com/dotnet/dotnet-docker/tree/main/samples/releasesapi) using Ubuntu 22.04 ("jammy") images for amd64. Alpine images will be similar.

### Framework-dependent deployment

| Image kind | Base Image | Uncompressed Image  | Compressed Image | Ratio |
| --- | --- |--- | --- | --- |
| Baseline | `aspnet:8.0-jammy`| 217 MB | 90.9 MB | 1.0 |
| Distroless | `aspnet:8.0-jammy-chiseled`| 111 MB | 49.3 MB | 0.54 |
| ASP.NET Composite Runtime | `aspnet:8.0-jammy-chiseled-composite`| 103 MB | 40.8 MB | 0.45 |

Notes:

- Baseline uses the largest image with the most functionality.
- Ratio is based on compressed image size.

### Self-contained + Trimming deployment

[Self-contained](https://learn.microsoft.com/en-us/dotnet/core/deploying/#publish-self-contained) deployments bundle all application and .NET Runtime code into a single file that is able to run on images without the .NET Runtime installed.
[IL Trimming](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained) removes unused code from the .NET Runtime and libraries to reduce application size.
And [Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) deployment produces an app that is completely compiled to native code at build time for the smallest deployment size that .NET allows for.

| Deployment model | Base Image | Image Size on Disk | Compressed Image Size |
| --- | --- |--- | --- |
| Full | `runtime-deps:8.0-jammy` | 146 MB | 57.9 MB |
| Distroless | `runtime-deps:8.0-jammy-chiseled`| 39.3 MB | 16.4 MB |
| Native AOT | `runtime-deps:8.0-jammy-chiseled`| foo | bar |
| Native AOT (preview image) | `runtime-deps:8.0-jammy-chiseled-aot`| 25.4 MB | 11.6 MB |

**Note**: Native AOT is fully supported for .NET 8, but our AOT-specific container images are in preview and thus only available in the `mcr.microsoft.com/dotnet/nightly/` repos. Please try them out!

## Notes

Please note that the record image sizes are a snapshot of deployment sizes at .NET 8.0 GA, and that image sizes will vary due to base image updates and updated package installations. This document will not be updated over time. The key takeaway is the size *difference* between the different models.

For more information about new images for .NET 8, please see the [.NET image variants documentation (temporary link to PR)](https://github.com/dotnet/dotnet-docker/pull/4979) and ["Announcement: New approach for differentiating .NET 8+ images"](https://github.com/dotnet/dotnet-docker/discussions/4821).
