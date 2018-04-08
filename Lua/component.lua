local class = require("classlib")

local component = class("Component")

function component:__init(entity, parent)
  assert(entity and string.match(entity:getName(), "Entity"), "Component must have an entity")
  self.entity = entity
  self.enabled = true
  self.parent = parent
end

function component:isEnabled()
  return self.enabled
end

function component:getBase()
  return self
end

function component:setEnabled(enabled)
  assert(type(enabled) == "boolean", "Must set enabled to a boolean value")
  if self.enabled ~= enabled then
    self.enabled = enabled
    self.entity.system:dispatchEvent("eventEnabledChanged", self, enabled)
  end
end

function component:eventRemovingComponent(args)
  if args.component == self then
    self.entity = nil
    self.enabled = false
  end
end

function component:eventRemovingEntity(args)
  if args.entity == self.entity then
    self:remove()
  end
end

function component:remove()
  return self.entity:removeComponent(self)
end

function component:serialize()
  local t =
  {
    "enabled", "boolean", "true"
  }
  if self.parent then
    for k,v in pairs(self.parent) do
      k = tostring(k)
      if not string.starts(k, "__") then
        local typ = type(v)
        if typ == "string" or typ == "number" or typ == "boolean" then
          local val = tostring(v)
          table.insert(t, k)
          table.insert(t, typ)
          table.insert(t, val)
        end
      end
    end
  end
  return table.concat(t, ",")
end

return component
