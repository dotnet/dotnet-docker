ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-bionic-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview4-19207-02

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='414dacfc3016a7ee464cb3066e116c1cf57b3fb672b7cea2ba9e16d0c957d3f0c2997b8060e787ca76be47a6319a497fa4fdaee5f320c3433b120e5f532d08ff' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
