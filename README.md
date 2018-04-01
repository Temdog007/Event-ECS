# Event-ECS
Event-driven Entity Component System written in Lua.

Each system contains multiple entities. A system will dispatch events to each entity.

Each entity contains multiple components. An entity will dispatch events to each component.

Each component is a table of functions that are called when the dispatched event name matches the function name.

Each system can have a server that will listen for requests to send data.

The System will encode itself into JSON code and then that code to the client.

The WPF C# application will be designed to give a visual representation of the ECS in Lua.

The WPF C# application will be the ECS state by sending requests to the system's server.
