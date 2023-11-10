ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.13 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='f60ddaa4e4523f991196e334a23d5155c0967053a99406c88bf423f1e5f3e6242edc5168cc51fb9e24da3fcb5406f4d38dc8d30073bd5faed42ddac9ed06b045' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.13-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
