ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.17-alpine3.22-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.17 \
    && wget --output-document aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='e9ec83100659502c611499f924bfc474c2e6491b518ed69739919c097a799bee7b4a4dd23edb1f0e7868fbce9198e4b1d16545ec5144a55b9820c1665b0dcf87' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.17-alpine3.22-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
