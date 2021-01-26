ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=6.0.0-alpha.1.21072.5

# Installer image
FROM amd64/buildpack-deps:buster-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='a3b91ae8cbaea388469e65df99e09aabc7b95b559254043ebb1d646ef08b2f722813db043c996b27e1cc7b50b1ea6a1bb84dd8a4b814620b9d7ccbbd55d07e8f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0-buster-slim-amd64
ARG ASPNET_VERSION

ENV ASPNET_VERSION=$ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
