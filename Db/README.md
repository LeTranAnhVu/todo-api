#### Requirement
- Install grate using dotnet
```
    dotnet tool install --global grate
```

#### Run
##### Initial step
Create the file `db.conf` which content format like `db.example.conf`

##### To drop and create new database everytime
```
    .\Drop_And_Create_Db_Local.ps1 ./db.[<env>].conf
```
or 
```
    /.Drop_And_Create_Db_Local.sh ./db.[<env>].conf
```
##### To update new database everytime
```
    .\Update_Db_Local.ps1 ./db.[<env>].conf
```
or 
```
    .\Update_Db_Local.sh ./db.[<env>].conf
```