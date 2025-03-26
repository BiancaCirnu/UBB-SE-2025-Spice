# Database Setup

This folder contains all database-related files for the SteamProfile application.

## Structure
- `DatabaseInit.sql` - Main database initialization script
- `Procedures/` - Contains all stored procedures organized by feature
  - `Users/` - User-related procedures
  - `Achievements/` - Achievement-related procedures
  - `Collections/` - Collection-related procedures
  - `Features/` - Feature-related procedures
  - `Wallet/` - Wallet-related procedures

## Setting Up the Database

1. Create a new database in SQL Server
2. Open SQL Server Management Studio (SSMS)
3. Connect to your database server
4. Open the `DatabaseInit.sql` script
5. Execute the script

## Adding New Stored Procedures

1. Create your stored procedure SQL file in the appropriate folder under `Procedures/`
2. Add a `:r` command in `DatabaseInit.sql` to include your new procedure
3. Commit both files to source control

Example:
```sql
-- In Data/Procedures/YourFeature/YourProcedure.sql
CREATE PROCEDURE YourProcedure
    @param1 INT,
    @param2 NVARCHAR(255)
AS
BEGIN
    -- Your procedure code here
END

-- In DatabaseInit.sql, add:
:r Data\Procedures\YourFeature\YourProcedure.sql
```

## Database Schema Updates

When you need to make changes to the database schema:
1. Create a new SQL file in the appropriate folder
2. Add the changes to `DatabaseInit.sql`
3. Update the README if necessary
4. Commit all changes to source control

## Best Practices

1. Always create stored procedures in the appropriate feature folder
2. Use meaningful names for procedures (e.g., `User_GetProfile`, `Achievement_Unlock`)
3. Include proper error handling in procedures
4. Document any complex procedures with comments
5. Test procedures before committing
6. Keep procedures focused on a single responsibility 