# Event-ECS
Event-driven Entity Component System written in Lua.

Each system contains multiple entities. A system will dispatch events to each entity.

Each entity contains multiple components. An entity will dispatch events to each component.

Each component is a table of functions that are called when the dispatched event name matches the function name.

Each system can have a server that will listen for requests to send data.

Each system, entity, and component will be able to serialize itself.

The WPF C# application will be designed to give a visual representation of the ECS in Lua.

The WPF C# application will be reading the serilalized ECS state to update the GUI.

The WPF C# application will be able to change the ECS state including (adding, removing, or editing the system, its entities, and its components)
