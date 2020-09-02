ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
ARG DOTNET_VERSION=5.0.0-rc.1.20431.14

# Installer image
FROM amd64/buildpack-deps:buster-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='c01705656d468fdf38663d81dfa45fbd8b5482f83e4f9e7aaf20b685dfa30e6b7aaa558b4d15950aa9899f999595a97fe8ddc178225c6e1b4cb74db8a08cedb9' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0-buster-slim
ARG DOTNET_VERSION

ENV DOTNET_VERSION $DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
