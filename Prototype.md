# Prototype
Our prototype journey has been really messy and all over the place due to us making last minute decisions all the time and encountering roadblocks, so ended up working on many things unnecessarily 

We tried working on many different solutions but at the very least, they are all within the theme of Diabetes management apps

## First Iteration using Power Apps
### Details
For the first iteration, we wanted to try to connect to db and have it track glucose levels, we planned to have several pages with some charts for glucose levels, health indicators, carb logging and notifications to be sent given specific levels of the health indicators... We did not manage to make a mock or connect it with other application because we found the tool not flexible enough to implement the solution. We were not able to control the flow for this reason we scrapped this method. ❌

We tried working on notifications for diabetes management apps like reminders to turn on sports mode using Power Automate, but without an app to connect to, we scrapped this as well. ❌

### What we used
- Power Apps 
- Power Automate (for notifications)
- Power BI (for dashboards)
- Microsoft Dataverse 

## Second Iteration using Maui
### Details
For the second iteration, we wanted to try connecting a CGM to an app and have it track glucose levels, essentially like the loop app, you have charts for glucose levels, active insulin, insulin delivery, active carbs etc, insulin overrides, carb logging... We did manage to make a mock CGM and connect it to anoter app to view glucose levels, but it was too complicated even when implementing the chart alone and the algorithm so this was scrapped. ❌

### Tools and technologies used
- .NET MAUI for Mock CGM and App
- .NET 7
- Azure SignalR Service (to act as a messaging hub)

## Third Iteration using Maui
### Details
For the third iteration, we just wanted something doable and so we just focused on the carb logging/meal planning feature but using a bot as a twist. We made another app from scratch, the app just has homepage to show quick stats, a meal manager to save commonly eaten meals for easier logging and a schedule page to log/schedule meals.

Bot Implementation
- The bot was hard to implement, we started out with a normal bot and just focused on it handling a command to add meals, tested on the Bot Emulator and it worked so we deployed it. ✅ 
- Then we tried to integrate LUIS into our bot, we were able to implement it but due to incorrect training, our LUIS model was not performing good enough, so we removed it. ❌
- Then we tried incorportating Speech into our bot and made it a virtual assistant using direct line speech, tested out on the Windows Voice Assistant Helper and it worked out well. ✅
- Then we tried integrating it into the app itself but the only example we had was using WPF, MAUI didn't have the right packages for playing audio from a speech service stream, so we couldn't integrate it in ❌

### What we used
- .NET MAUI
- .NET 7 
- Cosmos Db
- Azure Bot 
- Azure App Service
- Azure App Service Plan
- Azure Language Service 
- Azure Speech Service 
