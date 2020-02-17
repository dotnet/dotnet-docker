ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:buster-curl as installer

# Retrieve .NET Core
RUN dotnet_version=5.0.0-preview.1.20115.9 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='6d47c60ae9a3e88d31fe17e209ee1b69c6d867f5ca2ee9940a6ecf29032a71bd0429b6935249f17f630c16ae9285be8c6d9f65246c287267174d5f908b2de836' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-buster-slim-arm32v7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
