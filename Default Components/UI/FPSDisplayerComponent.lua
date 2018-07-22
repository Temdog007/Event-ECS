local Component = require('drawableComponent')
local class = require('classlib')

local fpsDisplayerComponent = class('fpsDisplayerComponent', Component)

function fpsDisplayerComponent:__init()
  self:setDefault("name", classname(self))
  
  local entity = self:getEntity(true)

  entity.x = 0
  entity.y = 0
  entity.scaleX = 1
  entity.scaleY = 1
  entity.limit = 100
  entity.color = {1,1,1,1}

  local values = entity.values or {}
  values.x = true
  values.y = true
  values.scaleX = true
  values.scaleY = true
  values.limit = true
  values.color = true
  entity.values = values
end

function fpsDisplayerComponent:eventDraw(args)
  local entity = self:getEntity()

  if not args or args.drawOrder ~= entity.drawOrder then return end

  local color = entity.color
  if color then love.graphics.setColor(color) end

  love.graphics.print(love.timer.getFPS(), entity.x, entity.y, 0, entity.scaleX, entity.scaleY)
end

return fpsDisplayerComponent
