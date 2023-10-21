# freecodecamp-org-godot-crash-course-for-beginners

**Please note that, with the exception of those listed below, I do not claim ownership or authorship of any of the graphical or audio assets in this repository. All other assets were downloaded from [ansimuz.itch.io](https://ansimuz.itch.io/sunny-land-pixel-game-art) as instructed by the tutorial that this project originated from.**
  
Assets exempted from the above statement:
- the contents of Heart-Pixel_001 
- the contents of Heart_002
- the contents of Heart_003
- Dmg_Lo-Fi_002.mp3
- Game-Over_001.png
<br /><br />

**I am not affiliated with freecodecamp.org.**
<br />

### About
This is a variation of the project created by the tutorial located [here](https://www.youtube.com/watch?v=S8lMTwSRoRg&ab_channel=freeCodeCamp.org). The most obvious change to this version is the use of C# instead of GDScript. Unfortunately, I have not kept track of the other differences until now, as I originally had no intention of making this repository public. Given some of the obstacles and opportunities encountered due to my choice of language, however, and the various changes made as a result, I thought it might prove helpful for anyone attempting to learn Godot using C#. Below is (I think) a complete list of changes from the original project. I will add additional changes and details as I create or come across them.

  Differences:
  - The project uses C# instead of GDScript.
  - The number of hit points provided to the player has been reduced from 10 to 3.
  - The hit point UI has been modified to use icons/sprites (i.e. heart containers) instead of label text.
  - Enemy/Player contact and damage handling has been modified as follows:
    - Enemies now only damage the player when contact is made from the bottom or sides.
    - Enemies now only take damage from the player when contact is made from the top.
    - When an enemy damages the player, the player will temporarily lose control and be pushed back.
    - When the player lands on an enemy, they will bounce.
  - The world is different from that provided in the tutorial.
