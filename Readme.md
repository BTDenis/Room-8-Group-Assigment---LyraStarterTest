# SDET Test Task 

## Project Overview

This repository contains an automated test for the LyraStarterGame for Unreal Engine 5.3.2. This automated test was done with the help of the GameDriver API.

## Folder structure

```
LyraTests
├── ProjectMods
│   ├── BPGameDriverClassPawn.T3D
│   ├── BPGameDriverClassPawn.uasset
│   ├── GameDriverClassPawn.cpp
│   ├── GameDriverClassPawn.h
│   ├── UIExtension.Build.cs
├── Tests
│   ├── Input
│      ├── PlayerCharacterAimAtTarget.cs
│   ├── SmokeTests
│      ├── CharacterMouseInput.cs
│      ├── CharacterMovement.cs
│      ├── UIHud.cs
├── GameDriverBase.cs
```

## Smoke Tests

The following Smoke Tests have been implemented:

### 1. Character Movement 
	- Tested the following movements for the PlayerCharacter:
		- Forward
		- Backwards
		- Left
		- Right
		- Diagonal (Specifically, Forward+Right)
		- Jumping
		- Crouching
	- Simulates player movement input by using the "FloatInputEvent". I chose this because I prefer to have the ability to control how much movement should be given toward a direction,
		as this can simulate a Controller Input.
	>>> This is currently keyboard specific. I did not use the Enhanced Input System for this test, but the test does validate locomotion successfully.

### 2. Character Mouse Input
	- Tested the following Mouse Inputs for the PlayerCharacter:
		- MouseInput_LeftClick
		- MouseInput_RightClick
		- MouseInput_RandomMovement
	- Simulates player mouse input by using the MouseDrag() and MouseClick() functions from the API.
	>>> This is currently Mouse&Keyboard only, with the assumption that clicking does something. I did not use the Enhanced Input System for this test, but the test does validate mouse input successfully.
	
### 3. UI HUD Check
	- Tested that the Pause Menu accessibility is functional for the player.
		- Tested that the player can open/close the Option Menu by using the "Back" button in the Pause Menu
		- Tested that the Player can use the "Back" button in the Settings Menu as well
		- Tested that the player can click between the Top Settings in the "Options" Menu.
		- Tested that the HealthUI, ReticleUI and AmmoUI are visible.
	>>> This test could be more thorough for a Smoke Test, but, due to time constraints, I was unable to deliver more.

## Approach for the "Aim At Referenced Object/Actor" Test

My approach to validate that the Player Character could aim at a Referenced Object/Actor was to aim dynamically at a Target Actor that will spawn at a random location in the world.
To achieve this, I have created the following functions and have used them in the same order:


** void SpawnTestActor(TSubclassOf<AActor> ActorToSpawn, float radiusFromOriginPoint) **

	- Spawn a Test Actor (ActorToSpawn) at a random distance (radiusFromOriginPoint = 3000)
	- Will ensure that it is above the ground and is facing the PlayerActor
	- Enable collisions for raycast validation

**Note: From here on, these functions can be used without the above SpawnActor() function as long as the input is correct.**

After an object was spawned (or if one was added via the Editor), we need to calculate the input we need to give to the Player Character.


** FRotator FindRotationToActor(FString ActorName) **
*This function was used instead of the GameDriverAPI's due to inconsistencies.

	- Calculate and return the FRotator value from the Player to the Target Actor.

After we get this Rotation, we can now call the following function by using the LookAtRotation data:


** void InjectMouseInput (float Pitch, float Yaw)**

	- Uses the result from **FindRotationToActor**.
	- Simulates the player Mouse_Look input and injecting it into the Enhanced Input System.

**!**	
	Known Issue
		Using the **InjectMouseInput** function will, as of now, aim sligthly off-set (Top-Right of the target), making it so that LineTrace validation becomes unreliable (~80% times results in a failed test)
**!**


** bool RaycastToTestActor (FString ActorName) **

	- Performs a sphere trace towards the Target Actor to confirm line of sight.
	- Will return true if the target was hit, false otherwise.


