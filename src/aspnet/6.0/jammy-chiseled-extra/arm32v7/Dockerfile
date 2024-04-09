ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.29 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='82efa016bf977649c907bfcf192684a4bbd9bc1f5a0b3f46be8aa7aea4965b570c72552a2617926ccd0624a74f9aa04ab2298aba8ebe4594b85f3b36d4ec7eec' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.29-jammy-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.29

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
