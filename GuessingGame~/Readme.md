A simple, yet complete, guessing game with Animations and Audio to help demonstrate just how versatile the API is and how it can handle most workflows (even for a Designer).

 It has:
 
1. Animations and Fading.
* We use the Unity **Animator** for buttons on hover as well for the "You won!" text.
* We use **Math.Lerp** to fade between the main menu and the game screen.
2. Callback functionality to demonstrate that its possible to mix styles. This is to demonstrate that the API is buy in... meaning you can start using parts of it without it needing to do any refactor (leave the existing code in place).
3. We show off **Reference** in order to grab Components/GameObjects on Awake/Start.
4. We show off **RootMapping** which is useful for a listing of the same type of heiearchy multiple times (think an inventory in a scroll view. Where you would populate similar entries of Images and Text in a "for loop").