** void DeleteTestActor (FString ActorName) **

	- Destroys an Actor from the world.
	- This was used to delete the SpawnTestActor created object.

## Challenges faced throughout the Smoke Tests and Aim At Referenced Actor 

# Smoke Tests:
	- Difficulties with trying to read the actual Player Health and create some basic functionality for testing Health-related events (Damage, Heal etc.)
		- I was unable to resolve this as I was unable to understand how the LyraStarterGame health system works.

# Aim At Referenced Actor
	- Initially took a while to decide on how exactly to perform a test that would meet the requirements to assure functionality.
	- Met difficulties with the GameDriverAPI.FindLookAtRotation function. This function would always return a Vector3 that had one of its values be basically unusable. (The Roll value).
		- Also had some difficulties with some of the Vector3 return values from majority of the functions from GameDriverAPI (Mostly order, where I would have to "flip" the Vector to get the exact values I needed)
	- Enhanced Input System with GameDriverAPI. As mentioned, Vector2DInputEvent did not work with this sytem and so I created a custom function to be able to re-create the Mouse Movement and then Inject it into the input system.
		- Additionally, some modifications were required in the **UIExtension.Build.cs** to make Enhanced Input System work properly.
	- Raycasting a Sphere instead of a Line. I was met with an issue I was still unable to fix where, when using the **InjectMouseInput** function, the aim would always be at the top-right of the created object.
		- This test would fail ~80% of the time, as sometimes the cube would spawn and be "just" on the TraceLine
		- As a work-around, we used a decently small Sphere to aim at the cube. The size of this sphere can be adjusted in the Header file.

### Lyra Project Modifications

The following setup has been used to perform the SmokeTests and the Input test:

** Level used: L_ShooterTest_FireWeapon **

In the Outliner, please modify the **Floor** StaticMeshActor, specifically the **Transform** category.

It should look like this:
** Location: 	(X=0		,Y=0		,Z=0)   **
** Rotation: 	(Pitch=0	,Yaw=0		,Roll=0)**
** Scale: 		(X=150		,Y=150		,Z=1)	**

*Note: It should not matter whether you modify Relative or World. However, to avoid any potential issues, please change the transform data with "Relative" selected.

From the "/ProjectMods/" folder, please retrieve the GameDriverClassPawn.cpp and GameDriverClassPawn.h files and add them to the LyraProject.
The folder structure should look like this:

```
LyraStarterGame
├── Plugins
│   ├── UIExtension
│      ├── Source
│         ├── Private
│            ├── GameDriverClassPawn.cpp
│         ├── Public
│            ├── GameDriverClassPawn.h
```

Additionally, another module has been modified to make the "Aim At Object/Actor" functionality work.
In the **UIExtension.Build.cs**, I have added the following two lines.

```
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
				"CoreUObject",
				"Engine",
				"SlateCore",
				"Slate",
				"UMG",
				"CommonUI",
				"CommonGame",
				"GameplayTags",
				"InputCore", 		<--- New 
				"EnhancedInput",	<--- New
            }
		);
```
This is required to make the EnhancedInput Injector method work.

Next, Please add the .uasset file into the project. The path should not matter as long as the Blueprint can be opened, can be dragged into the level and has working functionality.

# When adding the Blueprint to the level, please name it "BP_GameDriverClassPawn".

In the case that there's any issues, please try and add the GameDriverClassPawn.cpp/.h files to the Unreal Engine in the following locations:

```
All
├── Plugins
│   ├── UIExtension C++ Classes
│      ├── UIExtension
│         ├── Public
│            ├── GameDriverClassPawn.h
│         ├── Private
│            ├── GameDriverClassPawn.cpp
```

*Note: In the case that the GameDriverClassPawn does not show up, please press the "Recompile and reload the C++ code..." from inside Unreal Engine and make sure that the file exists in the project.
*Note: These tests will work in other levels as well as long as GameDriverClassPawn is added, but results might not be as accurate due to the interference of the other LyraBot characters (Especially enemy LyraBots).
