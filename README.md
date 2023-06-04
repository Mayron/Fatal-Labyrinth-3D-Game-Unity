# About
I developed this 3D Game using C# in Unity for the purposes of education only. I used free to use assets from the Unity store, as well as some custom assets created in Blender. All the C# code was written by myself as the sole developer.

# Game Overview

Fatal Labyrinth is a first person, single player, survival-horror game where the object is for the player to escape the labyrinth by battling monsters in close combat. There are two types of enemies: skeletons and gatekeepers. Each level has a portal that leads to the next scene but is locked away behind an iron gate and can only be unlocked by slaying a gatekeeper (also known as a knight). Unfortunately, gatekeepers are very strong and should be tackled once the player has reached a higher level, unless of course you are able to block and dodge your way around them skilfully. Both the gatekeepers and skeletons drop health and experience points when killed so it is advised to kill as many skeletons are you see fit to level up. Each time you level up your damaging attributes and health increases as well as other enemies becoming weaker. However, the gatekeepers will patrol the labyrinth and if they find you, they will chase you until you outrun them. As the player, it is up to you to choose your fights carefully and be prepared to fight off hordes of skeletons that you may rush into at any moment.

# Implemented Features
For the entire game all you have is a torch to light your way through a very atmospheric environment, and a sword which can both be used to attack and defend yourself. At the bottom of the screen is a health and experience bar as shown below. You only have one life in order to create the perfect survival horror experience so be careful!

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/a7344db1-bd9f-4a11-87e8-fa91db309de0)

Animations have been created without the use of assets for multiple sword animation clips. This includes a blocking animation, a sword swinging attack animation, and another for when the player gets stunned. When an enemy attacks the player while they have their sword raised up to block an attack they become stunned. This decision was made to avoid the player feeling invisible while blocking attacks as they only get to block one attack before becoming vulnerable. The player also has the ability to run by holding the shift key but only when they are not currently blocking attacks. If they player is running and then raises their sword to block an incoming attack, then their movement speed will be reduced back down to walking speed.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/72c4cd9b-a53f-4b54-85cd-21f513574e9f)

# Spawning Skeletons

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/8a624142-ef1f-40f2-9e56-7d40bcc4cfb2)

The above screen shot shows red lines to represent the formation of a group of skeletons (using Unity’s debugging, draw line function). Skeletons always work together in groups of 5 and guard their spawn point. When the player comes close to a skeleton, the skeleton will chase them but this does not cause the entire group to chase begin chasing them as well. There is a limit to the number of groups of skeletons that can exist on the current level’s map at any one time and these spawn areas are random. Spawn areas have been predefined but are not all active so there is some variation between where groups of skeletons can be located. When an entire group of skeletons has been killed a new group will respawn in another spawn location, however they will never respawn in the exact same place that the last group of skeletons died; This is to avoid skeletons spawning on top of the player. 

Each skeleton also has an evade radius where if they are lured by the player too far away from their spawn point they will retreat. During this evading phase the skeleton cannot attack or be attacked until they have been repositioned at an appropriate starting point.
The “SkeletonSpawnControl” script controls the spawning behaviour of these groups by storing a List of “SkeletonGroupController” objects. Each group of skeletons has its own unique class to control the positing of each individual skeleton so that they do not class together. Below is the update function of the “SkeletonSpawnControl” class which checks to see whether an entire group of skeletons has been killed, and if so a new group will be created and placed on the map. A list is stored in this class to hold all of the active groups. To avoid synchronisation problems, a temporary array is created to hold these groups when searching through them.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/b1b2bbfb-48d1-45b5-9324-be801806d6d7)

To avoid skeletons within the same group being spawned on top of each other, the function below assigns them a randomised distance from the spawn point within a predefined radius and then rotates them from a random range between 0 and 360.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/ec60abb5-d6ee-46cb-847c-17085a3a728f)

# Levelling and Statistics System

To make managing a levelling system for the player easier, rather than scattering combat related properties around multiple classes, one class holds all of them called the “Statistics” class which has been added to the Main Camera. This allows developers to see an entire overview of all the level design attributes all in one place so that tweaking of values is much simpler. It also makes it easier to alter values depending on the level difficulty.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/4c1dfd18-45dd-4a93-9f53-57fb45bedd9b)

This function is very large as it contains all of the combat data for the game. To add variations of damage, many combat attributes have been introduced such as giving both the enemies and the player the chance to deal critical (increased) damage, as well as parry and dodge attacks. From these data sets you can see that the skeletons and gatekeepers drop different numbers of pickups when killed depending on their level of difficulty. The health and experience point pickups are represented by glowing particles which move towards the player’s location if the player is clock enough to them, making it easier for the player to collect. The health and experience points bar is then increased automatically when the player collects the pickups.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/3194e930-345b-48cb-a14b-fcf9b1036ad4)

