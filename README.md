# Overview
This application lists the countries in order of decreasing population density. Users also have the ability to search for any country they want.

# Working
Users need to first run the WebAPI server `DHBTestApplication.API` in the `https` profile by the command:
```dotnet run --launch-profile https```

# Future Improvements:
1. More comprehensive error handling and propagating the error to the UI throuh the `error.razor` page.
2. More comprehensive logging.
3. Using debouncers when typing in the country names. Ideally we should wait for atleast 300ms so that the REST Server is not inundated with calls after each key press.
4. Comprehensive unit testing.

# Bugs:
When typing in a country name, the search does not take place real-time but when focus is moved away from the textbox.

# Watch out for:
1. Comments for bug fixes start with the word "Fixes".
2. Comments for Feature start with the word "Feature".
