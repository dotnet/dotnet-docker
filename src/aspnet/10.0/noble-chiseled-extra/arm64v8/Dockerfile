ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-alpha.2.25073.4 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='24b2d6baa656845b0e8de3cf7b0910efcf9281c2148f7664992b1fb47eb76bab660caaa112d3d03c0badb75a628f0867b94d5811bbab8e772f1ab1231d711f7b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-alpha.1-noble-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-alpha.2.25073.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
