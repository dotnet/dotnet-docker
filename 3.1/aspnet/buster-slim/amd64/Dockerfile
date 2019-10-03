ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19502.13

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='b783295df67b23076d9c643f6089c7743180e3820559c13854d1e0bd3f1c6e6974ce0f031bf5e1a1b2e8287ab0ed0fe96b0106607df7b3411cbb45596e8ff1ef' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
