# Diabot
Diabot is a carb logger app to help diabetes patients manage their carbs and glucose level but with a twist, they get to interact with a personal assistant bot, we are taking advantage of chatbots to make carb logger apps more user friendly and accessible

<img width="533" alt="image" src="https://user-images.githubusercontent.com/56083944/210423544-e6381543-148d-4789-bb8c-ea8847cf52f3.png">

https://user-images.githubusercontent.com/56083944/210425222-048835dd-ff67-4c96-b224-034fa3e718df.mp4

# What is in this repo
### Diabot
- Our bot application which allows us to interact with our meal database using a core bot

### Diabot Client
- Our MAUI app which has the meal manager and scheduler

### CarbLoggerService
- An api to do crud based operations on our meals database and schedules database 

### ~~MockHumanCGM (Irrelevant)~~
- A mock continuous glucose monitor / mock human which can send glucose levels over a signalr server

### ~~SignalRServer (Irrelevant)~~
- A signalr server which hosts hub that the diabot client can connect to to get glucose levels 

## Technologies used
### App
- .NET Maui
- .NET 7
- CosmosDB

### Bot
- .NET 6
- CosmosDB 
- Azure Bot
- Azure App Service 
- Azure App Service Plan 
- Language Service (LUIS)
- Speech Service 

## Team
### Team Name: 
DiaBot
### Team Members: 
Kenji Phang, Javeria Ehsan, Maryem Benali,  Eleonora Criscione
### Project type: 
Prototype Sprint - Healthcare

The process of prototyping typically will be developed in five main stages:

*  Defining the vision: creating an overarching vision for the product and answering key questions such as what problem it solves, who the target market is, and what the anticipated price point will be. This helps determine if it is viable and if the cost of creating a prototype is justified.

*  Focusing on key features: rather than trying to make the prototype identical to the final product, we should focus on one or two key features to test in the prototype. This helps keep the prototype simple and avoids wasting time on unnecessary details.

* Production

*  Testing and refining: after the initial prototype is created, it should be tested and evaluated to identify any potential issues or areas for improvement.

*  Presentation: The final stage of the prototyping process is presenting the prototype this helps gauge interest in the product and provides feedback that can be used to refine the design.

The whole goal of this project is to try building with Azure and .NET, and expose myself to as many technologies/services as possible, so this project is going to use a lot of overengineered services that wouldn't make sense to use in a real application. 
