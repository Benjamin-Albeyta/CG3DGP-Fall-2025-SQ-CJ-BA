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

- Create basic layout of a first real level using primatives
- Continue to refine and improve the control feel (possibly implement a new move)
- Get 3D models with textures for important objects (enemies, end level objective, player, Health pickups)
- Implement a view change potion effect (more reasonable if working with textures and more defined geometry)
- Create more of a game loop, transitions from level to level, a real game over and victory screen between levels

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

# Running Instructions
- Build and Run to load the scene (currently everything contained within one test scene)
- WASD to move
- Shift to dash
- Space to jump
- Mouse controls camera movement
- Losing all health results in reloading the scene with a unique message in the console
- Completing the objective by touching the object results in reloading the scene and the object turning green
