ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=5.0.0-rc.1.20451.2

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='80279a98df6eefd1cdbfed2d6d7b0fb03c05f53a4a278af6141b6418162c4ff1f8a03924060667a7d612005164eeb8561105220a5a10e2e894a5bae77658157b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-focal
ARG ASPNET_VERSION

ENV ASPNET_VERSION $ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
