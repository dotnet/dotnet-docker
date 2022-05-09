ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.17-alpine3.15-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=5.0.17

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='ce5a53f465d91d7c1d0fb29bca2e17aeadd31b493dc94233f3807d41df939e739f6339a94719561ec40d2cfcbafba01e64e125b91ecd7c32afc9cebb4a3bcc29' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
