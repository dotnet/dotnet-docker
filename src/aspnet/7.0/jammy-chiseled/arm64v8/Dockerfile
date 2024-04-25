ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.18 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='62bc42a8d094be8253be90acba02253c27afb1eb6b06976c3adea74f967f522bf7938eaed25c824d6e36a9fb71f7248ba315cc67577a3c7fb73b0d3d7a41ecac' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.18-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.18

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
