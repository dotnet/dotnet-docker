#!/usr/bin/env bash

# This is the Bash-based script for retrieving the list of upgradable packages

printPackageInfo() {
    echo $1
    echo "- Current: $2"
    echo "- Upgrade: $3"
}

writeError() {
    echo "Error: $1" >>/dev/stderr
    exit 1
}

addUpgradeablePackageVersion() {
    local pkgName=$1
    local currentVersion=$2
    local upgradeVersion=$3

    # If the package is not already in the list, add it
    if [[ -z ${upgradablePackageVersions[$pkgName]} ]]; then
        local versionData=$(echo "$currentVersion,$upgradeVersion")
        upgradablePackageVersions[$pkgName]=$versionData

        printPackageInfo "$pkgName" "$currentVersion" "$upgradeVersion"
    fi
}

checkForUpgradableVersionWithApt() {
    pkgName=$1
    pkgVersion=$2

    echo "Finding latest version of package $pkgName"
    local pkgInfo=$(apt policy $pkgManagerArgs $pkgName 2>/dev/null)
    if [[ $pkgInfo == "" ]]; then
        writeError "Package '$pkgName' does not exist."
    fi

    # Get the candidate version of the package to be installed
    local candidateVersion=$(echo "$pkgInfo" | sed -n 's/.*Candidate:\s*\(\S*\)/\1/p')

    # If a newer version of the package is available
    if [[ $candidateVersion != $pkgVersion ]]; then
        # Check if the candidate package version comes from a security repository
        apt-cache madison $pkgName | grep $candidateVersion | grep security 1>/dev/null

        # If the candidate version comes from a security repository, add it to the list of upgradable packages
        if [[ $? == 0 ]]; then
            addUpgradeablePackageVersion "$pkgName" "$pkgVersion" "$candidateVersion"
        fi
    fi
}

checkForUpgradableVersionWithApk() {
    pkgName=$1
    pkgVersion=$2

    echo "Finding latest version of package $pkgName"
    availableVersion=$(apk list $pkgManagerArgs $pkgName | tac | sed -n "1 s/$pkgName-\(\S*\).*/\1/p")
    if [[ $availableVersion == "" ]]; then
        writeError "Package '$pkgName' does not exist."
    fi

    # If a newer version of the package is available
    if [[ $availableVersion != $pkgVersion ]]; then
        # If the package exists, add it to the list of upgradable packages
        if [[ $availableVersion != "" ]]; then
            addUpgradeablePackageVersion "$pkgName" "$pkgVersion" "$availableVersion"
        fi
    fi
}

checkForUpgradableVersionWithCommonRpmBasedPkgMgr() {
    pkgName=$1
    pkgVersion=$2
    pkgManagerName=$3
    headerLineCount=${4:-2}

    echo "Finding latest version of package $pkgName"
    $pkgManagerName install $pkgManagerArgs -y $pkgName 1>/dev/null 2>/dev/null

    # If the package exists
    if [[ $? == 0 ]]; then
        local installedVersion=$($pkgManagerName list installed $pkgManagerArgs $pkgName | tail -n +$headerLineCount | sed -n 's/\S*\s*\(\S*\)\s*.*/\1/p')
        # If a newer version of the package is available
        if [[ $installedVersion != $pkgVersion ]]; then
            addUpgradeablePackageVersion "$pkgName" "$pkgVersion" "$installedVersion"
        fi
    else
        writeError "Package '$pkgName' does not exist."
    fi
}

checkForUpgradableVersionWithDnf() {
    pkgName=$1
    pkgVersion=$2

    checkForUpgradableVersionWithCommonRpmBasedPkgMgr "$pkgName" "$pkgVersion" "dnf"
}

checkForUpgradableVersionWithTdnf() {
    pkgName=$1
    pkgVersion=$2

    checkForUpgradableVersionWithCommonRpmBasedPkgMgr "$pkgName" "$pkgVersion" "tdnf"
}

checkForUpgradableVersionWithYum() {
    pkgName=$1
    pkgVersion=$2

    checkForUpgradableVersionWithCommonRpmBasedPkgMgr "$pkgName" "$pkgVersion" "yum" 3
}

