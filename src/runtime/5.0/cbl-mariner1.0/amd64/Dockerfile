ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.8-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=5.0.8

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='8789609f3039dca1d0dc19562f23bc9bfe5d513a2d10639a8a779afe7656447b7ee953f9a8d9d0b07ba6ca4a346770c0efb5a34e5240b5d355d4d8198220e9b1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