# Gatekeeper Enemies

Unlike the behaviour of the skeletons, the gatekeeper relies on ray casts to detect the player. They also use a lot more animations and have a much larger FSM’s for their animators. Again, these FSM’s has to be created from scratch to fit into the game. All scripts and FSM’s belonging to the prefabs had to be removed, however some animation clips were reused. The only animation clips created from scratch were for the player’s sword animations. Most animations rely on triggers to trigger animations to only play once. Boolean values are used to store different combat states as well as changes from walking to running.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/63beb95f-4068-40e1-b5da-8ab4875feec7)

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/51ad1ef2-1e91-4041-b31b-7e3a5949570c)

# Combat

Rather than creating a large script to control the entire behaviour of each enemy the scripts have been broken down into smaller ones and stored inside their own folder. Each of the two types of enemies share similar design patterns. The controller script controls movement and decides when the enemy can attack the player. Once they are in range and have equipped their weapon or their cooldown timer for their main attack has expired, a Boolean variable is set to true to notify the combat script that it can run the update function, allowing their combat functionality to begin taking effect.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/86caed86-baff-4bd7-80c4-f678236adcf8)

oth enemies behave very differently as the gatekeeper’s behaviour is far more complex. The gate keeper has 2 different attacking animations and must switch between 3 types of movements (walking, combat walking, and running). Once either of them has been killed they will play their death animation, collapse to the floor, and then be destroy by the Unity entire after a set time limit rather than disappearing straight away.

# User Interface

⚠️ The UI was kept very simplistic due to time constraints, as well as not being the main learning objective of the project. Most of my efforts were spent towards learning Unity and perfecting the pathfinding/collision detection.

There are two playable levels, a starting menu, and a splash screen that separates each of the scenes. Each of these scene’s fade in from black:

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/a800fee1-b08b-491c-863d-c0ad56bc8e5c)

An options menu is available from the start menu by pressing the options button. It is also during game play by pressing the escape key which also pauses the game at the same time. Unfortunately, the key bind menu does not work correctly due to issues with “PlayerPrefabs” not working correctly. This bug still needs to be fixed.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/41909795-6ba7-4ca7-a8ce-133da4c87ac9)

The start menu plays a camera navigation animation to take the player on a virtual tour of the labyrinth before the game begins. There is an option to continue from the last save point, create a new game, open the options menu, or quit the game. To quit the game while playing you first have to go back to the main menu.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/12887442-1110-43bf-9cd0-fc9d8a4dcc87)

# Game Manual

As the player, your only objective is to kill the gatekeeper to unlock the gate and enter the portal to get to the next level. The gatekeeper will be much stronger than you so first you should kill skeletons to collect experience points as they are much weaker. Below is a list of actions you can perform with the player. You can only block one attack and must wait a few seconds until you can block a second attack so try not to run into a pack of skeletons too hastily. Also you cannot run and block attacks at the same time.

The first level involves traversing a labyrinth so you will need to do some initial exploring to find out where the portal room is but try to avoid fighting the gatekeepers too soon. The second level is outdoors in a death match-like arena where hordes of enemies will be chasing you. This game is still in early development so it is very simplistic.

You must kill enemies for them to spawn experience and health orbs (pickups) around their body which you can then collect by running over them. When you are close to them, the orbs will begin to move slowly towards you in case you cannot read them due to obstacles blocking your way.

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/95dac6f8-b52d-4019-829d-b0e397dd644e)

# Acknowledgement of Assets Used

All assets were from the Unity Assets Store and were free to use. No scripts from any of the prefabs downloaded were used. Most animation clips were used from these packages, however no FSM animators were used and all transitions between animation clips were remade.

- 10 Low-poly medieval models: https://www.assetstore.unity3d.com/en/#!/content/30639
- Decrepit Dungeon LITE: https://www.assetstore.unity3d.com/en/#!/content/33936
- Fantasy Monster - Skeleton: https://www.assetstore.unity3d.com/en/#!/content/35635
- KY Magic Effects Free: https://www.assetstore.unity3d.com/en/#!/content/21927
- Medieval props: https://www.assetstore.unity3d.com/en/#!/content/41540
- Overlord: https://www.assetstore.unity3d.com/en/#!/content/48768
- Simple Torch: https://www.assetstore.unity3d.com/en/#!/content/7275
- Ambient Sample Pack: https://www.assetstore.unity3d.com/en/#!/content/376

![image](https://github.com/Mayron/Fatal-Labyrinth-3D-Game-Unity/assets/5854995/fed038f4-b777-469c-bbcd-2e9c222cf7af)
