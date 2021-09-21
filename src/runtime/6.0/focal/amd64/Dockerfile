ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.2.21470.23/dotnet-runtime-6.0.0-rc.2.21470.23-linux-x64.tar.gz \
    && dotnet_sha512='0761f84b39af07cb8588e7138780bef7937f66adaed2c201c79d5544df7bb7111cc1324ea0a0e747529420eb88cfe485dcd9b0a78862823e945233ec45e88ce9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.2-focal-amd64

ENV DOTNET_VERSION=6.0.0-rc.2.21470.23

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
