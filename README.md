# freecodecamp-org-godot-crash-course-for-beginners

**Please note that, with the exception of those listed below, I do not claim ownership or authorship of any of the graphical or audio assets in this repository. All other assets were downloaded from [ansimuz.itch.io](https://ansimuz.itch.io/sunny-land-pixel-game-art) as instructed by the tutorial that this project originated from.**
  
Assets exempted from the above statement:
- the contents of Heart-Pixel_001 
- the contents of Heart_002
- the contents of Heart_003
- Dmg_Lo-Fi_002.mp3
- Game-Over_001.png
- collectables/collectable.mp3
- BasicWater_top.png
- BasicWaterFill.png
- pixel-water_01_002_base.png
- pixel-water_01_002_top.png
- pixel-water_01_003_top.png
- The non-system Bitcount Mono font was designed by Petr van Blokland and sourced through Adobe Creative Cloud. It can be found here: https://fonts.adobe.com/fonts/bitcount-grid-double
<br /><br />

**I am not affiliated with freecodecamp.org.**
<br /><br />

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
    - Among the various differences are a moving platform.
    - The assets included in the world have also been extended by a few of my own to incorporate (very rudimentary) moving water.
  - The collectable UI is different from the one in the tutorial and incorporates sprites, etc.
  - The goal of the game is now to collect all of the cherries and reach the end of the level. Upon doing so, a congratulations screen loads.
    - The collectables UI will flash if there are still collectables.
  - A sound will play each time a collectable is picked up.
    - The sound will increase in pitch each time.
  
  Changes since last update:
  - A collectables system has been added, including changes to the UI.
  - Some items have been renamed/moved for better organization.
  - The world has been completed, including the addition of a proper water texture and a moving platform.
  - A full completion state has been added, dependent on the character collecting all available collectables and reaching a goal.

The game is now, technically, complete, with full win/lose states, etc. It does contain a couple of small bugs/insufficiencies that will not be fixed for reasons explained below:
  - Jumping into an enemies damage hitbox can potentially cause the character to fly into the air until they leave the screen and die. Think the Skyrim giants glitch, but in 2D. It should be noted, that this is not due to framerate dependent physics, but rather poor design. I go into more detail regarding this below.
  - Due to the way the player character was originally laid out, certain things are very difficult to animate. This is why the game contains no celebration animation, and part of why the completion state is so underwhelming. Candidly, I can't recall if this was my own decision or if it originated from the tutorial. Again, I go into more details regarding this below.
  - The player's position always starts at 0,0. I have no clue why this is. This seems like it may be some obscure glitch with Godot, as when I save and reload the world scene, the position resets there as well. It's the same issue described here: https://ask.godotengine.org/149615/unable-to-save-sprite-position-in-the-editor That is literally the only mention of it I found, however, and there's no fix listed. Candidly, this seems rare, has very little to no noticeable effect, and fixing it would not seem to be a good use of one's time.

As mentioned above, these will not be fixed due to the amount of work involved in what was ultimately meant to be a small educational project. I feel the project, as-is, served its purpose well.

That being said, there are some valuable design lessons to be learned from the various mistakes listed above. Here is what could have been and should be done differently next time:
  - The design of the code is, frankly, bad. Some of this is due to the conversion of the original tutorial's GD_Script into C# methods, and GD_Script appears to be a much "looser" language in terms of style and syntax, though admittedly I have very little experience with it. That being said, some of this is also due to my own poor decisions early on. In particular, object behaviors are strongly interdependent in a way that breaks encapsulation and is ultimately responsible for at least one of the above issues: The "flying" glitch mentioned above is, most likely, ultimately due to enemies having far too much control over the player's movement, when all of that should be handled by the player object itself. There are also the traditional "I'm lazy and this project isn't important, so let's make this field public" design failures, which can simply be addressed by using proper OO design and making full use of properties going forard.
  - In general, the layout and nomenclature of the resources in the project should be better, including the naming and layout of the nodes in the project's various scenes. This is largely due to the way the project was originally approached in the tutorial, though, admittedly, I continued it.
  - Next time, it would be good to make the player character a child of a Node2D if possible. As-is, the player character being the parent of their own scene makes some things incredibly difficult to animate. For example, you can't change the player's position reliably, because their transform is the global transform by which all other nodes in the scene are set, so any sort of "cutscene", like a simple dance upon completing the level, is prohibitively difficult for a project of this scale. I'm sure there are other ways around this, but simply making the player character a "neighbor" of the animation node would seem to be the be a better design and the easiest way to address this issue.
  - The save algorithm provided by the tutorial works as a proof of concept, but, unfortunately, little else. Godot has newer, "proper," saving incorperated via serialization. If I do ever come back to this, it would likely be to incorporate this sort of save system as as test.

  Once again, the amount of work to address the above issues makes them impractical for this type of project, so I'm going to consider things more or less "complete" for now and move on to other things. Hopefully someone finds this helpful at some point!