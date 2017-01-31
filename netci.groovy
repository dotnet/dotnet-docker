import jobs.generation.Utilities

def project = GithubProject
def branch = GithubBranchName
def isPR = true
def platformList = ['Ubuntu16.04:Debian', 'Windows_2016:NanoServer']

platformList.each { platform ->
    def(hostOS, containerOS) = platform.tokenize(':')
    def newJobName = Utilities.getFullJobName(project, containerOS, isPR)
    def machineLabel = (hostOS == 'Windows_2016') ? 'latest-containers' : 'latest-or-auto-docker'

    def newJob = job(newJobName) {
        steps {
            if (hostOS == 'Windows_2016') {
                batchFile("powershell -NoProfile -Command .\\build-and-test.ps1")
            }
            else {
                shell("docker build --rm -t testrunner -f ./test/Dockerfile.linux.testrunner . && docker run -v /var/run/docker.sock:/var/run/docker.sock testrunner powershell -File build-and-test.ps1")
            }
        }
    }

    Utilities.setMachineAffinity(newJob, hostOS, machineLabel)
    Utilities.standardJobSetup(newJob, project, isPR, "*/${branch}")
    Utilities.addGithubPRTriggerForBranch(newJob, branch, containerOS)
}
