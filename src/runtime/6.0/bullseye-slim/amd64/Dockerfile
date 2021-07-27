ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21376.17/dotnet-runtime-6.0.0-preview.7.21376.17-linux-x64.tar.gz \
    && dotnet_sha512='ac7f29bfddb35da9f02f766e1f850c55e916ac8c4d6296a8a72306605b0bd7f118046605f5948aa8013adbb8d0f4ffbb43a2890d3bcd194f76c7b334bb9c227b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-preview.7.21376.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
