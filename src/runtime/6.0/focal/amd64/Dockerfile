ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21373.17/dotnet-runtime-6.0.0-preview.7.21373.17-linux-x64.tar.gz \
    && dotnet_sha512='032ba4b959e7117bcc56b26cd5c9183d7a3b294fc4d735d5b3dd266ae09a64890031c446c05a8859866da54a02f424ef8c4693734f448f7d77571bb898f1a961' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-amd64

ENV DOTNET_VERSION=6.0.0-preview.7.21373.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
