# Bot Builder #

## Idea ## 
Create a Bot for disscord that allows members of a specific server add tickets with problems to be resolved. Those tickets will be added to a restful API server.
Finally user with administrator privileges should be able to view those tickets and resolve them.

## Expected Pain Points ##
- Integrating out database with the discord API.
- Managing roles to ensure the permissions are kept neat and tidy.
- Finding proper documentation for building bots in .NET that can integrate with the discord user interface

## Minimum Viable Product ##
- Users able to POST tickets to SQL server
- Admins able to GET tickets
- Admins able to UPDATE  tickets while resolving them
- Admin able to view all tickets, both open and closed
