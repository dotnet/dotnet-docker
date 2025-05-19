ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25267.102 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='934475c71e38d3598056fd791c2530e14d252546147518bf12ee4363e66b40d55077f88a470356c10ea8c5ff4792ce25977224555ef695964c9e2f80f5d373bd' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-noble-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25267.102

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
