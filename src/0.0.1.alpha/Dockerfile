FROM buildpack-deps:jessie-scm

# install dotnet
RUN echo "deb [arch=amd64] http://tux-devrepo.corp.microsoft.com/repos/dotnet-dev/ trusty main" > /etc/apt/sources.list.d/dotnetdev.list \
    && apt-key adv --keyserver tux-devrepo.corp.microsoft.com --recv-keys 008C0E6C \
    && echo "deb [arch=amd64] http://llvm.org/apt/trusty/ llvm-toolchain-trusty-3.6 main" > /etc/apt/sources.list.d/llvm.list \
    && wget -O - http://llvm.org/apt/llvm-snapshot.gpg.key|apt-key add - \
    && apt-get update \
    && apt-get install -y dotnet
