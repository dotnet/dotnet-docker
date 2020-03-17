ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.11-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.3.20156.6 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='96ac4bed3b0172de079d55ac462fd7c1b5807c2499a1ca6b38e516dbe1875a45c742a933394a25c9a97f3c14185abdcee568a36d4530f5a0f8201f27cd06e85f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
