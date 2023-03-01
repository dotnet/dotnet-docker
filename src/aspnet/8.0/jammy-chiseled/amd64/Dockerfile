ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.3.23126.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='cc89e9021d5e8e417233dbbccf4c68b56190a821a70f9fec2414e980c89c0189e954e80f63a7f4353da1c3f9031a954fdbd99b48cf0d33939db643145de008c3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.2-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.3.23126.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
