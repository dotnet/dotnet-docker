ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.6.24322.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='bbf3a20c47136d1b8969f5844dd204f89d11b726fbc88eb111cfd9b968f63e9f229a2b2367170082b58647abca048a026f63d7732233f7b9137205cbfba1fc81' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.6-noble-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.6.24322.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
