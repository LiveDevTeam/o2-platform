# Helper

*** Install MSSQL for Docker ***
docker pull microsoft/mssql-server-linux
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=_DEVELOPER_RACE_' -e 'MSSQL_PID=Express' -p 1445:1433 --name=catalogdb microsoft/mssql-server-linux:latest


dotnet tool install --global dotnet-ef

*** On mac ***
dotnet ef migrations add InitMigration -o Data/Migrations
*** On Windows ***
dotnet ef migrations add InitMigration -o Data/Migrations -c




docker build -t o2bionics/o2-catalog.api .

*** delete all images ***
docker rmi $(docker images --filter "dangling=true" -q --no-trunc)