# Immediate Style 
[![License](https://img.shields.io/badge/license-MIT-green)](https://github.com/hopeforsenegal/immediatestyle/blob/master/LICENSE.md)

**Immediate Style** is a convenient style wrapper around Unity's UI components. All the benefits of an [Immediate Mode GUI](https://caseymuratori.com/blog_0001) but without having to give up the power of the Unity Editor Layout System. Layout your UI in the typical Unity way and then, using code generation, immediately start coding response behaviors.

This allows you to interact with Unity UI GameObjects in an Immediate *Style* (similar in API form to an [Immediate Mode GUI](https://caseymuratori.com/blog_0001))

Why only be limited to different "Callback" or "Design" styles?

```cs    
protected void Start()
{
    rightButton.onClick.AddListener(OnAction);
}
private void OnAction()
{
    Debug.Log("Hello World!");
}
```

Give yourself a simpler "Immediate" styles to work with

```cs 
if(ImmediateStyle.Button(CanvasLeftd414).IsMouseDown){
    Debug.Log("Hello World!");
}
```
How many times have you wanted things to be simple... 
For instance to check that the exit button was clicked OR the escape key was pressed with just a single if statement?
```cs 
if(ImmediateStyle.Button(ExitButtond31h).IsMouseDown || Input.GetButtonDown(KeyCode.Escape)){
    Debug.Log("Exit the game!");
}
```
With this API you can (& more!)

<br>

![code (4)](https://github.com/user-attachments/assets/df496928-9f0c-49c2-8e2f-6cb970bc13a8)


<p align="center">
<b>Respond to actions of user input without creating Entire Classes or Writing Function Callbacks - Be FASTER and more Minimal
</b>
<br>
</p>

## Features
1. Has a new component called **Element** that adds what you need to a GameObject for the new ImmediateStyle API to work. Instead of coding manually and then creating your Unity UI Components, we instead create your Unity UI Components and allow you to code generate (No more missing reference exceptions).
* Use Code gen tools to make sure you only spend time coding functionality or doing layout. No more wasting time setting up references in the inspector.
*  Works with every existing Unity UI component (including TextMeshPro) and is compatible with existing Classes, Callbacks, and Callbacks in the inspector for designers (So you trying it out doesn't involve rewriting your entire codebase).
2. Has a new component called **Reference** that is useful for finding game objects (and Debug.Asserts if it could not be found). This is can code generate (to save you time typing) and it makes it easy to obtain a component (For Start/Awake) without having missing references in the inspector.
3. Has a new component called **DragDrop** that is useful for Pills and other more advanced UI functionality.

## Installation

- Add this GitHub URL to your package manager or, instead, in your 'manifest.json' add
```json
  "dependencies": {
	...

    	"com.moonlitstudios.immediatestyle": "https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle.git",

	...
  }
```

None of that working? **Honestly, just reach out us!** (Links & methods towards the bottom).

## Examples
Please look at the [Style Comparisons](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/StyleComparisons~#style-comparison) folder for more elaborate examples that are setup like the real world. (or specifically [Immediate Example](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/StyleComparisons~/ImmediateExample.cs) to hop right into a code example) 
The following is a basic example of using the **Element** component: 
```cs
protected void Update()
{
	ImmediateStyle.SetColor(Color.Red);		     // Coloring the UI works like this (and therefore can be done in a batch)
	ImmediateStyle.Image("-AutoGeneratedGuidGoesHere-"); // Controlling (CanvasGroup|Text) works like this
	ImmediateStyle.ClearColor();
	
	if (ImmediateStyle.Button("-AutoGeneratedGuidGoesHere-").IsMouseDown) {	  // Controlling (Toggle|InputField|Dropdown) works like this
	    // Write your code for what to do when this specific button is clicked
	}

	var value  = ImmediateStyle.Slider("-AutoGeneratedGuidGoesHere-").Value; // Controlling Slider works like this
	ImmediateStyle.DragDrop("-AutoGeneratedGuidGoesHere-", out var dragDrop); // Controlling DragDrop works like this
	ImmediateStyle.FollowCursor(dragDrop.transform);
}
```
The [Style Comparisons](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/StyleComparisons~#style-comparison) folder (and scenes) is your best bet to get your hands dirty. (If there is something you want to try, we are hoping that the examples are comprehensive enough to already have it. We believe UI should be that easy! So let us know if we fell short). We tried to handle every Unity UI component just look [here](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/Runtime/UI/Immediate/ImmediateStyle.cs) to see if we got them all. The Style comparison follow three different styles. [Standard Example](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/StyleComparisons~/StandardExample.cs) follows typical function callbacks that one might see in a Unity tutorial. [Design Example](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/StyleComparisons~/DesignExample.cs) follows setting up callbacks in your inspector (`UnityEvent` On Click) which one might also see in a Unity Tutorial. [Immediate Example](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/StyleComparisons~/ImmediateExample.cs) follows the style of the code you see above. All three types exist for your ability to compare and contrast and determine which style is best for you. Additionally the styles do not overrwrite each other so you can use the Immediate Style in your codebase and it won't affect other parts of your code.
There are more through and elaborate examples with [Immediate Style](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/StyleComparisons~/A_ImmediateStyle.cs) and [Typical Style](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/StyleComparisons~/A_TypicalStyle.cs) if the three intial examples don't suffice.
Lastly we show off a mini Game with animations and more in [Guessing Game](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/GuessingGame~#guessing-game).


The following is a basic example of using the **Reference** component: 
```cs
protected void Start()
{
        background = Reference.Find<SpriteRenderer>(this, "-AutoGeneratedGuidGoesHere-"); // We save you mental anguish and time!
}
```

The following is a basic example of using the **DragDrop** component in a *callback way*  **(you can still do it the Immediate Style way but we provide this way additionally as to not force you to do it "OUR WAY")**: 
```cs
protected void Awake()
{
        dragDrop.OnReleased.AddListener(DragDrop_OnReleased); // Is a UnityEvent so Designers can set OnReleased inspector instead.
}
```

## How does it work?
For **Element**, we just map all the components (in the Awake/Start function) into a Dictionary by a GUID. Then we just set a bool denoting they were interacted with that frame. It really is that simple and makes UI that much more easy to deal with. No more registering/unregistering callbacks in Awake/Start and having scattered logic across 20 files/classes/functions. In fact you don't even have to type that much because we code gen the UI usage code for you.

For **Reference** , we basically just call `FindObjectOfType<>` under the hood. We Debug.Assert for conveinece so you can be alerted right away if you deleted the GameObject from the inspector (normally you wouldn't know you had a missing reference until the component got used sometime later). We then just check for the matching GUID. In a nutshell it is faster/better than `GameObject.Find` since you can't have conflicting names, we only search by component type, and we do a convenient assert.

For **DragDrop**, we just simply make the gameobject that is clicked on follow the cursor (conditionally).

## Need Help or want to chat?
Feel free to just drop us a line on [Discord](https://discord.gg/8y87EEaftE). It's always better to have a real conversation and we can also screenshare there. It's also not hard to reach us through our various other socials. There we can talk about UI testing or about any additionaly performance considerations that you might have with your projects.

## Other Unity Packages
Check out [Asset Stripper](https://github.com/hopeforsenegal/com.moonlitstudios.assetstripper) & [Coordinator](https://github.com/hopeforsenegal/com.moonlitstudios.coordinator)

## Support this project 
Please please please!! ⭐ Star this project! If you truly feel empowered at all by this project please give [our games](https://linktr.ee/moonlit_games) a shot (and drop 5 star reviews there too!). Each of these games are powered by this framework (I'll update this section to include additional games as they start to use this project)

If you have feedback, want to extend the API to handle more components, or would like the API to respond to more event types please feel free to reach out (or create an Issue).

![icon512](https://github.com/user-attachments/assets/85141dc9-110e-4a8d-b684-6c9a686c278b)
[Apple](https://apps.apple.com/us/app/caribbean-dominoes/id1588590418)
[Android](https://play.google.com/store/apps/details?id=com.MoonlitStudios.CaribbeanDominoes)

![appIcon](https://github.com/user-attachments/assets/4266f475-ac9b-4176-9f97-985b8e1025ce)
[Apple](https://apps.apple.com/us/app/solitaire-islands/id6478837950)
[Android](https://play.google.com/store/apps/details?id=com.MoonlitStudios.SolitaireIslands)

![app_icon](https://github.com/user-attachments/assets/13ba91c7-53b4-4469-bdd0-9f0598048a28)
[Apple](https://apps.apple.com/us/app/ludi-classic/id1536964897)
[Android](https://play.google.com/store/apps/details?id=com.MoonlitStudios.Ludi)


Last but not least, drop some follows on the following socials if you want to keep updated on the latest happenings 😊

https://www.twitch.tv/caribbeandominoes

https://www.facebook.com/CaribbeanDominoes

https://x.com/moonlit_studios

https://x.com/_quietwarrior
