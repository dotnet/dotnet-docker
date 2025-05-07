ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:10.0.0-preview.3-alpine3.21-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.4.25255.103 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.4.25255.103


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='f5d5a34912277a806029d697d9aa1d227212c22e081e39911a2fe16b429308e3af15ca0727b955751c03a27d507568d9981c340218bb95a5b164d37b527c98fb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
