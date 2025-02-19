ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.1-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.1.25107.4 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='91be6d98fc3216a346ed043bc69178446fc8da401fdcdc08c54baf7585b1f4272f646241e030b245db32bb433dfc3a1b4e68124d5df5aab3338f26b0a77b02eb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.1-alpine3.21-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25107.4

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
