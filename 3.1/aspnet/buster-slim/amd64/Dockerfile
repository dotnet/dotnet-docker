ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19503.31

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='3c290c30ba25c3feafef14ad94b184a14ac50c0c915be7921f7fee2eb89db60947939cc8dba432132a1e6315fec3f7274a70f7561125d25676bc6d7ade4c4150' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
