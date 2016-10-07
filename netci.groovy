import jobs.generation.Utilities

def project = GithubProject
def branch = GithubBranchName
def isPR = true
def platformList = ['Ubuntu16.04:Debian'/*, 'Windows_2016:NanoServer'*/]

platformList.each { platform ->
    def(hostOS, containerOS) = platform.tokenize(':')
    def newJobName = Utilities.getFullJobName(project, containerOS, isPR)

    def newJob = job(newJobName) {
        steps {
            if (hostOS == 'Windows_2016') {
                batchFile("powershell -NoProfile -Command .\\build-and-test.ps1")
            }
            else {
                shell("./build-and-test.sh")
            }
        }
    }

    Utilities.setMachineAffinity(newJob, hostOS, 'latest-or-auto-docker')
    Utilities.standardJobSetup(newJob, project, isPR, "*/${branch}")
    Utilities.addGithubPRTriggerForBranch(newJob, branch, containerOS)
}
