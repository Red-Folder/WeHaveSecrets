Write-Host "Starting Teamcity Servers"
docker start wehavesecrets_teamcity-server_1
docker start wehavesecrets_teamcity-agent-dotnet_1
docker start wehavesecrets_teamcity-agent-compose_1

Write-Host "Done, opening Teamcity"
Start-Process "http://localhost:8111"