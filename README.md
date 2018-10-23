# Event-ECS
Event-driven Entity Component System is a game engine designed specifically for reloading components during runtime of game applications. This allows for quick, easy changes to code while the game is running without having to reload or restart the application. This engine was been written in Lua (designed  specifically for the **[Love2D](http:love2d.org)** game engine) and ported to java-script (**HTML5**).

## How it works
A game using this engine will consist of multiple systems. Each system contains multiple entities. Each entity contains multiple components. A component consists of event handling functions that its owner entity can call.

The job of the system is to dispatch events to the entity. The entity will call functions of its components based on the event.

### Lua GUI
The WPF C# application is designed to give a visual representation of the ECS in Lua. The WPF C# application reads the serialized ECS state from the Love2D application (from the server component) to update the GUI. The WPF C# application is able to change the ECS state including:

* Adding systems, entities, and components
* Removing systems, entities, and components
* Dispatch events to systems and/or entities
* Disabling systems, entities, and components
* Reloading components to use the latest updates to the code

![Alt text](Images/screenshot2.png?raw=true "GUI Screenshot2")
![Alt text](Images/screenshot3.png?raw=true "Game Screenshot")

### Java-script
Instead of a separate GUI, java-script code is written in to the web application. It has the same functionality as the WPF application.

![Alt text](Images/screenshot4.png?raw=true "Java-script Screenshot")
