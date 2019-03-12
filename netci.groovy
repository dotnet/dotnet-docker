import jobs.generation.Utilities

def project = GithubProject
def branch = GithubBranchName
def isPR = true
def platformList = [
        'Ubuntu16.04:Linux',
        'Windows_2016:NanoServer-1709',
        'Windows_2016:NanoServer-1803',
        'Windows_2016:NanoServer-1809'
    ]

platformList.each { platform ->
    def(hostOS, containerOS) = platform.tokenize(':')
    def machineLabel = (hostOS == 'Windows_2016') ? 'latest-docker' : 'latest-or-auto-docker'

    if (containerOS == 'NanoServer-1709' || containerOS == 'NanoServer-1803') {
        versionList = ['2.1', '2.2', '3.0']
    }
    else if (containerOS == 'NanoServer-1809') {
        versionList = ['1.', '2.1', '2.2', '3.0']
    }
    else {
        versionList = ['1.', '2.1-2.2', '3.0']
    }

    versionList.each { version ->
        def newJobName = Utilities.getFullJobName(project, "${containerOS}_${version}", isPR)
        def versionFilters = version.tokenize( '-' )
        def newJob = job(newJobName) {
            steps {
                if (hostOS == 'Windows_2016') {
                    batchFile("powershell -NoProfile -Command .\\scripts\\Invoke-CleanupDocker.ps1")
                    try {
                        versionFilters.each { versionFilter ->
                            batchFile("powershell -NoProfile -Command .\\build-and-test.ps1 -VersionFilter \"${versionFilter}*\" -OSFilter \"${containerOS}\"")
                        }
                    }
                    finally {
                        batchFile("powershell -NoProfile -Command .\\scripts\\Invoke-CleanupDocker.ps1")
                    }
                }
                else {
                    shell("./scripts/cleanup-docker.sh")
                    try {
                        shell("docker build --rm -t testrunner -f ./tests/Dockerfile.linux.testrunner .")

                        versionFilters.each { versionFilter ->
                            shell("docker run -v /var/run/docker.sock:/var/run/docker.sock testrunner pwsh -File build-and-test.ps1 -VersionFilter \"${versionFilter}*\"")
                        }
                    }
                    finally {
                        shell("./scripts/cleanup-docker.sh")
                    }
                }
            }
        }

        if (containerOS == 'NanoServer-1709') {
            newJob.with {label('windows.10.amd64.serverrs3.open')}
        }
        else if (containerOS == 'NanoServer-1803') {
            newJob.with {label('windows.10.amd64.serverrs4.open')}
        }
        else if (containerOS == 'NanoServer-1809') {
            newJob.with {label('windows.10.amd64.serverrs5.open')}
        }      
        else {
            Utilities.setMachineAffinity(newJob, hostOS, machineLabel)
        }

        Utilities.standardJobSetup(newJob, project, isPR, "*/${branch}")
        Utilities.addGithubPRTriggerForBranch(newJob, branch, "${containerOS} - ${version} Dockerfiles")
    }
}
