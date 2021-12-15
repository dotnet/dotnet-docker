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

ensureBashInstalledForRpmBasedPkgMgr() {
    echo "Ensuring bash is installed"
    local pkgMgr=$1
    $pkgMgr makecache 1>/dev/null
    $pkgMgr install -y bash 1>/dev/null
}

ensureBashInstalledForZypper() {
    echo "Ensuring bash is installed"
    zypper ref 1>/dev/null
    zypper install -y bash 1>/dev/null
}


scriptDir=$(dirname $0)

outputPath="$1"
pkgManagerArgs="$2"
shift 2
packages=$@

if type apt > /dev/null 2>/dev/null; then
    ensureBashInstalledForApt
    $scriptDir/get-upgradable-packages.container.bash.sh $outputPath "$pkgManagerArgs" $packages
    exit 0
fi

if type apk > /dev/null 2>/dev/null; then
    ensureBashInstalledForApk
    $scriptDir/get-upgradable-packages.container.bash.sh $outputPath "$pkgManagerArgs" $packages
    exit 0
fi

if type dnf > /dev/null 2>/dev/null; then
    ensureBashInstalledForRpmBasedPkgMgr dnf
    $scriptDir/get-upgradable-packages.container.bash.sh $outputPath "$pkgManagerArgs" $packages
    exit 0
fi

if type tdnf > /dev/null 2>/dev/null; then
    ensureBashInstalledForRpmBasedPkgMgr tdnf
    $scriptDir/get-upgradable-packages.container.bash.sh $outputPath "$pkgManagerArgs" $packages
    exit 0
fi

if type yum > /dev/null 2>/dev/null; then
    ensureBashInstalledForRpmBasedPkgMgr yum
    $scriptDir/get-upgradable-packages.container.bash.sh $outputPath "$pkgManagerArgs" $packages
    exit 0
fi

if type zypper > /dev/null 2>/dev/null; then
    ensureBashInstalledForZypper
    $scriptDir/get-upgradable-packages.container.bash.sh $outputPath "$pkgManagerArgs" $packages
    exit 0
fi

echo "Unsupported package manager. Current supported package managers: apt, apk, tdnf, yum, zypper" >&2
exit 1
