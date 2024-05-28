ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.5 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='ffe6a534ed7dffe031e7d687b153f09a743792fad6ddcdf70fcbdbe4564462d5db71a8c9eb52036b817192386ef6a8fc574d995e0cdf572226302e797a6581c4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.5-noble-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
