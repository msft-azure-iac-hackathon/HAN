CREATE USER [app-we-han] FROM EXTERNAL PROVIDER; 
ALTER ROLE db_datareader ADD MEMBER [app-we-han]; 
ALTER ROLE db_datawriter ADD MEMBER [app-we-han]; 
ALTER ROLE db_ddladmin ADD MEMBER [app-we-han]; 
GO