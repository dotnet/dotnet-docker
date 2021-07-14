ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.8/dotnet-runtime-5.0.8-linux-x64.tar.gz \
    && dotnet_sha512='8789609f3039dca1d0dc19562f23bc9bfe5d513a2d10639a8a779afe7656447b7ee953f9a8d9d0b07ba6ca4a346770c0efb5a34e5240b5d355d4d8198220e9b1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.8-focal-amd64

ENV DOTNET_VERSION=5.0.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
