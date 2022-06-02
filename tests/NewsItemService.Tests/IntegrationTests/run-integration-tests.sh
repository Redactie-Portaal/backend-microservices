#!/bin/bash

# Explanation
# Integration tests make use of a Docker image for running the database.
# In order to run this database in a CI environment efficiently, this script has been created.
# The script executes the Docker compose file, which run the database.
# It then looks in the docker logs, to see if the database is ready for the tests.
# After the database is ready, the integration tests will run.

# Script workflow
# 1. Spin up a PostgreSQL Docker environmnet
# 2. Wait for PostgreSQL to boot up
# 3. Run the Integration tests

printf "Message: Helper script for running Integration tests. \n"

printf "Message: Going to the working directory of the Integration tests."
cd $1/IntegrationTests
printf '\n'

printf "Message: current working directory: "
echo $PWD
printf '\n'

printf "Message: Running Docker compose file to pull up & boot up PostgreSQL container. \n"
printf "Message: Command that is executed: 'docker-compose up -d' \n"
docker-compose -f docker-compose.integrationtests.yml up -d
printf "\n"

printf "Message: Waiting for PostgreSQL to boot up, in a 5 second interval \n"
printf "Message: Command that is executed: 'docker logs postgresql' \n"
until [[ $(docker logs postgresqlintegration) == *"database system is ready to accept connections"* ]]
do
  printf "Message: still waiting on PostgreSQL to boot..."
  sleep 5s
done
printf "\n"

printf "Message: Going back to the directory of the project file."
cd ../
printf '\n'

printf "Message: current working directory: "
echo $PWD
printf '\n'

printf "Message: The environment is ready. Time to run the integration tests. \n"
printf "Message: Command that executed: {dotnet test --logger 'trx;LogFileName=test-results.trx' --collect:'XPlat Code Coverage'} \n"
dotnet test $1 --filter FullyQualifiedName~$2 --logger "trx;LogFileName=test-results.trx" --collect:"XPlat Code Coverage"
printf "\n"