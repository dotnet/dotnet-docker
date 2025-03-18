ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25164.1 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='ceebf19085159175e60004dba88e4698237c0708df5dd63b225177c70c91df6be110a46e9ee693001e27933050d8637cfc8b739d1e510d09dca562031c866ccc' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-trixie-slim-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25164.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
