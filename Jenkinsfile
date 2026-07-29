pipeline {
    agent {
        label 'linux-build-agent-01'
    }

    stages {
        stage('Agent Test') {
            steps {
                sh '''
                    whoami
                    hostname
                    id
                    docker version
                '''
            }
        }
    }
}