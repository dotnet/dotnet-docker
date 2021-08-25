ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21424.15/dotnet-runtime-6.0.0-rc.1.21424.15-linux-x64.tar.gz \
    && dotnet_sha512='bef3ba1e000a34d79a623b5a0af166bfd615456739d374355db93258b30e9379089edc761b17d2fa740913a20280b66836c315b9372acbde97deb717e11c35ff' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21424.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
