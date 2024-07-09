ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.32 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='1849c0073f12477b94357a1afb1cbd4ad67764263528b66037c19d554df41e681e4b41c0804b106319fe661d0bc3bae9e29e4913c0d0df33861cf6f32ebaac96' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.32-bookworm-slim-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.32

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
