ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.2.24462.5 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='90b84deed76313229210fef6d7462d6d4f56a06cb730e9227ab1f23745b6be5330333cef722d5aa7df9711d107fa95c8debbfa6dc5cda62c2ecf87add96d43ba' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.2-noble-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.2.24462.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
