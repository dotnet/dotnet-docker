ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22060.15

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2c154be536970a8447dc778c8f694b32c8bc4eb17dea014e5239789532ad1c2ca028d794f8096507fa9fb91faca98eb590746952066c4f1f4265dcb3eafcd88f' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='df4d43a6915b622376b54bcc1f186acf5fecd851f3f65874397079072c03a17b7463e417f313e53edfdc924355455930c4ba503c8818bb820f715843b9f5b491' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='69866409ad055aae1a387f24c0075a2ffb261597433233b8b7862cf594c393bcf625ddb31221846db7d41a352de169de0d64616810b53a47ebfafbef18206992' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
