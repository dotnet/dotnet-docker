FROM buildpack-deps:trusty-scm

# install dotnet
RUN echo "deb [arch=amd64] http://llvm.org/apt/trusty/ llvm-toolchain-trusty-3.6 main" > /etc/apt/sources.list.d/llvm.list \
    && wget -O - http://llvm.org/apt/llvm-snapshot.gpg.key|apt-key add - \
    && apt-get update \
    && apt-get install -y liblldb-3.6 liblttng-ust0 libunwind8 libssl-dev libicu52 \
    && wget https://dotnetcli.blob.core.windows.net/dotnet/dev/Installers/Latest/dotnet-linux-x64.latest.deb \
    && dpkg -i dotnet-linux-x64.latest.deb \
    && rm dotnet-linux-x64.latest.deb

# https://github.com/dotnet/cli/issues/234 - DOTNET package does not set DOTNET_HOME which is required.
ENV DOTNET_HOME /usr/share/dotnet
