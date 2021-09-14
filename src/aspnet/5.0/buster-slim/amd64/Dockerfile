ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.10/aspnetcore-runtime-5.0.10-linux-x64.tar.gz \
    && aspnetcore_sha512='60fd41e42e07a96416baf2dd7ea8112a7c7d510bc6f4656274981777d2cf5e824cd519924cdf06215338d74e78cdc7801e1b24c39b3d67cd2d1b3c6fee6474a9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.10-buster-slim-amd64

ENV ASPNET_VERSION=5.0.10

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
