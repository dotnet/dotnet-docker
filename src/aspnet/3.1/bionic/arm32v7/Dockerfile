ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.27-bionic-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.27 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='4137f2e27a9c9d8c78fc9b8c416082588dcc2d8dec7d5b4d92c8a09cc31ad2b9972a0ceacfaf451bdcdf9f46cf279f9ef9bf4eb922d8ca539f3640907dd8d558' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
