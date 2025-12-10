# Shepard of Dreams

## Team Members

Caroline Jia, Benjamin Albeyta, Sophia Qian

## Game Summary

A game, where you play as a Sheep and progress through dreams trying to wake up a resting dreamer. Progress through levels jumping through different obstacles trying to reach the alarm clock at the end of the stage.

Early Concept Sketch:
![Concept Art](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreig4f2yvxduzlcot7hqc6tedirmyhjp55fe5embpgj4jjtrur6rn7u@jpeg)

## Genres

3D Platformer, adventure, action.

## Inspiration

### [Super Mario Galaxy]
Inspiration in terms of general game structure, level design, controls and movement. Being a level by level linear structure with different environments throughout. The general level design of a linear 3D platformer is largely where our inspiration from Mario Galaxy comes from; along with the variety of moves that Mario has while exploring a level and how those can lend themselves to more enjoyable gameplay and level design. 
![Mario Galaxy Level](https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmEhjqVIlFUWoa-AjDfDzlizQlJyPAhHEBug&s)
![Mario Galaxy Controls](https://cdn.mobygames.com/covers/1295072-super-mario-galaxy-2-wii-reference-card.jpg)


### [A Hat in Time]
More inspiration in terms of movement, general momentum, level design as well as having more creative objectives and concepts on a level per level basis. Inspiration in terms of what can be done with a similar base set of mechanics to what we have.
![Hat in Time Example](https://i0.wp.com/operationrainfall.com/wp-content/uploads/2017/10/Trainwreck.png?ssl=1)


### [Animal Crossing]

Animal Crossing is a social simulation game with anthropomorphic villager characters. The player spends time collecting items, catching bugs and fish, and customizing their island. Its cute and dreamy, yet semi-realistic art style influences our design. Additionally, we will take inspiration from its functionalities to interact with many objects in the game. 

![Animal Crossing 1](https://i.pinimg.com/736x/48/68/c5/4868c51d2714f3116e361be1fe81deb1.jpg)

![Animal Crossing 2](https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/ncom/software/switch/70010000027619/9989957eae3a6b545194c42fec2071675c34aadacd65e6b33fdfe7b3b6a86c3a)


### [Monument Valley]

Monument Valley is a puzzle and indie game by Ustwo Games. The player leads the princess Ida through mazes of optical illusions and impossible objects while manipulating the world around her to reach various platforms. We will take inspiration from their environmental art and puzzle level design, which matches our dream setting. The general aesthetic and style is what we're mainly drawing inspiration from here. 

![Monument Valley 1](https://design-milk.com/images/2014/04/caledonia.jpg)

![Monument Valley 2](https://einfogames.com/reviews/files/2014/07/Monument-Valley-Gameplay.jpg)



## Gameplay

- User interface: fireflies representing current amount of health (currently shown as orbs rotating around the player)
- Enemies to avoid representing insecurities / other things of the psyche
- To pass the level you must find certain objects
- Third Person Camera controlled by the mouse
- Walk and move around with WASD and the arrow keys
- Shift to run increasing base movement speed, horns appear when dash is available, disappear when on cooldown
- Use space to jump
- Sheep faces cursor or walking direction

## Development Plan

### Project Checkpoint 1-2: Basic Mechanics and Scripting (Ch 5-9)

- ~~Set up items to represent characters and objects in the game~~
- ~~Implement basic movement including walking and jumping~~
- ~~Implement a camera perspective~~
- Draft rough designs and storyline
Wasn't seen as a priority and so the focus was instead on making sure that the base gameplay and structure was properly implemented.
- Create basic layout of inital levels
Instead created a basic test level, just to make sure that colision, enemies and all other implemented aspects worked properly, most notably player movement.
- One view-change potion effect
Decided this wasn't a priority comapred to getting the baseline movement and mechanics implemented.
- ~~Implement one advanced movement option~~ The dash

### Additions

- Created a health system with floating 3D objects that circle around the player
- Created pickups to restore health
- Created visual indicators of the dash and when it can be used

### Project Part 2: 3D Scenes and Models (Ch 3+4, 10)

- ~~Create a level with terrain with a terrain tool or Probuilder, so that the world is not a flat plane.~~
- ~~Implement 3D models with complete meshes and textures for important objects like your player, an enemy, and key objects in your environment. Remember to cite!~~
- ~~Implement your view change potion and associated effect.~~
- ~~Instead of restarting the level, have the win condition load into a different scene that may be less polished than the first.~~
- ~~Iterate on movement physics, especially the floaty jump and the option to vary your jump height based on hold length. Implementing slightly more control over the jump will suffice, as we are not looking for professional platformer physics here.~~

### Additions
- Created squash and stretch systems for main character movement
- Created a system for moving platforms
- Created a Death plane object prefab
- Added the ability to wall jump as well as temporarily cling onto walls
- Added some basic animations for items and collectables
- Implemented a dropshadow for aiding the player in terms of perspective.

### Project Part 3: Visual Effects (Ch 11, 12, 13)
- ~~For improvement, add a visual indicator for invincibility frames to make player feedback clearer.~~
- ~~Adding a few extra environmental details could make it feel more complete, like some unreachable floating islands in the distance that is slightly more “terrain-like” and less like a flat plane.~~
- ~~Add post-processing for ambient tone and visual cohesion for your dream-like mood stated in the GDD.~~
- ~~Include particle effects for movement, like dust during wall jumps.~~
- ~~Refine lighting around the level to bring more depth and focus to the environment.~~
- ~~Add period where you lose control after being hit~~

### Additions
- Updated moving platforms so they carry momentum properly
- Added particle effects for landing after jumps and for moving around
- Slightly revised first level structure
- Created prefab for object cubes that can be used
- Changed hitbox on cloud platforms so easier to read
- Revised View Change potion so it instead spawns a platform

### Project Part 3-4 (Sound, UI and Animation): 
- ~~Fix the camera falling below the terrain on the starting platform.~~
- ~~For post processing, implement an effect (e.g., chromatic aberration or similar override) when the player dashes or gets hit.~~
- ~~Add player movement animations, such as running and jumping.~~
- ~~Add a burst of particles and a sound effect when a platform appears. (Later on when we complete the cutscenes chapter, you can use Cinemachine to create a short camera cutscene to highlight the revealed platform.)~~
- ~~Since your enemy floats, add particles or visual cues (like a subtle gust of wind) to show why it’s floating.~~
- ~~For UI, create a main menu, pause screen, restart option, and a clear way to reset once the end of the level is reached.~~
- ~~Include SFX for footsteps, jumping, landing, and getting hit.~~ (Note, decided to add sound effect on the enemy rather than the player, having both lead to a more messy soundscape so decided to only keep the one for now) 
- ~~Add at least one background music track for your levels.~~

### Additions
- Created an Audio mixer to control sounds
- Made the enemy have a unique animation and sound effect with particles for when coming into contact with the player
- Added sound effect for finishing the level (collecting the Clock at the end)
- Added sound effect for collecting heart for health
- Added unique dash trail effect on using the dash
- Added particle effects to health icons so easier too see

###  Project Part 4: Finishing Touches
- ~~Update running instructions~~
- ~~Work on optimization and reducing size (removing uneeded files that have been imported and etc)~~
- ~~Work on Level 2 and making it more complete (textured and etc)~~
- ~~Possibly work on creating a level 3~~
- ~~Complete a fully playable start-to-finish game loop, including all planned levels and a final win condition, and ensure players can return to the beginning. This is essential for your WebGL build.~~
- ~~Add at least one element of UI juicing, as you mentioned in your Project Part 4 plan. Could be anything from transitions, subtle scaling, to opacity changes.~~
- ~~Add one additional juicing element of your choice to any part of the game and include it in the GDD (e.g., camera shake on impact, a unique tween for a new mechanic in one of your currently unimplemented levels, etc.).~~
Specifically created a new system with the collected potion, triggers a countdown timer for objects that appear and dissapear on a timer. 
- ~~Update your UI to use a non-default font and replace the default Unity textures.~~
- ~~Test your WebGL build early to catch any web-specific issues before the next submission.~~
I have tested it and it seems to work

### Additions
- Updated pause so it also stops the music that's currently playing.
- Added additional music
- Added victory level with ability to return to main menu

### Final Project Submission
- ~~Ensure the aspect ratio works correctly in both 16:9 and 16:10.~~
- ~~Add a delay between dying and restarting so sound/visual effects aren’t cut off early.~~
- ~~Add a sound effect for picking up the potion.~~
- Have disappearing platforms use a fade-in/fade-out transition instead of popping in/out.
Decided not to have them fade in and fade out, I tried it but it was very unclear when specifically they were in and out because of the ever present transparent platforms showing where to jump.
Decided instead to make them flash when about to dissapear.
- ~~Make the platform-appearing particles more obvious, and include a small camera shake for added impact.~~ Implemented but only for level 1, as wouldn't fit with the other levels which had their own changes to how the platforms appear, screenshake would cause its own problems if it was that constant in those levels
- ~~Add a visual indicator for the remaining wall jumps counter.~~
- ~~Make the third level significantly easier to complete. A good metric is all team members should be able to complete it reliably, not just one.~~ 

### Additions
- Added a death UI that you are taken too after dying with options and unique music
- Implemented sillouettes that show where the platforms are going to spawn for levels with those as elements.

## Development

### Project Checkpoint 1-2:
Our work for this checkpoint mainly consisted of establishing base movement mechanics, systems and getting used to collaborating on github, generally building a framework for future systems to build on.
- #### Basic Movement, Collision and Test Map
Basic movement and collision is implemented through the use of the Unity New Input system and rigidbody objects. The playermovement is implemented in the PlayerMovement.cs script and uses the ShepardofDreams.inputactions to interface with the Unity New Input System. While movement could admittedly still be refined, it is currently in a functional state and includes the ability to jump as well as a dash ability both of which are also implemented in PlayerMovement.cs and use the Unity New Input System for their implementation, the dash is shown visually to the player by the horns, they appear when the dash is able to be used and upon use dissapear until it can be used again.
![Dash Ready](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreibojomqlnsgpwyuajlkrotmhei523ly6apcledaf3frj4rd3bygfq@jpeg)
![Dash Used](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreicfgbzsrj5mxqoojo2hc65zphednm5hyknuuobdus4z7grklgyl2u@jpeg)

The current base test map, is very simple; it was just a stage to test the movement as well as any interactions between the player unit and both enemies and game objects, for this purpose it served very well and allows us to test these systems before we work on wider implementation in a completed level.
![Test Level](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreie7652xdrbesead5wv6tntcjxvo4icwkrxb32jnk5lsjcyvjbqgya@jpeg)

- #### Camera Implementation
Implementation of the camera was tricky, mostly due to a combination of factors involving us not intially knowing what type of camera to use. Ultimately a third person controlable camera was selected because it was easiest to implement and fit best into the game. The camera uses the New Input System which requires the Main Camera to be a child of the Player Unit object, as before we ran into issues with it being seperate and therefore having two player input systems which were conflicting with each other. Currently the camera works very well and has been sucessfully implemented with no issues. The code for the camera implementation is in the ThirdPersonCamera.cs script.
![Camera Perspective](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiaq6sxzlmc2cjh3ajker4lmbcxkhwkebf7yl32fnp4e7t25kisdii@jpeg)

- #### Health system
Created a health system, both to track the amount of hits the player can take, but also to create systems that allow for a loss state, interactions with enemies and health recovering items.
The main aspect of this system is in PlayerHealth.cs, which contains all elements relating to the Player Unit itself, it also creates 3 subobjects with no collision that rotate around the player as a way to represent current health, every time you take a hit you lose one and if you have none and take a hit, it triggers a "player died" debug message and game over. When getting hit the player is pushed back and given a brief moment of invulnerability to avoid being instantly hit again. 
![Full Health](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreidnizphuvfvfgjefhc6ycimutuaizug4a255onpd6otp54wvshe4y@jpeg)
![Taken a hit](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreic7lw5nfzrzt5kr2vs6vxr325j5wu7n6j55e5zkkmvdsnzprsduui@jpeg)

Also implemented a collectable orb that refils the players health if collected, scripted with HealthPickup.cs
![Health Pickup](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiaznmwoakvpqx7pwoz2yq7fuzzsnm36zv444ux7jpfxrgck46mmea@jpeg)

- #### Basic enemy template 
Created a basic enemy template, represented by a pill with a cube on its head. The enemy has a basic AI where it walks back and forth between two GameObjects (Point A) and (Point B). Enemy attributes and behavior controlled in Enemy.cs and EnemyPatrol.cs
![Point A](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreigamp5ckdrpnxgnhy47uqg2rbfc4v3wb6ksmxquqyjmc5x26rgziy@jpeg)
![Point B](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreia7kvwm5hmgle2jed5tugeeruqghy347b5fyvsaqrxwafbj3lqtcq@jpeg)

- #### Win and Loss conditions
Created a game state manager that runs and controls the win or loss conditions GameManager.cs
Created checks which cause the scene to reload upon fufilling conditions for winning or losing, for losing this comes in the form of losing all of your health where upon a debug message "player died" will display in the console and the scene will reload. 
![Game Over Example newly respawned](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreif2oaig6rwpxfmqhm7byghjyvrhdfvi6j6ajkn6nkkj4ekstafi6e@jpeg)

There is also the object for completing the level, a rectangle that appears red initally, and upon coming into contact with it, turns green and resets the scene, DoorGoal.cs
![End Level Object Red](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreifmni2sbb7ps76wozewqh7hnza65kdq7aactfn6ltr5hlxmb3cfdq@jpeg)
![End Level Object Green](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreia4u3yjdqtzgpgsm2ww7qxuwqxrvb6xpssiajbwbyzd65p3bl6p4u@jpeg)


### Project Checkpoint Part 2:

- #### Implemented 3D Models with Textures
For the main character there is a custom 3D model made with it's own accompanying texture 
![Main character texture model](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiejqb5gsxmh5hpdry37dmq6la33fmadjrsny7d5xrm7nb5hl6fmzu@jpeg)

For objects, the "door" to end the level was changed to an alarm clock:
![Clock/Door](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreido7abevwwzrfmkia6i6mutnar7b6vtoctmurw2ewokmsqquhbw3u@jpeg)
The health pickup was made a heart:
![Health pickup](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreifmqkfkq6vkkqpowiymbihb2i4l7ztwxcmqtu327ltuvdrnvubfmi@jpeg)
the floating health indicators geometric rhombuses:
![Health indicators](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiacep5elljg32sgtrgxtbntnb7ne3ej3xjyysr3gk7fd63r5v6qje@jpeg)
and the view change potion was created as a potion:
![View change potion](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreidb22me3tks43t7edj5qscwyuazb7nlafnox32muc2t3fk25dobku@jpeg)

All of these models were taken from the BTAM Simple Gems and Items Ultimate Animated Customizable Pack; this pack also included a script for having the items rotate, float and scale, which I used to make the items appear more lively in game and to make the clock have it's own unique little pseudo animation when interacted with to symbolized the end of the level, which is contained within DoorGoal.cs
https://assetstore.unity.com/packages/3d/props/simple-gems-and-items-ultimate-animated-customizable-pack-73764?srsltid=AfmBOoqZdxiQJteFKui1fd9VJjrpzbojcStyqMv8w2jnBsIbMm9LCYg8

The enemy model, for the cactus dude, was taken from the Models Resource, a free use archive of models.
https://models.spriters-resource.com/playstation/finalfantasy8/asset/286689
![Cactuar](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreifgbmsb7ffmb2j7ayjsyhoasixnncl2lsddj2snhxxd7aceenuota@jpeg)

- #### Implemented View Change Potion and Associated Effect
Created the view change potion which when taken changes loaded textures in the level, the TextureChanger.cs is placed on the items who have their textures changed and TextureChangeItem.cs is placed on the view change potion itself.
![Before](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiftyk43yixgtmw2qfo2f6dp3ouyznylslabkj63bcl5e2kr4of65e@jpeg)
![After](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreigtb7zembpo7zpfavf3pl2mcige3jl5aandwge4g275l6ymh2hypa@jpeg)

- #### Iterated on platformer movement and jump height
Rewrote the player controller multiple times trying to get movement to feel just right, was eventually able to get it to a satisfactory level, max movement speed determined by an equation that raises drag the closer to maximum speed you are, jumping retains player control while not speeding them up or slowing them down in the air, variable jump heights based on how long the button is held was implemented through changing the player input system and instead of jump being a button press it was set to a button hold and release so that the input action could tell when the player let go of jump. Jump was made significantly less floaty due to the addition and reivsion of custom gravity initally it's own script but was eventually implemented into PlayerMovement.cs, upon initally rising up gravity is lowered before steadily increasing after the apex of the jump, until reaching the ground or the max gravitational value. 
<video src="https://github.com/user-attachments/assets/327a79d2-bc12-4805-a426-3a0890471530"></video>

In addition new abilities were added, specifically a wall cling and a wall jump. The wall cling is done by checking vectors for in front and to the sides of the character and checking for specifically marked walls, if there are some then the gravity is temporarily significantly reduced, while in this state a wall jump is then possible a certain set number of times until again touching the ground.
<video src="https://github.com/user-attachments/assets/6f207148-bd2d-4a62-b551-74e001fa48b8"></video>

- #### Added player dropshadow
A common feature in 3D platformers, a dropshadow allows the player to see where they're landing, this was accomplished by creating an all black texture on a plane, then removing the plane's collision and making it follow under the player at whatever level is directly below them as determined by a vector; this is contained within the ShadowProjector.cs script.
<video src="https://github.com/user-attachments/assets/95d58a8a-5f3b-404f-b5c1-a747cc5f8975"></video>

- #### Created a System for moving platforms and a death plane
Just basic aspects of any platformer that I felt were necessary to have at least as options, the death plane simply instantly calls the player death when made contact with and is contained within DeathPlane.cs.
<video src="https://github.com/user-attachments/assets/4f4c5832-f0d0-4b96-95eb-80e3db57e307"></video>

The moving platform follows the same basic structure as the enemy prefab, moving between two set points on the scene as determined by Point A and Point B. Contained within MovingPlatform.cs
<video src="https://github.com/user-attachments/assets/40066146-d93f-4ab6-9d7e-64f3feccd538"></video>

- #### Added player squash and stretch
Inspired by the premade script from some of the used textures, created a script that changes the player model based on squash and stretch when jumping, landing from a jump and dashing. Was easy to implement from the side of PlayerMovement.cs because those checks prexist, and simply called on the newly created PlayerSquashStretch.cs. One issue that did arise was squash bringing the player slightly into the air because it would shrink them based on their origin, to solve this I used the point for Feet that already existed as a part of the ground check and made the model a child of that, so when the feet were transformed it applied that as the origin of transformation to the model iself resolving the issue.
<video src="https://github.com/user-attachments/assets/dd4f4a92-3911-4071-902b-4f436a67c72c"></video>

- #### Created levels & means to move between them 
Created levels, using prefabs and probuilder which the player transitions between when reaching the goal, currently only 2 levels. If there are no remaining levels currently there is an option to loop back to the first level that is ticked by default, the level migration script is in GameManager.cs.
![Level 1](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreic7qdijjaeqjyhjo2kfb5bq7ypi23wwezjlpkjsalrs64lqhz7gzu@jpeg)
![Level 2](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiborzm6riqarrsbbp6dht3myb3hkya6xamp23n2fkecrxg3enjsku@jpeg)

### Project Checkpoint 3:
- #### Made moving platforms correctly carry players momentum
Updated the script for MovingPlatform.cs so that it carries the players momentum more, by when the player makes contact with the platform it makes the player a child of the platform temporarily.
<video src="https://github.com/user-attachments/assets/37240734-7b75-482f-a32e-38f481dc8c1b"></video>

- #### Changed textures and added terrain for background of level 1
Revised the texture and added terrain in the background so the level feels more complete and dreamlike in terms of its structure.
![Level 1 Revised](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiftoq7jvb7e4qj6ctxx2y4nmuipr6td3mqbapqj34os26yvnaox3u@jpeg)

- #### Added flashing effect during invincibility period and a script for a lockout period after being hit
For the sake of the player being more easily able to view the period, implemented in the PlayerHealth.cs script, attach the model in the inspector and flashes it for a period of time, by getting all rendered objects attached to it and changing their visibility. Changes located in PlayerHealth.cs.
<video src="https://github.com/user-attachments/assets/229488e5-a1d0-4c33-ac5a-d4f113abf4de"></video>

- #### Added Custom Shader for player
Created a custom cell shader for the player, specifically changes shading based on specific sections of the player, improves performance and makes a generally interesting and cartoony look. Currently only applied to the player texture, might be applied to others later on and is in PlayerMaterial.mat
![Player Cel Shading](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreibxzrbctqglym7shqygstenlrhqjgmz2j3wyloj4ccmcfnfm56xci@jpeg)
![Player Cel Shading Graph](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreifj3ma5wg2nrg6nl73dviff2lbmfplw2p5gjfagyug33joecsdoy4@jpeg)

- #### Added Custom fullscreen shader for enviorment and objects
Fullscreen shader that creates an outline on every visible object in the scene. Thickness of outline depends on distance from camera and from player. Located in FullscreenOutline.mat.
![Fullscreen shader](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiadjrm6iqq4w72o6v7igksjzjrhzloldoacilo3q3q3szxtrwuxre@jpeg)
![Fullscreen shading graph](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreigtmanvbwtdl5k5zxvsapmyi4bhvp3qlrgdkydxepbenm6hhga4iu@jpeg)

- #### Added Lighting Effects and prebaked lighting in level with static objects
Made most of the level geometry static, including the terrain, only the moving platform and objects aren't static objects for increasing performance, also generated the lightmap which is located in the Level1 folder and edited the lighting settings to be generally lower resolution to add to the dreamlike quality of the game. 
![Level1 Lighting](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreibqdma4a52tq55nm7myaipfl7os77gubgxv2hwz5acihkg5p36ozq@jpeg)

- #### Added post processing effects (shadows midtones, chromatic abberation, bloom)
Effects were added and used in SampleSceneProfile.asset, added some bloom, changed the coloration of shadows and midtones and a small amount of added chromatic abberation.
![Level1 Post Processing](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreienhrdtyjni3mcvugqpmzmnyfahm46spx2b7rd6yiswg3rpy4ctee@jpeg)

- #### Added prefab for platforms
Created a prefab for square platforms where the sides are walls that can be walljumped, while the top is ground.
![Platform prefab](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiftyqa5fwi3xuhmeket5q2sqzzvvkhxsl4yqlzjdcbxcj37gpnbzi@jpeg)

- #### Added particle effects on running
Created a custom mesh and then used that mesh to be spawned via particle system Feet Dust attached to the player object. Spawns when moving on the ground through logic and checks in PlayerMovement.cs
<video src="https://github.com/user-attachments/assets/8abb312f-fa8d-4106-bce7-b776a52a4044"></video>

- #### Added particle effects on landing after a jump
Used that same custom mesh but smaller to spawn particles via particle system Landing particles attached to the player object, spawns upon landing on the ground through updates to PlayerMovement.cs
<video src="https://github.com/user-attachments/assets/4551f1a8-7062-416e-8e1d-f555ded9d958"></video>

- #### Changed hitbox on cloud platform prefab
Revised the hitbox on the cloud platforms just so that they more properly match the player and don't result in as much weird collision.
![Cloud platform](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreiepulxxpen4cle6a76co3punfrpsd6mvzgcsupkuvo5vcpkwhgq4u@jpeg)

- #### Revised view change potion
Revised the view change potion so that instead of changing view it spawns the moving platform, made it so that it interacts with a Level Event object that can be used to control objects through the Texture scrips, changes implemented in TextureChanger.cs and TextureChangeItem.cs
<video src="https://github.com/user-attachments/assets/6acfb37f-0ab5-4150-881a-b3c435bfb4ee"></video>

### Project Checkpoint Part 3-4:

- #### Fixed camera falling below level
Changed the minimum distance camera can go (changing the value for the negative axis) so that it can't fall underneath the starting platform.
<video src="https://github.com/user-attachments/assets/46b76996-d521-416f-bbc4-bdcafb36e4e6"></video>

- #### Added animations and animation controller
Created an animation controller PlayerAnimator.controller and imported custom made animations for Dash, Jump, Idle, Walk and taking Damage (Armature_Jump.anim, Armature_Idle.anim, Armature_Dash.anim, Armature_Walk.anim, Armature_Damage.anim) Then updated the playermovement script so that it called on the variables establishes in the animation controller to ensure that the animations worked, also made it so that the speed of the walking and dash animations was controlled by the speed of variable keeping track of movement speed so it would appear more natural.
![Animation Controller](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreifk4dotahs44x7yrmmyq3qx2676vfns4ijxnsbeisvwmsxzlbfnpm@jpeg)
<video src="https://github.com/user-attachments/assets/713e55de-83d1-42a5-a157-777a716fd458"></video>

- #### Added Particles and sound effect for platform appearing
Sound effect is taken from FreeSound.org, particle effect is an altered version of the one used for landing; implemented in ViewChangePotion.cs
<video src="https://github.com/user-attachments/assets/a6f4719e-8979-420a-9079-ab6859dfe177"></video>

- #### Added particles for enemy floating as well as for enemy making contact with the player alongside a sound effect
Added the sound effect and animation to the Enemy.cs script, created 2 different particle effects for the enemy as well and made the audio 3D spacially aware on the attack. Also added a post processing effect that triggers increase in chromatic abberation upon getting hit by an enemy, implemented in Enemy.cs.
<video src="https://github.com/user-attachments/assets/8466b43f-3fd5-415b-b1fe-4661c1d83c99"></video>

- #### Created Audio Mixer to control sound balance
Audio Mixer overviews the current sound effects, music, player sfx and ambiance and their balance; currently no ambient sounds but wanted to include the slider regardless. 
![Audio Mixer](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreigjaagpngivl2ze4g2becus6okcawd4vq2arkybya2gtmc3x2nole@jpeg)

- #### Created Main Menu
Only has one option currently which is to start the game, and has it's own unique background music, taken from FreeSound.org, specific citations in file name. Actual design is still very placeholder. Start button implemented in StartGame.cs
![Main Menu](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreigslxk7xa4w7n5thwy5tc4d4onyxhwc7o22kaa4y6mnxmmrxpmkj4@jpeg))

- #### Created Pause Screen
Pause screen has options for Resume, Exit to main menu and Restart Level. Pause implemented in Pause.cs, Exit to main menu in ReturnToMenu.cs and Restart level in ResetLevel.cs. Pause by pressing p.
![Pause Screen](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreigqm54p5xr2sasxpwiez5ybeijrcaj22isxhrsv7xswmue22o3lnu@jpeg)

- #### Included Background music
Background music by Elektrobear, is 2D so that the music doesn't fade even with an increase to distance 
<video src="https://github.com/user-attachments/assets/09866a8f-ed19-49c6-8e0c-81ae27a8402a"></video>

- #### Added player sound effects
Footstep sounds were taken from the unity asset store, there are 6 different ones, each time a footstep is called it randomly selects one of the 6 options. Called using events triggered in the animations for the character.
For the landing, dash, and jump sounds they are taken from FreeSound.org, specific citations in file name and called in PlayerMovement.cs; there is also a sheep baa sound effect also taken from FreeSound.org which has a 1/10 chance to happen when jumping or dashing, also implemented in PlayerMovement.cs
<video src="https://github.com/user-attachments/assets/7f4e7ad7-35df-4554-abb6-60013db8fd80"></video>

- #### Added sound effect for clock
Sound effect taken from FreeSound.org, specific citations in file name; implemented in DoorGoal.cs
<video src="https://github.com/user-attachments/assets/258db1d5-d93c-4393-9e3f-ccdaff9b753f"></video>

- #### Added sound effect for heart
Sound effect taken from FreeSound.org, specific citations in file name; implemented in HealthPickup.cs
<video src="https://github.com/user-attachments/assets/be4fd808-73c4-4449-be51-baace74ba7d8"></video>

- #### Added particle effects to health icons
Created particle effects around the heath icons so that they are easier too see and won't get lost during gameplay
<video src="https://github.com/user-attachments/assets/ff633041-30c5-457d-aa33-931d720fc557"></video>

- #### Added unique Dash effect
Not achieved by using the particle system, instead in PlayerMovement.cs, when the player uses a dash and during that dash coroutine the game looks at the player's mesh, makes a copy of it in the exact position it was in with a different texture (transparent blue) and does this every certain amount of frames during the dash. 
<video src="https://github.com/user-attachments/assets/94d1562c-862e-4235-9b91-60e7aaccca81"></video>

### Project Checkpoint Part 4:

- #### Created a Third Level
Created a new third level with it's own unique mechanics, terrain background, textures, and music.
![Level 3](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreih6m2mvmdcyvyql7kz6luncxrbuuaxwybyki2hgiipil5twzw7nhu@jpeg)

- #### Created a new main menu
New main menu with a new button and custom fonts.
![Main Menu](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreicillzrb5uaggoqiumcgszdsvpzycwbscx5rljb4up6zec3mw3xe4@jpeg)

- #### Created a system for the buttons expanding when you hover over them for UI juicing
Contained within ButtonHoverAnimation.cs, uses DG.tweening which is a package downloaded from the Unity Assset store.
<video src="https://github.com/user-attachments/assets/193afb95-778f-4fbe-bcfd-0b04cfa342ca"></video>

- #### Updated the Pause Menu UI
Updated pause menu ui to use the expanding buttons as well as different custom button items instead of the built in ones with Unity. 
![Pause Menu](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreie33ymjlljmkieouyokhyod62zgui6h6kf73sqlrxq4hd47sk7k2m@jpeg)

- #### Created a new system with potion for dissapearing reappearing platforms, with sound effects
Implemented in PotionPickup.cs and AlternatingObjectSets.cs; this way it's a variant on what the potion does that's more interesting from a gameplay perspective and allows for more unique level design. Works by having a blank game object with the script for AlternatingObjectSets attached, then add objects to Set A and B in those object sets, and have the object containing AlternatingObjectSets.cs be referenced by the potion which contains PotionPickup.cs. Each object does have a particle effect and a sound effect as well as a countdown sound effect for timing up when the next set is going to appear.
<video src="https://github.com/user-attachments/assets/219b655f-4a35-4110-bc61-b39c66272409"></video>

- #### Finished texturing the second level
Created a new terrain for background with it's own textures, slightly revised the design of the second level and textured all objects in the level.
![Level 2](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreic3iqr7kfaqiudivp5dlmyf3ai5adyijtoa4lbcwk7mkfzzlvzwrq@jpeg)

- #### Created a victory scene
Created a victory screen the GameManager.cs now transitions to the victory screen when the player completes level 3, the screen allows the player to go back to the main menu and start playing again.
![Victory Screen](https://cdn.bsky.app/img/feed_fullsize/plain/did:plc:gw3cmasus5q2obg274yrww7u/bafkreicdn2doiy7spsplvpqwlqnndqbbhoposnmsjni62z5q4wenqrffoi@jpeg)

- #### Updated pause so that it also stops music and sound effects
This way it properly lines up even after pausing, with the sound effects saying when the objects are appearing or dissapearing. Accomplished through minor tweaks to Pause.cs, ResetLevel.cs, and ReturnToMenu.cs.
<video src="https://github.com/user-attachments/assets/9fa3cce9-6df7-4fc6-906c-930d387b52f0"></video>

# Running Instructions
- Build and Run on the main menu to start 
- WASD to move
- Shift to dash
- Space to jump
- Mouse controls camera movement
- Losing all health results in reloading the scene with a unique message in the console
- Completing the objective by touching the clock results in moving to the next level / scene
- Pressing start on Main Menu loads into first level, can return to menu at any time from pause menu, press 'p' to pause
- Pause menu also allows you to return to the main menu at any time
- when all levels complete goes to a victory scene
