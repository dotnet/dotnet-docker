ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:10.0.0-preview.5-alpine3.22-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.5.25280.105 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.5.25280.105


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='e22085078de9b7877c4ce44790730cc12b169abdcc7982c13e85b15bcac56913a0cec3a739c4dcfe9c9eba65b3f7e4f5b6409a9865feaec2838222d334ec0fc4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
