# BattleShipGame
This project is supposed to be for CV. It is supposed to show the flow of my Git and how i code.\
BattleShipGame is backend and frontend designed to create sessions and playing battleships\
Backend docker-compose file contains adminer for better database management\
There's 2 entities : User, Session.

## Instruction:
### Backend
To properly launch backend you need to have docker\
-Open "BattleShipGame.slnx" file using Visual Studio\
-Switch startup profile to "docker-compose"\
-Click the launch button
### Frontend
-Download from release section "Frontend release vX.X"\
-Make sure backend in docker is running\
-Launch program using "BattleShipGame_Frontend.exe"\
-Register account, login and create game\
-To properly simulate two players game, open second program with
different account and join game

###

## Endpoints:
### Users
POST /api/user/register: register a new user based on username and password \
POST /api/user/login: user login (returns JWT token)\
GET /api/user/name/{name}: get user details by name\
GET /api/user/id/{id}: get user details by id\
GET /api/user/current: get user details by JWT token (authentication required)\
DELETE /api/user/: delete user by JWT token (authentication required)\
PUT /api/user/: update user details by JWT token (authentication required)

### Session
POST /api/session: create a new session (authentication required)\
POST /api/session/place: place board in session\
POST /api/session/shoot: shoot a tile in session based on coordinates\
GET /api/session: get a list of available session\
GET /api/session/{id}: get session details\
GET /api/session/join/{id}: join as guest to existing session\
DELETE /api/session/end: delete session

    
