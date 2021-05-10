ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-focal-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.15 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='574db7a64e6afe6e55dbc4f95b5d87bfde9cec973e9501f8b8ce6a11383edc97b600a3e926cda53a3711d2d7bc195dbe5d77ecb954c8d09a6b332b45c07e6512' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
