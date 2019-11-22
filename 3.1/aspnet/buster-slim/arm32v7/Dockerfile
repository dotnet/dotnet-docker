ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.0 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='d8ac48b37b5d239e08f68c9f56c7f8d8c3ef2db3b7f55aaa123c990bf19a4b4546d71aded53ce266ea77095a0b6bcc319a9e3c1a5be418a09ece501e59538677' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
