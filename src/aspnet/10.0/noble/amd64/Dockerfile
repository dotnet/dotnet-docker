ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25161.17 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='e2fca2689ce7f09dea237a955ac2ab6d76ee6afb074e494fbd833f647e4642209e1f122f776e3d351b625213e88411e55a4f5ee85fbdeba454a127c0d5773f57' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-noble-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25161.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
