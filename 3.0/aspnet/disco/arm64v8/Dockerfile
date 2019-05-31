ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview6-19279-08

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='ba5a365ce3b2520ec2f7cf549ac1993efb3be6199c939c97785fb230743cc617e3958985e7e5554e5760988010887877f3a9f6924062a7f44893f09ed404b32c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
