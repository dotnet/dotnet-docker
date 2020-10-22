ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-alpine3.12-arm64v8

# Install .NET
ENV DOTNET_VERSION 5.0.0-rtm.20514.6

RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-5.0.0-linux-musl-arm64.tar.gz \
    && dotnet_sha512='6e58a48d18a3840133dd30fe2dab60d6e9b517e50ca161bbecb49999afff47be47894545b7275f405678f04561dd16d25a7f470c4cb47c018921711a4655a6cb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
