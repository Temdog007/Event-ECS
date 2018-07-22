local class = require('classlib')
local drawableComponent = class('drawableComponent', require("component"))

function drawableComponent:__init()
  self:setDefault("name", classname(self))
  
  local entity = self:getEntity(true)
  entity.drawOrder = 0

  local values = entity.values or {}
  values.drawOrder = true
  entity.values = values
end

function drawableComponent:canDraw(args)
  local entity = self:getEntity()
  return args and args.drawOrder == entity.drawOrder
end

return drawableComponent
