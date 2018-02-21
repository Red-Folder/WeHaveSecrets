Write-Host "Stopping WeHaveSecrets environment"
docker stop wehavesecrets_tests-end2end_1
docker stop wehavesecrets_web_1
docker stop wehavesecrets_db_1


Write-Host "Stopping Teamcity Servers"
docker stop wehavesecrets_teamcity-server_1
docker stop wehavesecrets_teamcity-agent-dotnet_1
docker stop wehavesecrets_teamcity-agent-compose_1

Write-Host "Done"
