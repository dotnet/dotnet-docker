ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.2.24462.5 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='ebaa727c22a6a83a8192d640f1eb5694d89d9dd3a74a080f94d2db42a1cb457f5f801a78622805c94b64701a0d342cba0400208e9666f2cfcb966d0c992fcd99' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.2-noble-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.2.24462.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
