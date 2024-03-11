ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.0-preview.2-alpine3.19-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-preview.2.24128.5

# Install .NET Runtime
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='3384c37ae4dd0b0f9eda8a4b7bbdc24ab8fd82a9fba9977408b93ef2a49c4aeca7808faf6c28fa970cd07959c6883045cd0408d3c96f52d1bfe9282dd6cf06eb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
