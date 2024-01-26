# Game Enemy AI
This is my Game Enemy AI Project

I used the Unity Game Engine and C#

## Table of Contents 
* [Motivation](#motivation)
* [Approach](#My-Approach)
* [Results](#results)
* [What Was learned](#what-was-learned)


## Motivation
I wanted to create a game enemy AI that can patrol, chase, and attack player.<br>


## My Approach
The AI will alternate between Idle and patrol state, and if the player gets close,<br>
the AI will switch to attack and start shooting balls at the player.<br>

To accomplish this, I decided to use a state machine. It allows me to add in behaviors and transitions between them for the AI<br>
The picture below illustrates when the AI will change behavior.

![state machine](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/02f68499-c854-451f-a19c-30c6c5efe4b7)

<br>
Attack state<br>
The AI checks if player is within a certain range. It it is, it starts attacking.

![attack](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/faeff65e-625d-4e92-b841-a0ff09dbc0f9)
<br>
Idle<br>
For idle, the enemy AI will just look around, when it stops looking around, it will choose between <br>
going on patrol or continue staying idle.

![idle](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/2ace8440-5aff-472b-bd2e-78b580b05b52)
<br>
Patrol<br>
For patrolling, I used the A-star pathfinding algorithm.
It finds the best path to a random location or where the player is while making sure it does not hit obstacles.<br>

![image](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/ebfa15fb-eac1-416d-95e9-5f0d441f4a5b)

![image](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/4143a6fb-5392-4d66-b16b-88180867a556)

The algorithm in action

![patrolling](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/9fc21a53-ccce-4913-a0a1-128db4f6f8a8)

## Results
Overall, the project was a success. The AI can patrol and idle while the player is not around, and if
the player is near, the enemy AI will start attacking by shooting balls at the player

![demo](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/705ad39a-a7fa-44b6-944a-6ad2a31d0ea2)

## What was learned
* Learning how to make an enemy AI attack the player and walk around like we see in games proved difficult. There is lots of code that
  goes into just pathfinding.
* Learned a cool pathfinding algorithm
* 
* 
  



























