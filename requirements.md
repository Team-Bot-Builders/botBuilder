# Bot Builder Sowftware Requirements #

## Vision ##

WHAT IS YOUR PURPOSE

## Scope (In/Out) ##

- In of Scope:
- - API connects ot Discord Bot
- - Submitting a ticket to the database
- - Retreiving a ticket from the database
- - Administrator closing a ticket
- - Administrator can change user's privelege level

- Out of Scope:
- - Integrating Discord authorization with outside services
- - Recreatig work added by the Identity service
- - 

## Minimum Viable Project (MVP) ##
- Users able to POST tickets to SQL server
- Admins able to GET tickets
- Admins able to UPDATE  tickets while resolving them
- Admin able to view all tickets, both open and closed


## Stretch Goals ##
- Server can respond with data like total number of completed tickets or average time to resolution for a ticket
- Bot can detect if a single user is abusing system and automatically put them in "probation"
- 

## Functional Requirements ##
- Admin can alter permission levels of other users
- Regular user can submit tickets
- Moderators can receive tickets
- Moderators can resolve tickets
- Server can load tickets into the database through API calls
- Server can retreive tickets from the database through API calls

## Data Flow ##
The user will generate tickets, which will then be packaged up by the Discord Bot.
The Discord Bot will then send that ticket out to an API which will store or update the ticket in the database.
When the Discord Bot queries the API the database will return ticket information.
Moderators and Administrators can request the Discord Bot to retreive ticket information
An administrator can send requests to update the user information stored in the database

## Testability ##
The main ponits to be tested are as follows
- Tickets can be sent from the Discord Bot
- Those tickets are stored in the database by the API
- The Ticket can be pulled out and displayed to a "Moderator" user
- The closed ticket object can be created by the Discord Bot and sent back to the API
- "Administrator" users can change the privelege level of other users

## Useability ##
The requirements here are for the console commands to be able to be used for any level of user to input
tickets clearly and concisely.
