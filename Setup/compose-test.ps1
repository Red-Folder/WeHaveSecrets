cd ..\Db.WeHaveSecrets
dotnet publish --output obj/Docker/publish

cd ..\WeHaveSecrets
dotnet publish --output obj/Docker/publish

cd ..\Setup
$env:WEHAVESECRETS_WORKINGFOLDER = '/c/tmp/wehavesecrets'
$env:WEHAVESECRETS_SQLPASSWORD = 'Ch@ng3M3!'
$env:WEHAVESECRETS_SQLLOCALPORT = 1433
$env:WEHAVESECRETS_SQLLICENCEACCEPTED = 'Y'

docker-compose -p 'WeHaveSecrets' up -d web
