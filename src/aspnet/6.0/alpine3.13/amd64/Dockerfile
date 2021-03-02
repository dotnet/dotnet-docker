ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.2.21125.25

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='01b937a32a8dd6bb701c11f3f361039b94b7fb09902d254ccc0f30b943bbeb7b3547ebf736c771e3db048e02d46b7f91db6e0f2cbc2604ccaf5f9496b1ff16de' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
