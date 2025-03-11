ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.3 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='8a027078b46b6ebb3f4beda2b3f3cf7960701b71b9f2b6704c17f2752279c764755cadfd30f3e2f3e3ab26869e50a35c567c2fbd41fd98d77ae2f6fecde18b50' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.3-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
