# Game Enemy AI
A game enemy AI that can patrol, chase, and attack player.
The AI will alternate between Idle and patrol state, and if the player gets close,
the AI will switch to attack and start shooting balls at the player


![demo](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/705ad39a-a7fa-44b6-944a-6ad2a31d0ea2)


<br>
<br>
<br>


The AI uses a state machine with 3 states: Idle, Patrol, Attack
The picture below illustrates how the state transition to one another
<br>
<br>


![state machine](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/02f68499-c854-451f-a19c-30c6c5efe4b7)

Patrol

![patrolling](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/9fc21a53-ccce-4913-a0a1-128db4f6f8a8)

Attack

![attack](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/faeff65e-625d-4e92-b841-a0ff09dbc0f9)

Idle

![idle](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/2ace8440-5aff-472b-bd2e-78b580b05b52)
<br>
<br>
<br>
<br>

The AI uses A-star algorithm for pathfinding
It finds the best path to a random location or where the player is while making sure it takes obstacles into account

![image](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/ebfa15fb-eac1-416d-95e9-5f0d441f4a5b)

![image](https://github.com/vincentc168777/Game-Enemy-AI/assets/93815609/4143a6fb-5392-4d66-b16b-88180867a556)





