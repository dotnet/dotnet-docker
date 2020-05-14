ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.11-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.4 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='dcb695a944cf75c33ca34bdc4e1d5dd78ee0239bfb4054a7123061ca7ca4e2f2dde9039db8a5097bbab723bcf542d7d8aca66abd22bbcde721e00e5f43baa377' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
