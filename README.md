# Event-ECS
Event-driven Entity Component System written in Lua

Each system contains multiple entities. A system will dispatch events to each entitiy.

Each entity contains multiple components. An entity will dispatch events to each component.

Each component is a table of functions that are called when the dispatched event name matches the function name.
