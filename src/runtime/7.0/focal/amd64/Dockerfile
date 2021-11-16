ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.1.22076.8 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='8d48436646071c371b862002fcdd24de07addd02ec878ab413cc6c98a5ff6da821f2175c637167462261ad5e47da0d2b83f465949e673462f0e147d62255651c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-preview.1-focal-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.1.22076.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
