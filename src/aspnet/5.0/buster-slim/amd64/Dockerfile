ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.11/aspnetcore-runtime-5.0.11-linux-x64.tar.gz \
    && aspnetcore_sha512='2843b60a599fcb269bf700ea6233653264529d6522808c1b6ae0dc119e52e2c3203f3050dfef441dcb44130031a9ae03adbf25f1a51605ddc6f696b91433a667' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.11-buster-slim-amd64

ENV ASPNET_VERSION=5.0.11

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
