ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.3-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.3

# Install .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='f0f9fb191054dea2e4151a86c3de1a11ce574cc843cde429850db0996c7df403dfa348a277f1af96f13fec718ae77f3be75379ed3829b027e561564ff22c7258' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
