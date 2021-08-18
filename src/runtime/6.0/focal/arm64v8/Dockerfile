ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21417.1/dotnet-runtime-6.0.0-rc.1.21417.1-linux-arm64.tar.gz \
    && dotnet_sha512='567ae2b70281705d647a63f2caea160d868d7f587e3db44e1ad1b31a5300efbfd8d7be3853ea84deb8f2c6cdc329fa914d3c9762a522f15345cb4696e4fa6d8f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21417.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
