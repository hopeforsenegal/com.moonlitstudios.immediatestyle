# Control Structures
The most important thing in programming are control structures. It's how you tell the computer what to do, when to do it, and how many times to do it.

The most important two concepts in this paradigm are branch/jumping and looping/iterating.
ImmediateStyle is built with these in mind.
Below, we go over "If" and "For" and in this readme we try to plainly demonstrate these control structures.

It's important to reiterate that these belong in the Update() function and that ImmediateStyle functions are intended to be called every frame.

### If Example
```cs  
if(showing_button){
	// The button renders and can take click events
	button_clicked = ImmediateStyle.Button("Button").IsMouseDown;
}else{
	// The button doesn't render nor can it have click events
}

if(showing_image){
	// Only when showing_image is true does this image show
	ImmediateStyle.Image("Image");
}

if (showing_drag_drop) {
	// Only when showing_drag_drop is true does this object follow the cursor
    var hasDropped = ImmediateStyle.DragDrop("DragAndDrop", out var component).IsMouseUp;

    if (snap_back) {
        if (hasDropped) component.transform.position = component.PinnedPosition;
    }
}
```
___
### For Example 
```cs  
	// There are a few ways to deal with a set of the same thing 
	// (like in GuessingGame.cs with the listing of previous answers)

	// Sometimes the quickest way might be to just give unique ids to all things and just 
	// iterate over the ids
	string[] unique_guid_list = new[] {
        "Unique1a3e",
        "Unique29af0",
        "Unique352d5",
        "Unique4b866",
        "Unique58c27",
        "Unique6ba83",
        "Unique7405f",
        "Unique8efa3",
        "Unique9ce93"
    };
    for (var i = 0; i < unique_guid_list.Length; i++) {
        var guid = unique_guid_list[i];
		ImmediateStyle.DragDrop(guid, out _);
    }

    // Other times we can use a RootMapping to allow all guids to be the same 
    // but differ by a predetermined prefix
    var prefix = "prefix";
    const int number_of_same_count = 10;
    for (var i = 0; i < number_of_same_count; i++) {
        var guid = $"{prefix}{i}Same";
        // This component (well.. really all of these components) needs a to have a RootMapping. 
        // Each Rootmapping then specifies an "ID" which we use as the index.
        // We suggest using "Using Sibling Index for ID" in particular. 
		ImmediateStyle.DragDrop(guid, out _); 
    }
```

If you are still confused then perhaps 
https://caseymuratori.com/blog_0001
will be better at explaining on what the intended goal is here