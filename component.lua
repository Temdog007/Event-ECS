require("classlib")

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

function component:setEnabled()
  assert(type(enabled) == "boolean", "Must set enabled to a boolean value")
  if self.enabled ~= enabled then
    self.enabled = enabled
    self.entity.system:dispatchEvent("eventEnabledChanged", self, enabled)
  end
end

function component:getID()
  return self.id
end

function component:getData(data)
  data = data or {}
  for k, compValue in pairs(self) do
    if k ~= "__type" and (type(compValue) == "number" or type(compValue) == "string") then
      data[k] = compValue
    end
  end
  data.id = self:getID()
  local topClass = getmetatable(component)
  if #self.__bases > 0 then
    topClass = self.__bases[#self.__bases]
  end
  data.name = classname(topClass)
  return data
end

function component:remove()
  self.entity:removeComponent(self)
end

return component
