ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21513.22/dotnet-runtime-6.0.0-rtm.21513.22-linux-x64.tar.gz \
    && dotnet_sha512='2bc6d2b7010b455eae1b08e181aa65ce835694b48c71bd4dbd74cba44147b465fa67db8bc1faf320303034425437f14c38b365bf09a04a5a5fcb7a458f4e6029' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-focal-amd64

ENV DOTNET_VERSION=6.0.0-rtm.21513.22

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
