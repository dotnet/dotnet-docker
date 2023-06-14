ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.18 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='16a1834d46cd8208221fa7d7f1b1e57a6544e855915e58e859b34bd7af2bc0dff1e94726356cee5302f1baf5a5bf6f25c0401cc8f904e13ca38a1175e406be73' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.18-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.18

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
