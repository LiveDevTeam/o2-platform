# Helper

*** Install MSSQL for Docker ***
docker pull microsoft/mssql-server-linux
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=_DEVELOPER_RACE_' -e 'MSSQL_PID=Express' -p 1445:1433 --name=catalogdb microsoft/mssql-server-linux:latest