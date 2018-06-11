local Component = require('component')
local class = require('classlib')

local SliderActionComponent = class('SliderActionComponent', Component)

function SliderActionComponent:__init(entity)
  self.Component:__init(entity, self)
end

function SliderActionComponent:eventSliderUpdated(args)
  if not args then return end

  local slider = args.slider
  if not slider then return end

  if slider ~= self:getComponent("SliderComponent") then
    return
  end

  if self.action then
    self.action(slider.value)
  elseif Log then
    Log("Slider was updated but an action has not been defined")
  end
end

return SliderActionComponent
