#############################################################################################
# Import all modules
#############################################################################################
Get-ChildItem -Path scripts\*.psm1 -File | % {Foreach-Object {Import-Module $_.FullName}}

#############################################################################################
# Get variables
#############################################################################################
Write-Host 'Hi.  This script will help you get up and running.'
Write-Host 'First off, a few questions about how you want to configure the setup:'

$workingFolder = Read-Host -prompt 'Where would you like to put docker working files? [c:\tmp\wehavesecrets]'
if ([string]::IsNullOrEmpty($workingFolder)) {
    $workingFolder = 'c:\tmp\wehavesecrets'
}
if (-not (Test-Path $workingFolder)) {
    Write-Host 'Creating folder: $workingFolder'
    New-Item -Force -ItemType directory -Path $workingFolder
}
$env:WEHAVESECRETS_WORKINGFOLDER = $workingFolder

$sqlPassword = Read-Host -prompt 'What password do you want to use for SQL Server [Ch@ng3M3!]'
if ([string]::IsNullOrEmpty($sqlPassword)) {
    $sqlPassword = 'Ch@ng3M3!'
}
$env:WEHAVESECRETS_SQLPASSWORD = $sqlPassword

$sqlLocalPort = Read-Host -prompt 'What local port do you want for SQL Server [1433]'
if ([string]::IsNullOrEmpty($sqlLocalPort)) {
    $sqlLocalPort = '1433'
}
$env:WEHAVESECRETS_SQLLOCALPORT = $sqlLocalPort

$sqlLicenceAccepted = "N"
while ($sqlLicenceAccepted -ne "Y") {
    $sqlLicenceAccepted = Read-Host -prompt 'Do you accept the SQL Server licence (see https://hub.docker.com/r/microsoft/mssql-server-linux/) [Y/N]'
    if ($sqlLicenceAccepted -eq "N") {
        Write-Host 'Sorry that I have no non-SQL Server option at the moment.  If you would like one, please leave me a Github issue - if I get enough feedback, I will look at an alternative option'
        exit
    }
    if ($sqlLicenceAccepted -ne "Y") {
        Write-Host 'This project rerquires SQL Server to be used.  If you are unable to accept the licence, then this setup script will need to terminate'
        Write-Host 'Available options: Y - Accept the licence, N - Do not accept licence (and terminate setup)'
    }
}
$env:WEHAVESECRETS_SQLLICENCEACCEPTED = $sqlLicenceAccepted

#############################################################################################
# Setup TeamCity
#############################################################################################
Write-Host 'Starting the docker environment'
Start-DockerEnvironment -project 'WeHaveSecrets'

#############################################################################################
# Setup TeamCity
#############################################################################################
Write-Host 'Please setup TeamCity: http://localhost:8111'
Write-Host '    * Click Proceed to accept the Data Directory'
Write-Host '    * Use the Internal (HSQLDB) and click Proceed'
Write-Host '    * Wait for TeamCity to finish starting'
Write-Host '    * Review & accept the licence agreement (assuming you do) and click Continue'
Write-Host '    * Create your user'
$continue = $true
while ($continue) {
    $confirmation = Read-Host -Prompt 'When done, type "DONE"'
    if ($confirmation -eq 'DONE') {
        $continue = $false
    }
}
Write-Host 'TeamCity - Creating VCS - WeHaveSecretsCode'
Add-TeamCityVcs -id 'WeHaveSecretsCode' -url 'https://github.com/Red-Folder/WeHaveSecrets.git'
Write-Host 'TeamCity - Creating VCS - WeHaveSecretsConfig'
Add-TeamCityVcs -id 'WeHaveSecretsConfig' -url 'https://github.com/Red-Folder/WeHaveSecrets-TeamCityConfig.git'
Write-Host 'TeamCity - Creating Project - WeHaveSecrets'
Add-TeamCityProject 'WeHaveSecrets'
Write-Host 'TeamCity - Setting Project source control'
Set-TeamCityProjectVersionSource -projectid 'WeHaveSecrets' -vcsid 'WeHaveSecretsConfig'

Write-Host 'TeamCity - Waiting for project to be restored'
Await-TeamCityProjectRestored -id 'WeHaveSecrets'

Write-Host 'TeamCity - Authorising agent'
Set-TeamCityAgentAuthorisation -id 'agent1'
Write-Host 'TeamCity - Adding Project to default agent pool'
Add-TeamCityProjectToDefaultPool -id 'WeHaveSecrets' -name 'WeHaveSecrets'



Write-Host 'Ok, we are pretty much done'