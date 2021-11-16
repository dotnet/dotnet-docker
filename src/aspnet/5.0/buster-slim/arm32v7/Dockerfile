ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.12/aspnetcore-runtime-5.0.12-linux-arm.tar.gz \
    && aspnetcore_sha512='7bb48192b97569fc198a03deb9330f32ff7ba14a4d903a31fa66e184df1055ed7419f7975ad9104582ee4edb767064437ab04117128a80cb4f310d5a22193b32' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.12-buster-slim-arm32v7

ENV ASPNET_VERSION=5.0.12

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
