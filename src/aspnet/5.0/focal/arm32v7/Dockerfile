ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.10/aspnetcore-runtime-5.0.10-linux-arm.tar.gz \
    && aspnetcore_sha512='8227fce474bb349e2897a16a8dd7ac935c55b12881a0fae9806c161a6ff59acba14a475f02795838a8b516d639bc1bc2d90a7492ebbfa86f81ae5386c60def05' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.10-focal-arm32v7

ENV ASPNET_VERSION=5.0.10

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
