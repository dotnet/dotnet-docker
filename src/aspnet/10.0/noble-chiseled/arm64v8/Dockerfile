ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25164.1 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='2161e53f30413f27be9a81a7aedace12ad523d9e68b35d6a051e417701d238b1353569e227845aa76a9c889b7ecd1b5e7bafd3a678e2bf29438dae540e970d0a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25164.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
