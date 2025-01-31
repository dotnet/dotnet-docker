ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:10.0.0-preview.1-alpine3.21-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.1.25078.5 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.1.25079.4


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='0410566665fba66a43a876ea2d740a41de5c0cf848f150c64d0518f8c4e94b9c422d31f5258d6da9003780c42d76e59a5aaf385d4636172ea49a905b65060829' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
