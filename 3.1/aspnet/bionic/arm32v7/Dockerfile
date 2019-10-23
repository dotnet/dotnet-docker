ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview2.19522.3

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='9320f4eb15e3d0b6975dc68bea7773cca48adf1699c0ae931dfebb9bc4a877b73ff763fa943aff727083ae3ba9443ffedfb80ee3a18058bcaf3d5d5aad3afe49' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
