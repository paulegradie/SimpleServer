## Simple Server in C#

This project implements a simple HTTP/1.1 server in C# using a TcpListener.

This is a study project to both understand how servers are implemented in C# and understand the mechanics of the C# language. It is currently a WIP and initially follows a three part tutorial video series on youtube found. 

### Instructional content
 - [Part 1](https://www.youtube.com/watch?v=HFnJLv2Q1go)
 - [Part 2](https://www.youtube.com/watch?v=8d5JWDuG2Tg)
 - [Part 3](https://www.youtube.com/watch?v=PPX2V10eQkI)

From this, I will have made additions and modifications including refactorings and extended functionality.

## Progress Details

#### Current Status

The server builds successfully and serves a simple webpage. ;)

#### Next Steps

 - Refactor the response and request objects and figure out why StreamReader.ReadToEnd doesn't work as expected.
 - Link up the get request with a postgress db (later sql server db proper) and display some dummy data
 - Modify the front end to accept text and update the db
 - Provide a means to request updated data and specific data from the db
 - Handle alternative request types
 - Create a duplicate project that uses proper .NET classes to handle requests/responses and the server loop
