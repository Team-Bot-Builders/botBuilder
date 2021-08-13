# Bot Builder

> This solution is a bot created for midterm projects at Code Fellows for 401 in .Net.

> [Live Server Demo](https://ticketbotapi20210812160408.azurewebsites.net/index.html)

## Table of Contents

* [General Info](#general-information)
* [Technologies Used](#technologies-used)
* [Features](#features)
* [Images](#images)
* [Setup](#setup)
* [Usage](#usage)
* [Project Status](#project-status)
* [Contact](#contact)
* [License](#license)
* [Other Docs](#links)

## General Information

* Create a Bot for discord that allows members of a specific server add tickets with problems to be resolved.
* Those tickets will be added to a restful API server.


## Technologies Used

* C# 9.0
* ASP.NET 5.0
* Discord.Net
* Entity Framework
* Identity Framework

## Features

### Bot Features

* Can respond to User via [!]help command.
* Will take input from Users to create a help ticket, and store each ticket in a database.
* Can request and change data within database.
* Is set up with Authorization and will only allow users with specific roles to access specific data.

### API Features

* Tracks all tickets, both live and resolved.
* Keeps records of all users and their roles.
* Allows administrators to update users, user roles, ticket data, etc.

## Images

**Domain Model**
![Domain Model](./images/DomainModel.png)
**ERD**
![ERD](./images/ERD.png)


## Setup

In order to run this you will need to pull down the entire github repo and publish the API to the web (we used Azure). Follow directions from Discord Documentation and the YouTube tutorial listed in resources to get your bot connected to your Discord Server.

## Usage

The Discord Bot handles ticket requests. You can use it on a large or small server to receieve as a way to handle and organize tickets. The bot will take a problem description from the User, and than acquire that users name and time of request. Moderators will have access to all open tickets and will have the ability to close them, but onl admins can view closed tickets and delete tickets from the record.

## Project Status

Project is: Complete

## Contact

Authors

* [Joshua Haddock](https://www.linkedin.com/in/joshuahaddock/)
* [Steven Boston](https://www.linkedin.com/in/steven-boston/)
* [Charles Bofferding](https://www.linkedin.com/in/charles-bofferding/)

feel free to contact any of us via the above links.

## License

This project is open source and available under the [MIT License](./LICENSE).

## Links

* [Project Pitch](./ProjectPitch.md)
* [Team Agreements](./TeamAgreements.md)
* [User Stories](https://github.com/Team-Bot-Builders/botBuilder/projects/1)

### Resources

* [RestSharp](https://restsharp.dev/)
* [Discord Bot Docs](https://discord.com/developers/docs/intro)
* [YouTube Bot Tutorial Playlist](https://www.youtube.com/playlist?list=PLaqoc7lYL3ZDCDT9TcP_5hEKuWQl7zudR)
* [Deployed API](https://ticketbotapi20210812160408.azurewebsites.net/index.html)