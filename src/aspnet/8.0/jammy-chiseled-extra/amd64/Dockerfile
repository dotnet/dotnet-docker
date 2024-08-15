ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.8 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='d6c0cc2aac79fbacbf81b597f286763599f66278c17ddb448ce0b93d499bad8f88777d425854e68602945ab18af8a61f1ee59d431d5503006137f86113faa8b2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.8-jammy-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
