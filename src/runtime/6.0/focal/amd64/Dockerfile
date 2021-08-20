ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21419.17/dotnet-runtime-6.0.0-rc.1.21419.17-linux-x64.tar.gz \
    && dotnet_sha512='6e814d5897e7657c5863a25b910087bc1dc059fc672e46390072a8871acad6f411fc42601896a0f2ad18c2be42c71411238948804de2e8ebb7ade4fd2c4d9e3f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21419.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
