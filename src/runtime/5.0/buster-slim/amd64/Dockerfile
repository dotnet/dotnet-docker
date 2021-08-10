ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:buster-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.9/dotnet-runtime-5.0.9-linux-x64.tar.gz \
    && dotnet_sha512='454a11d87199be6fcd5f97f28b0f9c2f15c0288dd0759dc96ac2c730697b9e6514a19bbfc06185ccb4289dd11681ea5e9cfaf6f3676040e36d21e9b90282af8d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.9-buster-slim-amd64

ENV DOTNET_VERSION=5.0.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
