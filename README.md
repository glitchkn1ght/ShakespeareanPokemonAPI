# ShakespeareanPokemonAPI
API challenge for TrueLayer


**How To Run (Debug)**
- Clone repo from github
- Open in visual studio
- Click on green triangle in VS toolbar to run in IISExpress.

 **Known Issues/Areas for improvement**
- FunTranlsationsAPI free tier is limited to 5 requests per hour which is quite low, especially in terms of testing. Can avoid this issue by using a VPN and changing
  location periodically. 
- At the moment the description retrieved from PokeApi is simply the first one in the list that is in the language specified. Given more time i would include parameters
  that would allow you to you specify which version you wanted.
- I chose to add the language option as although not in the spec it seemed in the spirit of exercise (can't translate to Shakesperean if the description is in japanese)
  however this can only be set in appSettings when really it should be a parameter. 
- I think the Interpreter classes could be one class with different constructor arguments or at least implementations of the same interface, same for the mappers.
