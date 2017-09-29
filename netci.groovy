import jobs.generation.Utilities

def project = GithubProject
def branch = GithubBranchName
def isPR = true
def platformList = ['Ubuntu16.04:Debian', 'Windows_2016:NanoServer']

platformList.each { platform ->
    def(hostOS, containerOS) = platform.tokenize(':')
    def machineLabel = (hostOS == 'Windows_2016') ? 'latest-docker' : 'latest-or-auto-docker'
    def versionList = (hostOS == 'Windows_2016') ? ['1.', '2.0', '2.1'] : ['1.', '2.']

    versionList.each { version ->
        def newJobName = Utilities.getFullJobName(project, "${containerOS}_${version}", isPR)

        def newJob = job(newJobName) {
            steps {
                if (hostOS == 'Windows_2016') {
                    batchFile("powershell -NoProfile -Command .\\build-and-test.ps1 -Filter ${version}")
                }
                else {
                    shell("docker build --rm -t testrunner -f ./test/Dockerfile.linux.testrunner . && docker run -v /var/run/docker.sock:/var/run/docker.sock testrunner powershell -File build-and-test.ps1 -Filter ${version}")
                }
            }
        }

        Utilities.setMachineAffinity(newJob, hostOS, machineLabel)
        Utilities.standardJobSetup(newJob, project, isPR, "*/${branch}")
        Utilities.addGithubPRTriggerForBranch(newJob, branch, "${containerOS} - ${version} Dockerfiles")
    }
}
