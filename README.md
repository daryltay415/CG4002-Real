# B19 Street Fighter AR

## Overview
Street Fighter AR is a third person, multiplayer fighting AR game. The player is able to control the character using physical movements such as jabs, hooks and uppercuts.

* Controls: Players use a glove with IMUs and buttons attached to it to control the sprites. The AI will recgonize the gestures when the players move and it will send a command to the AR app to run the animation.
* Niantic spatial: It is used to syncrhonize the positions of both players and also set up the servers for the matchmaking.
* MQTT: MQTT protocol is used to communicate with the AI and the hardware of the gloves with the AR application.

## Key features
### Multiplayer match
* The players are able to play a match together. They have 3 sets of the moves for each hand. The jab, hook and uppercut. Each player will also have 1 special move, the fireball and a guard move.
* The healthbars are shown at the top of the opponent's head and the player's health is shown on the right side of the screen. The heartrate is visible on the top right and the special meter bar is on the top left.
* The player sprites are positioned directly infront of the camera in third person view. The player's head will be missing to give players a better view of the opponent.

### Tutorial
* There is a guide in the tutorial to teach users how to do the actions and it verifies their accuracy as well.

### God mode
* When god mode is activated, players will have 999 health and 5 seconds of cooldown time for their special ability.
* Players can still play in multiplayer but only the person that activated god mode will have the extra health and lower cooldown time for the special ability.

## Set up for developer
### Prerequisites
* Unity version 2022.3 LTS (2022.3.11f1) is the best version for this AR application.
* An Android phone preferablly released in 2020 and above.
*  Niantic spatial account.

### Installation
1. Clone the repo
2. Open the project in unity hub
3. Download the required packages, Niantic lightship, AR foundation, textmeshpro, google AR core xr plugin and unity netcode.

## How to play
### Main multiplayer Gameplay
### For host:
1. When hosting a multiplayer room, ensure the qr code is infront of the phone camera before pressing the host button. 
2. Once the start button is enabled, put the phone at your eye level and press start

### For client:
1. When joining a multiplayer room, ensure the qr code is infront of the phone camera before pressing the join button. You must be facing the host and scanning the same qr code.
2. Once the start button is enabled, put the phone at your eye level and wait for the host to join before pressing start.

Once both players are joined, they can control the sprites using the gloves and play the game.

### Tutorial Gameplay
1. Ensure the qr code is infront of the camera before pressing the tutorial button.
2. When the start button is enabled, put the phone at your eye level and start the game.
3. Follow the tutorial instructions to complete the tutorial.

### GodMode
1. The steps are the same as the main multiplayer gameplay but the players have to press the godmode button before they host or join the game.

### Controls
* Left jab
* Right jab
* Left hook
* Right hook
* Left uppercut
* Right uppercut
* Special move: tap each button individually on the gloves before doing a right jab to shoot a fireball
* Guard: Raise both hands to guard.

## References
* AR Foundation: https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.4/manual/index.html
* Niantic Lightship: https://www.nianticspatial.com/docs/nsdk/setup/?platform=unity
