ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.10/dotnet-runtime-5.0.10-linux-arm64.tar.gz \
    && dotnet_sha512='30861f2bd41fcd7c1d657be1eafa09f22886af0c3e09d1854c626b675a469347ce9fb79b2ea92b5ed4e2fd3d3457766ac03fc5393a690099e1d2b9b15f3334b9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.10-focal-arm64v8

ENV DOTNET_VERSION=5.0.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
