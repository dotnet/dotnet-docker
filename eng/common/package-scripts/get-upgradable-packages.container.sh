#!/usr/bin/env sh

# This is just a wrapper around the Bash-based script. This ensures that Bash is installed and then runs the Bash script. 

ensureBashInstalledForApt() {
    echo "Ensuring bash is installed"
    apt update 1>/dev/null 2>/dev/null
    apt install -y bash 1>/dev/null 2>/dev/null
}

ensureBashInstalledForApk() {
    echo "Ensuring bash is installed"
    apk add bash 1>/dev/null
}

ensureBashInstalledForTdnf() {
    echo "Ensuring bash is installed"
    tdnf makecache 1>/dev/null
    tdnf install -y bash 1>/dev/null
}



scriptDir=$(dirname $0)

if type apt > /dev/null 2>/dev/null; then
    ensureBashInstalledForApt
    $scriptDir/get-upgradable-packages.container.bash.sh $@
    exit 0
fi

if type apk > /dev/null 2>/dev/null; then
    ensureBashInstalledForApk
    $scriptDir/get-upgradable-packages.container.bash.sh $@
    exit 0
fi

if type tdnf > /dev/null 2>/dev/null; then
    ensureBashInstalledForTdnf
    $scriptDir/get-upgradable-packages.container.bash.sh $@
    exit 0
fi

echo "Unsupported package manager. Current supported package managers: apt, apk, tdnf" >&2
exit 1
