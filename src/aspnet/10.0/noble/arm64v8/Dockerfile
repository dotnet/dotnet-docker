ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.1.25107.4 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='0d1e3b4ddaa5026e60dc02bb0b78f245a706b285cdbc9d27224008cb94c7774e8f418ee83d61fb4155db2bbc50fe29516d40fb62c5ee2115b25bde5b6cccda36' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.1-noble-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25107.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
