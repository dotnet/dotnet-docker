ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=5.0.6

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='7aece6b763305fcf6e47e31540830797670287622ec424e689967c8974f80cefdb04961fc8cdf23c67588f3b0804b5e8291f87b06b10f2fc83d48ce0b9700d38' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
