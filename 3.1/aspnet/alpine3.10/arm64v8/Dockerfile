ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.10-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.1 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='34aa2507dd8f0be7f8fa3f92450a017d23a48ffcf16e67cd6f314b1145a48874e49b881928a497413e12cd21021af8c9c9955dda5f2b866f8c6e599fdd7d71aa' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
