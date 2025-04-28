ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.4.25227.102 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.4.25227.102


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='9c530bad67ad8c2e976042e897bf6e5e07b83fd92b4220e8cdc31d657f40888249639c31a01e5bb4663b1458899435d0c2c1e6b41cf200183fc96bcecb4c6268' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
