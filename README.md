# Radio-Duck-Global-
GOTY 2024

The basic character under Prefabs -> Characters -> KnightPivot is not the GameObject with the Animator, there is a Child GameObject under it that contains the Animator. Make sure that any new character has a structure like this:

Empty (Character's Pivot):
  (Contains the collision, triggers, character movement, Rigidbody)
  Actual sprite (Visual Element)
    (Only the animator)

----------------------------------------------------
Creating new character movement with Scriptable Objects!
----------------------------------------------------

To create custom movement for each character, under the Scripts folder in the Asset Manager, then go to ScriptableObjs, and right click + go to the top to create a new Scriptable Object. Each trait the ScriptableObject has is explained below:

  **Gravity**
  Fall Gravity Mult: The scale the character will experience Gravity when falling normally
  Max Fall Speed: The maximum "velocity" the character will fall, basically like reality

  Fast Fall Gravity Mult: When you press down after reaching the apex of your jump, this allows you to fall faster. In this case, this is the new multiplier to gravity
  Max Fast Fall Speed: Likewise, this is the maximum "velocity" the character will fast fall

  **The Run**
  Run Max Speed: The maximum "velocity" the character will run at
  Run Acceleration: The rate of change velocity will experience positively to reach max speed
  Run Deceleration: The rate of change velocity will experience negatively to reach 0

  Accel in Air: The multiplier to run acceleration when the character is in the air, either from jumping or falling from running
  Decel in Air: The same but for deceleration
  
  Do Conserve Momentum: A bool that allows velocity to "lerp", making the character experience inertia

  **The Jump**
  Jump Height: The maximum height, in Unity units, the character will reach when the jump button is pressed
  Jump Time to Apex: The time it takes to reach the Jump Height

  **Both Jumps**
  Jump Cut Gravity Mult: The scale the character will experience Gravity after pressing the down arrow, "cutting" the jump
  Jump Hang Gravity Mult: The scale the character will experience Gravity after reaching the apex, "floating" in the air
  Jump Hang Time Threshold: The amount of time the jump hang gravity multiplier will apply for
  Jump Hang Acceleration: The multiplier player acceleration will occur when hanging in the air
  Jump Hang Max Speed Mult: The multiplier player max speed will occur when hanging in the air

  **Assists**
  Coyote Time: The buffer of time after leaving a platform that the player can still jump
  Jump Input Buffer Time: The amount of time that the jump executes for
