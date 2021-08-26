ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21425.15/dotnet-runtime-6.0.0-rc.1.21425.15-linux-x64.tar.gz \
    && dotnet_sha512='7996dc93a169514a804e74bb0348520a37b5a04bc3914acadbf10c0ce7e35da6ae36d386c238658640b88f27d8e178fe3c491ac2e63e17f06fdde86a367913c7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21425.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
