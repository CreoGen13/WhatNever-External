Project contains only client source code from my indie game WhatNever

Project uses MVP with UniRX and ZenJect as base architecture for UI and game.

Main classes that run the game are located in ```Core/Game``` folder and presented by ```GameView```, ```GamePresenter``` and ```GameModel```.

Game must download plot - list of panel nodes that describes what part of story to show and current panel - list of action nodes that describes what to do or what to show.
```GamePresenter``` delegates it's duty to calculate nodes to processors - every node has it's own processor.

Node editor (is not icluded in this project) has it's own domain area for node that serialize nodes in ```NodeData``` or ```PanelData```.
Client code converts ```NodeData``` into ```LoadedNodeData``` and ```PanelData``` into ```LoadedPanelData```.

There is a lot to say about this project, nevertheless I just highlighted only main points. You are free to discover my code by yourself :)
