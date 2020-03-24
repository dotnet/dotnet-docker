ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-alpine3.10

# Install ASP.NET Core
RUN aspnetcore_version=3.1.3 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='0a2043df6f57c9936c0585cdcdb2c4e00f33dfa0e87f61e11e1aa4d5e34f425c9c4c48eb0928aeb9381dd890cfd9d767ee345f1b6d4dc9a87054ab63b14c0f08' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
