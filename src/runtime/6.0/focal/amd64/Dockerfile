ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.6.21352.12/dotnet-runtime-6.0.0-preview.6.21352.12-linux-x64.tar.gz \
    && dotnet_sha512='b060e5c1d568cadfd813d8b1dd3b378fa7465e47829c4b6831eeb6af9760db456a60da54c99a705b68269ddb8287def6119814514d24a38ed2f0d08464381211' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.6-focal-amd64

ENV DOTNET_VERSION=6.0.0-preview.6.21352.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
