ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-alpha.1-alpine3.15-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22062.12 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='51ba6a55083475b914169bd39f2de78f8f7281181ba8ea6b95bc4cd6521b5e53612d3a762bfc220dec8b34bd1199e736228514d5dc0596daf96952e601d1710d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
