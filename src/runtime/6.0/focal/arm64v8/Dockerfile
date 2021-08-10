ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21377.19/dotnet-runtime-6.0.0-preview.7.21377.19-linux-arm64.tar.gz \
    && dotnet_sha512='6c084b0dcd8b56cbca947adf93fe715c8531e39bb081c3b2ede1301a6177effe2e6db7d9746d9f3c92f9d878bed8f9dad0ffae92ab3359afa6c8935d03b95958' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-preview.7.21377.19

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
