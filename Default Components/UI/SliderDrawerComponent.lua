local Component = require('drawableComponent')
local class = require('classlib')
local SliderDrawerComponent = class('sliderDrawerComponent', Component)

function SliderDrawerComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))
end

function SliderDrawerComponent:eventDraw(args)
  local entity = self:getEntity()
  if not args or args.drawOrder ~= entity.drawOrder then return end

  local button = entity.SliderDrawerComponent
  if button then button:draw() end
end

return SliderDrawerComponent
