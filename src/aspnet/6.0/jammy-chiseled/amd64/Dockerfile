ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.15 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='db41bbd6ffb061402acee12f498f41fe5987d355c9004091ff63010303cc9ea969ab233986dc11556bc6def5194883f50fdf216e1c50b26bb60cacd4f2ecd98a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.15-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
