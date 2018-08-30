local Component = require('drawableComponent')
local class = require('classlib')

local fpsDisplayerComponent = class('fpsDisplayerComponent', Component)

function fpsDisplayerComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))

  local entity = self:getData(true)

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
  if not self:canDraw(args) then return end

  local data = self:getData()
  local color = data.color
  if color then love.graphics.setColor(color) end

  if data.font then love.graphics.setFont(data.font) end
  love.graphics.print(love.timer.getFPS(), data.x, data.y, 0, data.scaleX, data.scaleY)
end

lowerEventName(fpsDisplayerComponent)

return fpsDisplayerComponent
