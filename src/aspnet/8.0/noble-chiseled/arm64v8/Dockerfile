ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.15 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='967d43a9387d226ed804cfee35144a69f249f6206b73ed0d8915dad358fede3c5ddc3ec963a5c35400b62dc57265da1dbc07d793cf5e3940ce94e54783312f0e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.15-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
