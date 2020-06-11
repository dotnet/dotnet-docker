ARG REPO=mcr.microsoft.com/dotnet/core/runtime
ARG ASPNET_VERSION=5.0.0-preview.6.20310.9

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='350a71c4f23484b8de5d39698ed51d7d05055a1af01a7493bcb8d7277c63dfce141b7c7aa97c837d17d1bad78578f7e4e01118b8317021780746bf8bc0829549' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-focal-arm32v7
ARG ASPNET_VERSION

ENV ASPNET_VERSION $ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
