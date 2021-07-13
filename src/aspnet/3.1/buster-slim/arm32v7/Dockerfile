ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.17 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='0388c9e9499bde6e36a75cc160c8f1de025ed8a998dadf8c8f6bb3b5db8399b567b2ca5baa8eba09944d953e44ee725962304688fdb933856b6c3ec6a8d64548' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
