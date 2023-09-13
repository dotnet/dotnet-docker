ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-rc.1.23421.29 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='ba8035da535cb3bffa720e962e6f9e0f88b36e1221b588f2a126ee4b43c02e4d8c27958017d29e5ab68121fab6a564fe0a27099c4103ee3d527f8554b4ab495e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-rc.1-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-rc.1.23421.29

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
