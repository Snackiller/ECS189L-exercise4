[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/UID5I7-h)
# ECS189L Programming Exercise 4: The Factory Pattern

# TODO #
Change Capacity to either MaxCapacity or CurrentCapacity.  
Update coding style

## Description

The goals of this programming exercise are:
* to gain experience with the factory software design pattern
* to create significant portions of game programming systems from scratch or lighter outlines
* to implement tables and equations (as given by designers) as game procedures and logic

### Grading
The points distribution for the stages totals 70 points and can be found in the table below. The remaining 30 points are for your peer review of another student's submission.

| Stage | Points |
|:-----:|:------:|
|  1    |   15   |
|  2    |   15   |
|  3.1  |   15   |
|  3.2  |   10   |
|  4    |   15   |


### Due Date and Submission Information

The due date and submission information is on the official class communication channels.

## Description of Provided Unity Project 

### Scene and GameObjects in the Hierarcy

* **Main Camera** is the main viewport into the lab.
* **Laboratory** is the parent for the `GameObject`s that compose the testing facility's laboratory. Inside there are the sides of the room, test pedastals, a weapon on the right (camera-relative), and a shield on the left. 
  * **ShieldPedestal** contains a `ShieldPoint` where the shields (i.e. the test subjects) spawn and an instance of the `HealthBar` prefab.
  * **WeaponPedestal** holds the test weapon located at `WeaponPoint` which contains the `ProjectileSpawn` location for new `Projectile`s to spawn at.

### Assets and Scripts

Prefabs of note:  
* **HealthBar** is a prefab that implements the dynamic health bar shown above the shield.
* **Projectile** is a prefab for the projectiles shot by the test weapon.

Scripts: 
* **ShieldController.cs** contains the game logic for test shields. It is where the refreshing capabilities of the shield should be implemented. It contains references to the `ScrollingText` and `HeathBar` instances.
* **EffectType.cs** holds the `enum` for the weapon projectile damage types.
* **ManualFire.cs** implements firing a `Projectile` instance that moves from the `ProjectileSpawn` toward the `ShieldPoint` on `jump` input.
* **ProjectileController.cs** is a component of the `Projectile` prefab that holds the physical parameters of the projectile and has information on the damage the projectile should do to a sheild.
* **ProjectileMotion.cs** is a component of the `Projectile` prefab that controls its motion.
* **HealthBarController.cs** controls the animation and behaviors of the `HealthBar`.
* **ScrollingText.cs** partially controls the upward-scrolling damage values that appear when a `Projectile` collides with a shield.

Assets used by the project that you need not manipulate but are otherwise interesting:
* **ScrollingText** prefab and animation controller.
* **TextScrollUp** animation.
* **Consequences** is the font used for the damage display text.

## Stage 1: Shields Online

The Aegis prototype shield mechanism is nearly complete. Your task is to complete the shield's functionality by adding a recharging mechanism which consists of the following:
* Use the `RechargeRate` and `RechargeDelay` properties of the `ShieldController` class to enable:
  * After the shield has taken damage or is not at maximum capacity, it enters a recharge delay period lasting `RechargeDelay` seconds.
  * After the recharge delay period, the shield begins to recharge at `RechargeRate` capacity points per second.
  * If the shield is damaged before it is fully recharged, the shield stops recharging and enters another recharge delay period.
  * Shields cannot recharge greater than their `Capacity` value.
  * Shield `Capacity` can never be negative.

## Stage 2: Damage Engine

Create a `DamageEngine` class that implements the following combat algorith:

`damage = ProjectileDamage * (TypeFactor)`

| Damage Type Factors |         | Shield Type |        |        |
|:-------------------:|:-------:|:-----------:|:------:|--------|
|                     |         |   Kenetic   | Energy | Arcane |
|  **ProjectileType** | Kenetic |      1      |   0.5  |    2   |
|                     |  Energy |      2      |    1   |   0.5  |
|                     |  Arcane |     0.5     |    2   |    1   |

This class does not need to be a `MonoBehavior` or `ScriptableObject`. It doesn't necessarily have to be a class. It does need to provide a static function that takes the parameters of a shield and the parameters of a projectile and returns a damage value as denoted by the combat algorithm.

## Stage 3: Factories and Specifications

Your task is to create factories that generate shields and projectiles via specification classes. These generated items should be instantiated as `GameObjects`. Use shield and projectile placeholders in the Unity project as a guide for how they should interact with the scene. 

### Stage 3.1: Specific Construction

This task has the following objectives:
1. Create a `ShieldFactory` and a `ProjectileFactory` using the factory design pattern.
2. You need to create `ShieldSpec` and `ProjectileSpec` classes whose only role is to hold a specification to be used by your factory. These types of data classes typically only have properties and no methods.
3. Your factories `Build` function should take a specification class as a parameter and should `Instantiate` a new `GameObject` based on the specification.
4. Your factories should be able `GenerateRandomShield`s and `GenerateRandomProjectile`s with properties within the stated specification ranges.

The automated factories in Aegis primarily build to external specifications. Specifications consist of the following values and ranges:

Shield Specifications:
* Capacity: 50 to 250
* Recharge Delay: 0.5 to 5 seconds
* Recharge Rate: 1 to 25 capacity per second.
* Type: Kinetic, Energy, or Arcane.

Projectile Specifications:
* Damage: 1 to 50
* Charge Time: 0.5 to 3 seconds.
* Type: Kinetic, Energy, or Arcane.

### Stage 3.2: Power Underwhelming

Unfortunately, your factories are also limited in the quality of shields and projectiles they can produce. Shields are limited to a power rating of 300, while projectiles are limited to a rating of 100. The following functions can determine ratings:

`ShieldRating = Capacity + (5 - RechargeDelay) * 5 + RechargeRate * (RechargeRate / 2)`

`ProjectileRating = Damage * 2 + (3 - ChargeTime)^4`

Your factories should not produce shields or projectiles with power ratings higher than the stated limits. If your factory receives a specification with a power rating over the stated limits, your factory should scale down the specification to be within the rating limit. How your factory scales down the specifications is your design choice; you could increase delay or change times, lower all properties by a percentage, or even randomly generate an entirely new shield.

## Stage 4: Still Alive

>This was a triumph!  
>I'm making a note here:  
>Huge success!  
>  
>It's hard to overstate  
>my satisfaction.  
>  
>Aperture Science:  
>We do what we must  
>because we can  
>For the good of all of us.  
>Except the ones who are dead.  

*[Still Alive](https://www.youtube.com/watch?v=VuLktUzq23c), Portal, 2007, Jonathan Coulton*  

In the grand traditions of automation and ever-increasing abstraction, this stage tasks you with creating a `TestFactory` that tests the output of factories.

Your `TestFactory`'s build method should create the following:
* A `ShieldSpec`.
  * A shield based on the generated specification.
* 5 to 10 `ProjectileSpecs`.
  * Projectile instantiations of those 5 to 10 `ProjectileSpec`s
* A `TestSchedule` that is capable of firing the projectiles in a sequential order while respecting their `ChargeTime`s.
* Setting up the generated shield GameObject and the `TestSchedule` in the scene.
* Running the test by shooting the projectiles at the shield via the `TestSchedule`.

You should report failure and huge success with the appropriate fanfare.
