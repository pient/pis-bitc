echo Execute SqlWorkflowInstanceStoreSchema.sql
sqlcmd -S %COMPUTERNAME%\MSSQLSERVER -E -n -d PersistenceDatabase -i "SqlWorkflowInstanceStoreSchema.sql"

echo Execute SqlWorkflowInstanceStoreLogic.sql
sqlcmd -S %COMPUTERNAME%\MSSQLSERVER -E -n -d PersistenceDatabase -i "SqlWorkflowInstanceStoreLogic.sql"

::Pause