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
- Add a particle effects system for dash and when landing on the ground
- Add some post processing effects to elements in the levels
- Apply and organize unique lighting
- Create a new level and finish the current work in progress level
- Overhaul how damage taken works (add period where you lose control)
- Create new second type of enemy with more advanced pathfinding

(need to check on what's needed here / need to finish this section of the readme) 

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



# Running Instructions
- Build and Run to load the scene (reaching the alarm clock will transition between scenes)
- WASD to move
- Shift to dash
- Space to jump
- Mouse controls camera movement
- Losing all health results in reloading the scene with a unique message in the console
- Completing the objective by touching the clock results in the scene reloading from the start
