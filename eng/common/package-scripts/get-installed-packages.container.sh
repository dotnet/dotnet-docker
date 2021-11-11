#!/usr/bin/env sh

# If package manager is apt
if type apt > /dev/null 2>/dev/null; then
    # Extract the package name and version out of the list of installed packages.
    # Example output of "apt list":
    #   zlib1g/now 1:1.2.11.dfsg-1 amd64 [installed,local]
    # Regex consists of two capture groups:
    #   1: Package name: all chars up until the first '/' character
    #   2: Package version: substring after the first whitespace occurrence and before the next whitespace
    # Output is the format of "DEB,<pkg-name>=<pkg-version>"
    apt list --installed 2>/dev/null | grep installed | sed -n 's/^\([^/]*\)\S*\s\(\S*\).*/DEB,\1=\2/p' | sort
    exit 0
fi

# If package manager is apk
if type apk > /dev/null 2>/dev/null; then
    # Extract the package name and version out of the list of installed packages.
    # This uses the output of the "apk info" command to build a regex to find the version from the "apk list"
    # command. The reason the "apk info -v" or "apk list --installed" commands (which provide both the package name and version)
    # aren't used just by itself is because of the ambiguity between what characters are part of the package name and which are 
    # part of the version. For example, the output for something like "libssl1.1-1.1.1k-r0" can be ambiguous because of the dash 
    # separator. In this case, the package name is "libssl1.1" and the version is "1.1.1k-r0". You can't necessarily assume that 
    # the first '-' character indicates a version separator because some package names also contain a '-' character.  What the 
    # command below does is to first get the list of package names (w/o any versions) and then feed each package name into another
    # command to lookup that package name with the "apk list" command to get the version details.
    # Example of the output of "apk info -d musl":
    #   musl-1.2.2-r3 description:
    #   the musl c library (libc) implementation
    # Output is the format of "APK,<pkg-name>=<pkg-version>"
    apk info 2>/dev/null | xargs -I {} sh -c "apk list 2>/dev/null {} | grep '\[installed\]' 2>/dev/null | sed -n 's/\({}\)-\(\S*\)\s.*/APK,\1=\2/p'" | sort
    exit 0
fi

# If package manager is tdnf
if type tdnf > /dev/null 2>/dev/null; then
    # Extract the package name and version out of the list of installed packages.
    # Example output of "tdnf list installed":
    #   zstd-libs.x86_64   1.4.4-1.cm1   @System
    # Regex consists of two capture groups:
    #   1: Package name: all chars up until the first '.' character
    #   2: Package version: substring after the remaining arch text/whitespace and before the next whitespace
    # Output is the format of "RPM,<pkg-name>=<pkg-version>"
    tdnf list installed --quiet | tail -n +2 | sed -n 's/^\([^\.]*\)\S*\s*\(\S*\)\s*.*/RPM,\1=\2/p'
    exit 0
fi

echo "Unsupported package manager. Current supported package managers: apt, apk, tdnf" >&2
exit 1
