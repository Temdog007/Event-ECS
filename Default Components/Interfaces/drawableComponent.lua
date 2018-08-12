local class = require('classlib')
local drawableComponent = class('drawableComponent', require("component"))

function drawableComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))

  local entity = self:getData(true)
  entity.drawOrder = 0

  local values = entity.values or {}
  values.drawOrder = true
  entity.values = values
end

function drawableComponent:canDraw(args)
  local entity = self:getData()
  return args and args.drawOrder == entity.drawOrder
end

return drawableComponent
