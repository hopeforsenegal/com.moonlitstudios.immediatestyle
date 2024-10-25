# Style Comparison
Here we are just showing the stylistic code changes and workflow changes that is entailed in different UI workflows.
First... A quick recap

### Design Example
This is how a **designer** might interact with UI based on a Unity tutorial such as this
https://learn.unity.com/tutorial/creating-ui-buttons
___
### Standard Example 
This is how a **programmer** might interact with the UI based on a Unity tutorial such as this
https://docs.unity3d.com/2022.3/Documentation/ScriptReference/UIElements.Button-clicked.html
or this (for older versions of Unity)
https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.Button-onClick.html
___
### Immediate Example
This is the *new way* that we provide you to interact with the UI. Less steps. Less code. Straightforward "if statements" if you desire.
___
Below, each step is approximately what you would do in order to achieve the functionality that you see in those scenes. We've broken it up into the roughly same comparible steps and functionality. You can now evaluate which method or style best fits your needs. (We do recommend opening up the scenes and give them a spin after reading the steps in case you are not familiar with one of the styles)

**Design Example**

Step 1.

<img width="600" alt="Screenshot 2024-10-10 at 9 47 09 AM" src="https://github.com/user-attachments/assets/89e64f6f-22c5-413e-9ec5-28790bfdb92a">

Step 2.

<img width="600" alt="Screenshot 2024-10-10 at 10 01 03 AM" src="https://github.com/user-attachments/assets/cf7bc61c-7b64-46a5-8643-16f35c0ee0cc">

Step 3.

<img width="400" alt="Screenshot 2024-10-10 at 9 53 40 AM" src="https://github.com/user-attachments/assets/26426c9a-4ed9-4367-b56f-dfbefa6e38f2">

**Standard Example**

Step 1.

<img width="600" alt="Screenshot 2024-10-10 at 9 47 09 AM" src="https://github.com/user-attachments/assets/89e64f6f-22c5-413e-9ec5-28790bfdb92a">

Step 2.

<img width="600" alt="Screenshot 2024-10-10 at 9 52 53 AM" src="https://github.com/user-attachments/assets/be85909f-0059-4a61-a9ad-dc0875aaba3d">

Step 3.

<img width="200" alt="Screenshot 2024-10-10 at 9 52 20 AM" src="https://github.com/user-attachments/assets/05a69e45-96ad-4d1b-9065-24c01c2f1cc8">

**Immediate Example**

Step 1.

<img width="600" alt="Screenshot 2024-10-10 at 9 47 09 AM" src="https://github.com/user-attachments/assets/89e64f6f-22c5-413e-9ec5-28790bfdb92a">

Step 1a. (This is step 1a because you haven't left the editor and is a simple click to get the generated code)

<img width="300" alt="Screenshot 2024-10-10 at 9 47 29 AM" src="https://github.com/user-attachments/assets/fe88d97d-dd7a-4ee4-a3aa-9f83e333152b">

Step 2. (The thing to note is that we don't need to get back to the editor and "set references")

<img width="600" alt="Screenshot 2024-10-10 at 9 48 01 AM" src="https://github.com/user-attachments/assets/d277dbb7-29eb-4522-b3c9-904c56a47e7c">

___
From here you can now hit **Play** without getting missing reference exceptions and get the responsive UI funuctionality that was intended.

Note: Remember you can mix all three style if you would like. See [Guessing Game](https://github.com/hopeforsenegal/com.moonlitstudios.immediatestyle/blob/main/GuessingGame~) if you would like to see that in action
