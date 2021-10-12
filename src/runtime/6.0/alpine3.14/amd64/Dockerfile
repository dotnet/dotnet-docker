ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.2-alpine3.14-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.2.21480.5

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='ee3fb0c0311e0d88ae3f8b0d150f8d98da4fa24d77c429438fad04393b24e214db49bfbc4c9e89918e99061004d63f40ec7b76cb1c0e1c2b12414be29ab63238' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
