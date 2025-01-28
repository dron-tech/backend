pipeline {
  agent any
  environment {
    REGISTRY_HOST = credentials('docker-registry-host')
    REGISTRY_HOST_REMOTE = credentials('docker-registry-domain')
    JENKINS_SERVER = credentials('jenkins-server')
    GIT_REPO_NAME = env.GIT_URL.replaceFirst(/^.*\/([^\/]+?).git$/, '$1').toLowerCase().trim()
    SLACK_CHANNEL = 'C053HJTAW95'
  }

  stages {
    stage ('Check build') {
      when { changeRequest() }

      steps {
        script {
          sh """
            docker run --rm -v "\$(pwd):/app" mcr.microsoft.com/dotnet/sdk:6.0 sh -c "cd /app; dotnet restore "WebApp/WebApp.csproj" && dotnet publish WebApp -c Release -o build --no-restore"
          """
        }
      }
    }

    stage('Build') {
      parallel {
        stage('Prod') {
          when {
            allOf {
              not {
                changeRequest()
              }
              anyOf {
                branch 'main'
                branch 'master'
              }
            }
          }

          environment {
            DOCKER = credentials('DOCKER')
            DB = credentials('DB')
          }

          steps {
            sh '''
              sed -i -e "s/ID=[^ ]*;/ID=$DB_USR;/" Rps/appsettings.json
              sed -i -e "s/Password=[^ ]*;/Password=$DB_PSW;/" Rps/appsettings.json

              echo $DOCKER_PSW > .docker_password
              cat .docker_password | docker login $REGISTRY_HOST_REMOTE -u $DOCKER_USR --password-stdin

              docker-compose --env-file .production.env build
              docker-compose --env-file .production.env push

              rm .docker_password
              git restore Rps/appsettings.json
            '''
            notify_slack('Production build success')
          }
        }

        stage('Dev') {
          when {
            allOf {
              not {
                changeRequest()
              }
              anyOf {
                branch 'dev'
                branch 'development'
              }
            }
          }

          environment {
            ENV_FILE = '.development.env'

            COMPOSE_PROJECT_NAME = 'itbit'
            DOTNET_ENVIRONMENT = 'Development'

            DB_USERNAME = 'postgres'
            DB_PASSWORD = 'root'
            DB_NAME = 'itbit' 
          }

          steps {
            sh """
              touch ${ENV_FILE}
              truncate -s 0 ${ENV_FILE}

              echo COMPOSE_PROJECT_NAME=${COMPOSE_PROJECT_NAME} >> ${ENV_FILE}
              echo DOTNET_ENVIRONMENT=${DOTNET_ENVIRONMENT} >> ${ENV_FILE}

              echo DB_USERNAME=${DB_USERNAME} >> ${ENV_FILE}
              echo DB_PASSWORD=${DB_PASSWORD} >> ${ENV_FILE}
              echo DB_NAME=${DB_NAME} >> ${ENV_FILE}

              docker-compose --env-file ${ENV_FILE} build
              docker-compose --env-file ${ENV_FILE} push
            """
          }
        }
      }
    }

    stage('Start') {
      parallel {
        stage('Prod') {
          when {
            allOf {
              not {
                changeRequest()
              }
              anyOf {
                branch 'master'
                branch 'main'
              }
            }
          }

          stages {
            stage('Approve') {
              input {
                message 'Deploy this build?'
                ok 'Yes'
                submitter ', alukashenko, nbobkov'
              }

              environment {
                LOKI = credentials('LOKI')
                DOCKER = credentials('DOCKER')
                PGADMIN = credentials('PGADMIN')
                DB = credentials('DB')
                SSH_PROFILE = ''
                FOLDER = 'backend'
                PRODUCTION_URL = ''
              }

              steps {
                sh '''
                  ssh -o UserKnownHostsFile=/dev/null -o StrictHostKeyChecking=no $SSH_PROFILE bash -c "'
                    mkdir -p $FOLDER
                  '"

                  scp -o UserKnownHostsFile=/dev/null -o StrictHostKeyChecking=no docker-compose.prod.yml $SSH_PROFILE:$FOLDER
                  scp -o UserKnownHostsFile=/dev/null -o StrictHostKeyChecking=no .production.env $SSH_PROFILE:$FOLDER

                  ssh -o UserKnownHostsFile=/dev/null -o StrictHostKeyChecking=no $SSH_PROFILE \
                    bash -c "'
                      cd $FOLDER
                      sed -i \
                        -e s/^LOKI_USR=.*/LOKI_USR=$LOKI_USR/ \
                        -e s/^LOKI_PSW=.*/LOKI_PSW=$LOKI_PSW/ \
                        -e s/^DB_USERNAME=.*/DB_USERNAME=$DB_USR/ \
                        -e s/^DB_PASSWORD=.*/DB_PASSWORD=$DB_PSW/ \
                        -e s/^PGADMIN_USR=.*/PGADMIN_USR=$PGADMIN_USR/ \
                        -e s/^PGADMIN_PSW=.*/PGADMIN_PSW=$PGADMIN_PSW/ \
                        .production.env

                      echo $DOCKER_PSW > .docker_password
                      cat .docker_password | docker login $REGISTRY_HOST_REMOTE -u $DOCKER_USR --password-stdin

                      docker compose -f docker-compose.prod.yml --env-file .production.env pull
                      docker compose -f docker-compose.prod.yml --env-file .production.env up -d
                    '"

                  git restore .production.env
                '''
                notify_slack("Production deployment success")
              }
            }
          }
        }

        stage('Dev') {
          when {
            allOf {
              not {
                changeRequest()
              }
              anyOf {
                branch 'development'
                branch 'dev'
              }
            }
          }

          environment {
            ENV_FILE = '.development.env'
          }

          steps {
            script {
              sh """
                if [ "\$(docker-compose --env-file .development.env port traefik 80)" ]; then
                  IMAGE_PREVIOUS_PORT="\$(docker-compose --env-file .development.env port traefik 80 | egrep "[0-9]+\$" -o)"
                fi

                if [ -z "\${IMAGE_PREVIOUS_PORT}" ]; then
                  WEB_PORT=80 \
                    docker-compose --env-file ${ENV_FILE} up -d
                else
                  WEB_PORT="\${IMAGE_PREVIOUS_PORT}:80" \
                    docker-compose --env-file ${ENV_FILE} up -d
                fi
              """
            }
            notify_slack("Traefik backend startup success")
          }
        }
      }
    }
  }

  post {
    failure {
      script {
        if (
          env.BRANCH_NAME == "development" ||
          env.BRANCH_NAME == "dev" ||
          env.BRANCH_NAME == "master" ||
          env.BRANCH_NAME == "main"
        ) {
          notify_slack('Build failure')
        }
      }
    }
  }
}
