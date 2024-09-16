ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.2.24463.6 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='ca4c8091d0da3761956019c65f6aaeb05cb777d473b70de6048b4a19312dbdca76373b93ead0bafa33d25e9d4b7895b48c49ac6430634952b7646eb72c6f0968' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.2-noble-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.2.24463.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
