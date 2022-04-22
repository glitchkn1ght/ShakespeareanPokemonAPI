# ShakespeareanPokemonAPI
API challenge for TrueLayer

**How To Run**
- Download Visual studio if not already installed. 
- Clone repo from github.
- Open in visual studio.
- Click on green triangle in VS toolbar to build and run in IISExpress/Docker.
- For convenience I've included the Postman collections i used during development. Two are for the public API's the solutions uses and other is for the solution itself
  (You may need to change the port number but should work otherwise). 

**Notes on the General Design**
- Overall my goal was to keep the solution well organized and easy to comprehend with folders/classes/variable named descriptively without being overly verbose.
- Feedback I've had on previous tests noted that my controller was way too bulky, therefore i tried to move as much as possible into different layers of business logic classes. I'm aware these BL classes have slightly awkward names, it's always something i struggle with. 
- Originally each interpreter class had its own response type: PokeApiResponse and FTApiResponse. However each inherited from same Base class and each only contained one property that were both strings. Therefore i decided to combine them into a single ServiceResponse class. This seems cleaner but comes as the expense of the property name being generic e.g. returnedText. 

 **Known Issues/Areas for improvement**
- FunTranslationsAPI free tier is limited to 5 requests per hour which is quite low, especially in terms of testing. Can avoid this issue by using a VPN and changing
  location periodically. 
- Currently the description retrieved from PokeApi is simply the first one in the list that is in the language specified. Given more time i would include parameters that would allow you to you specify which version you wanted.
- I chose to add the language option as although not in the spec it seemed in the spirit of exercise (can't translate to Shakesperean if the description is in japanese) however this can only be set in appSettings when really it should be a parameter. 
- I think the Interpreter classes could be one class with different constructor arguments or at least implementations of the same interface, same for the mappers, first thing i would look at given more time. 
- I am using a library PokeApiNet which is intended as a wrapper for calls to PokeApi net. However calls using the libraries httpClient immediately throw an exception
 if a non success code is received. This seems less than ideal as it gives you less control over the flow so i decided to use a standard HttpClient instead and only used the library for it's built in classes. However this comes at the expense of not having the libraries caching mechanism. Given more time i'd try to incorporate the caching without using their client. 
- I didn't write any integration tests because in my current team we have an automation specialist for it and there didn't seem enough time to teach myself. I have recently been learning PACT testing which i would have included except it's designed to function between projects e.g. a consumer and a provider and the test is only one side of the equation. 
- The unit testing is designed to be illustrative of general understanding rather than completely exhaustive although i did try to be thorough.
- I think i might have been a bit too enthusiastic with the amount of logs included but i've tried to keep them concise and in a format that could be easily read by consumers such as splunk.
- I didn't want to include details of exception in controller responses in order to be mindful of security so kept them to logs only.
- I added the dockerFile and it seems to be working (I can run using docker from visual studio rather than IIS) but i have very little experience with docker having only started learning it recently.
- When run in docker the logs are placed in the wrong folder, not sure how to solve this.
- Not exactly an issue but..unintended extra functionality...you can search by pokemon number instead of name and it works (using Postman at least).  
