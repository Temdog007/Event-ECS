local Component = require("component")
local class = require("classlib")

local ColorComponent = class("ColorComponent", Component)

function ColorComponent:__init(entity)
  self.Component:__init(entity, self)
  self.r = 1
  self.g = 1
  self.b = 1
  self.a = 1
  entity.color = self
end

function ColorComponent:set(r, g, b, a)
  if r then
    assert(type(r) == "number")
  end
  if g then
    assert(type(g) == "number") end
  if b then
    assert(type(b) == "number") end
  if a then
    assert(type(a) == "number")
  end

	self.r = r or self.r
  self.g = g or self.g
  self.b = b or self.b
  self.a = a or self.a
  self:getEntity():dispatchEvent("eventColorChanged", {color = self, entity = self:getEntity()})
end

return ColorComponent
