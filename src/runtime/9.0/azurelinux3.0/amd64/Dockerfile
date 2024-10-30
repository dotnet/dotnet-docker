ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.0-azurelinux3.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-rtm.24516.10 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-9.0.0-linux-x64.tar.gz \
    && dotnet_sha512='f9a72c71551f52e49146790a4bc149628cf26e3efbe95cd3844ecf73259da4cc812a7b0a7692d178c576924a6982b4ccb95abc218dd38e5b674af8e7e871ef3a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-azurelinux3.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-rtm.24516.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
