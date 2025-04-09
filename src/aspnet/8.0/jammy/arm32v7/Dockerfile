ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.15 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='0995e596c1fd02f5e89a7f695b93976af253841ef30fe83a05310a3fcf4e069692c5620e80355dcd3fe6ae32e7b5adbae0bb3f0ff35e334c2f28f14c44dca6ef' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.15-jammy-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
