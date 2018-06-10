local Component = require('component')
local class = require('classlib')

local LabelComponent = class('LabelComponent', Component)

function LabelComponent:__init(entity)
  self.Component:__init(entity, self)

  self.x = 0
  self.y = 0
  self.width = 100
  self.text = ""
end

function LabelComponent:eventDraw(args)
  local color = self:getComponent("ColorComponent")
  if color then
    love.graphics.setColor(color)
  end
  love.graphics.printf(self.text, self.x, self.y, self.width, "center")
end

return LabelComponent
