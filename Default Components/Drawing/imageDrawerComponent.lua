local Component = require('drawableComponent')
local class = require('classlib')

local imageDrawerComponent = class('imageDrawerComponent', Component)

function imageDrawerComponent:__init(en)
  self:setDefault('name', classname(self))
  self:set('entity', en)

  en:setDefaultsAndValues({
    x = 0,
    y = 0,
    rotation = 0,
    scaleX = 1,
    scaleY = 1,
    offsetX = 0,
    offsetY = 0,
    color = {1,1,1,1}
  })
end

function imageDrawerComponent:eventDraw(args)
  if not self:canDraw(args) then return end

  local d = self:get("drawable")
  if not d then return end

  local entity = self:getEntity()
  if entity.color then love.graphics.setColor(entity.color) end
  love.graphics.draw(d, entity.x, entity.y, entity.rotation,
    entity.scaleX, entity.scaleY, entity.offsetX, entity.offsetY)
end

lowerEventName(imageDrawerComponent)

return imageDrawerComponent
