ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.2 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='5576751ea9414449add100cb60c917f72305209dc831ecd3698b8ef6a69e2a802850148b33caf5a05d049be66b19c7404531ea6ecd643cc1fd6ae61179e1249b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.2-bookworm-slim-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
