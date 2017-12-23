#############################################################################################
# Import all modules
#############################################################################################
Get-ChildItem -Path scripts\*.psm1 -File | % {Foreach-Object {Import-Module $_.FullName}}

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
Add-TeamCityVcs -id 'WeHaveSecretsConfig' -url 'https://github.com/Red-Folder/TestRepo.git'
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