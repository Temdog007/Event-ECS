local class = require("classlib")

local component = class("Component")

local id = 0
function component:__init(entity)
  assert(entity and string.match(entity:getName(), "Entity"), "Component must have an entity")

  self.entity = entity
  self.enabled = true
  self.id = id + 1
  id = id + 1
end

function component:isEnabled()
  return self.enabled
end

function component:setEnabled(enabled)
  assert(type(enabled) == "boolean", "Must set enabled to a boolean value")
  if self.enabled ~= enabled then
    self.enabled = enabled
    self.entity.system:dispatchEvent("eventEnabledChanged", self, enabled)
  end
end

return component