checkForUpgradableVersionWithZypper() {
    pkgName=$1
    pkgVersion=$2

    echo "Finding latest version of package $pkgName"
    availableVersion=$(zypper info $pkgManagerArgs $pkgName | sed -n 's/^Version\s*:\s*\(\S*\).*/\1/p')
    if [[ $availableVersion == "" ]]; then
        writeError "Package '$pkgName' does not exist."
    fi

    # If a newer version of the package is available
    if [[ $availableVersion != $pkgVersion ]]; then
        # If the package exists, add it to the list of upgradable packages
        if [[ $availableVersion != "" ]]; then
            addUpgradeablePackageVersion "$pkgName" "$pkgVersion" "$availableVersion"
        fi
    fi
}

outputPackagesToUpgrade() {
    echo
    echo "Packages requiring an upgrade:"

    local packagesToUpgrade=()
    local pkg
    # Lookup the provided package names to see if any are in the list of upgradable packages
    for pkg in "${packages[@]}"
    do
        if [[ ! $pkg =~ $packageVersionRegex ]]; then
            writeError "Unable to parse package version info. Value: $pkg"
        fi

        local pkgName=${BASH_REMATCH[1]}
        versionData=${upgradablePackageVersions[$pkgName]}

        if [ ! -z "$versionData" ]; then
            # Split versionData by comma
            local versionArray=( ${versionData//,/ } )
            local currentVersion=${versionArray[0]}
            local upgradeVersion=${versionArray[1]}
            
            packagesToUpgrade+=($(echo "$pkgName,$currentVersion,$upgradeVersion"))
            printPackageInfo "$pkgName" "$currentVersion" "$upgradeVersion"
        fi
    done

    local upgradeCount=${#packagesToUpgrade[@]}
    if [ $upgradeCount = 0 ]; then
        echo "<none>"
    fi

    local outputDir=$(dirname "$outputPath")
    mkdir -p $outputDir

    printf "%s\n" "${packagesToUpgrade[@]}" > $outputPath
}

updatePackageCacheWithApt() {
    apt update 1>/dev/null 2>/dev/null
}

updatePackageCacheWithApk() {
    apk update 1>/dev/null
}

updatePackageCacheWithCommonRpmBasedPkgMgr() {
    pkgMgr=$1
    $1 makecache 1>/dev/null
}

updatePackageCacheWithDnf() {
    updatePackageCacheWithCommonRpmBasedPkgMgr dnf
}

updatePackageCacheWithTdnf() {
    updatePackageCacheWithCommonRpmBasedPkgMgr tdnf
}

updatePackageCacheWithYum() {
    updatePackageCacheWithCommonRpmBasedPkgMgr yum
}

updatePackageCacheWithZypper() {
    zypper ref 1>/dev/null
}



outputPath="$1"
pkgManagerArgs="$2"
shift 2
packages=( $@ )

declare -A upgradablePackageVersions
upgradablePackageVersions=()

if type apt > /dev/null 2>/dev/null; then
    pkgType="Apt"
elif type apk > /dev/null 2>/dev/null; then
    pkgType="Apk"
elif type dnf > /dev/null 2>/dev/null; then
    pkgType="Dnf"
elif type tdnf > /dev/null 2>/dev/null; then
    pkgType="Tdnf"
elif type yum > /dev/null 2>/dev/null; then
    pkgType="Yum"
elif type zypper > /dev/null 2>/dev/null; then
    pkgType="Zypper"
else
    writeError "Unsupported package manager. Current supported package managers: apt, apk, tdnf, yum, zypper"
fi

if [[ $(type -t updatePackageCacheWith$pkgType) != "function" ]]; then
    writeError "Missing function named 'updatePackageCacheWith$pkgType'"
fi

if [[ $(type -t checkForUpgradableVersionWith$pkgType) != "function" ]]; then
    writeError "Missing function named 'checkForUpgradableVersionWith$pkgType'"
fi

echo "Updating package cache..."
updatePackageCacheWith$pkgType

packageVersionRegex="(\S+)=(\S+)"
for pkgName in "${packages[@]}"
do
    echo "Processing $pkgName"
    if [[ ! $pkgName =~ $packageVersionRegex ]]; then
        writeError "Package version info for '$pkgName' must be in the form of <pkg-name>=<pkg-version>"
    fi

    checkForUpgradableVersionWith$pkgType ${BASH_REMATCH[1]} ${BASH_REMATCH[2]}
done
outputPackagesToUpgrade
