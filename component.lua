require("classlib")

local component = class("Component")

function component:__init(entity)
  assert(entity and entity:getName() == "Entity", "Component must have an entity")

  self.entity = entity
  self.enabled = true
end

function component:isEnabled()
  return self.enabled
end

function component:setEnabled()
  assert(type(enabled) == "boolean", "Must set enabled to a boolean value")
  if self.enabled ~= enabled then
    self.enabled = enabled
    self.entity.system:dispatchEvent("enabledChanged", self, enabled)
  end
end

function component:remove()
  self.entity:removeComponent(self)
end

return component
