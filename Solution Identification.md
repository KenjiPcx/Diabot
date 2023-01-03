# Solution identification
## Here are some of the solutions we identified in month 2: 

### Using Azure ML to predict glucose levels and optimize glucose delivery
- We started with Azure Machine Learning (ML) since it can be used to fine-tune the predicted glucose levels for individual users and retrain the model periodically with user data. This can help improve the accuracy of the predictions and make the insulin delivery plan more personalized and effective.
- We couldn't find the right data so we did not went with this idea

### Using Azure Computer Vision to estimate carbs from foods
- Then we proceeded to research Azure Computer Vision. It can be used to identify the types of carbohydrates in food pictures, which can be useful for carb planning.
- Basically, a user would just have to take a picture of their food and it would log carbs for them.
- But we couldn't find the data again so we did not went with this idea

### A solution which did not need data for model training
- We wanted something doable, without the need of existing data, so we just focused on carb logging
- We decided to go with a simple app that can allow users to save their favourite foods and also schedule them for carb logging
- We then thought of making this app more accessible with Azure Bots, introducing a virtual assistant that can be used to interact with the app and provide summaries and other information to users.

Other options we could have implemented if there was the time were: 
### Integration with Azure Maps Services
- Azure Maps can also be integrated into the app to remind users to record their carbohydrate intake and provide other helpful reminders, 
such as when they are in a gym to turn on sports mode for their insulin pump or when they are in restaurants for a while, then they would be reminded to log their carbs and other places where they may be more likely to need to manage their diabetes.

### Integration with Azure IoT Hub
- Finally, an Azure IoT interface can be added to connect the app to continuous glucose monitors and insulin pumps, allowing for even more accurate
and personalized management of diabetes.
