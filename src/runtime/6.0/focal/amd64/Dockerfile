ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21377.12/dotnet-runtime-6.0.0-preview.7.21377.12-linux-x64.tar.gz \
    && dotnet_sha512='c3b1d104791e63ac879287a312ecadfad30cfac25990521caeba4ad38aab5138561aeae17cd750d78b3223d960f80ee5ecf181cf738349c27d49611da004e4e1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-amd64

ENV DOTNET_VERSION=6.0.0-preview.7.21377.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
