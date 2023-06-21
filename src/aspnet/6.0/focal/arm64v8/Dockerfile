ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.19 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='739acf3966e32092243f9a2ede241a16b2ae7c26bccdbcd53e63149429c4fad7d7166beca8c54ef1e79f18b43d3a5334bab42128a534aae7499cd137b833e888' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.19-focal-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.19

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
