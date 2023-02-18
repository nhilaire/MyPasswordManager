# MyPasswordManager

The purpose of this repo is to build a web app in Blazor that can manage your passwords in a centralized area. The infrastructure use **frees tiers** of Azure's services.

### Prepare Azure Infrastructure
I use azure cli to provision my infrastructure.
```sh
# first you have to login
az login

# create a ressource group
# warning, depending of $location, free tiers of cosmos db may not be available
# if you encounter an error, please change $location
$location = "westus"
$groupName = "MyPasswordManagerGroup"
az group create -l $location -n $groupName

# create cosmosdb account ($cosmosdbaccount must be unique)
$cosmosdbaccount = "yourcosmosdbaccountname"
az cosmosdb create -n $cosmosdbaccount --enable-free-tier true --default-consistency-level "Session" --resource-group $groupName

# wait for account to be created

# then create database
$dbName = "yourdatabasename"
az cosmosdb sql database create -a $cosmosdbaccount -g $groupName -n $dbName

#then create cosmosdb's container
$containerName = "yourcontainername"
az cosmosdb sql container create -a $cosmosdbaccount -g $groupName -d $dbName -n $containerName -p '/pkey'
```

Now that everything is created we can look for connection strings and create dotnet secrets for our project

```sh
# note the account key
az cosmosdb keys list -n $cosmosdbaccount -g $groupName --type connection-strings
```

Place yourself in the test directory test\MyPasswordManager.Intregation.Tests, then init dotnet secrets :
```sh
dotnet user-secrets init
# add connection string to secret, I use ##values## that will be replaced later in code
dotnet user-secrets set "CosmosDbConfiguration:ConnectionString" "AccountEndpoint=https://##login##.documents.azure.com:443/;AccountKey=##password##"
dotnet user-secrets set "CosmosDbConfiguration:DatabaseName" $dbName
dotnet user-secrets set "CosmosDbConfiguration:ContainerName" $containerName
# this two values are used only for test project
dotnet user-secrets set "login" $dbName
dotnet user-secrets set "password" "xxxx" <-- put the account key here

```
In case of errors and you want to delete everything, just delete the ressource group
```sh
az group delete --name $groupName
```

