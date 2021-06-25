ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.6.21324.6 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='29fb43ea99570bd36f0d378c9d4f06063a1eb4be95bd1ed226f23b0dab7a1a92259e063b8c256e6f31c34a66736828a8a2ebefcf8390cb1b10e3b1e637c13594' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
