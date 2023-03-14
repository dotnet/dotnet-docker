ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.4 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='5e149ccdd003dfc4413927f23b07b2f27ce915c63146e514b2f88446bd44f64df51755644b56c316b0a1388c873404fc1d39b24a0bf8066f09fc37d6f32cc03f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.4-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
