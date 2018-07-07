local Component = require('component')
local class = require('classlib')

local sliderActionComponent = class('sliderActionComponent', Component)

function sliderActionComponent:__init(entity)
  self.Component:__init(entity, self)
end

function sliderActionComponent:eventMouseMoved(args)
  if not args then return end
  local x, y = args[1], args[2]
  if not x or not y then return end

  local slider = self:getComponent("sliderComponent")
  if not slider then return end

  if slider.isClicked and slider:isOver(x, y) then
    if self.action then
      self.action(slider.value, slider:getPercentage())
    else
      print("Slider was moved but has not action defined")
    end
  end
end

return sliderActionComponent
