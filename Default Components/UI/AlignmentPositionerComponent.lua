local Component = require('component')
local class = require('classlib')

local AlignmentPositionerComponent = class('AlignmentPositionerComponent', Component)

function AlignmentPositionerComponent:__init(entity)
  self.Component:__init(entity, self)
  self.horizontalAlignment = "center"
  self.verticalAlignment = "center"
end

function AlignmentPositionerComponent:setPosition(w, h)
  w = w or love.graphics.getWidth()
  h = h or love.graphics.getHeight()

  if self.horizontalAlignment == "center" then
    self.item.x = (w/2) - (self.item.width * (self.item.scaleX or 1) * 0.5)
  elseif self.horizontalAlignment == "right" then
    self.item.x = w - self.item.width * (self.item.scaleX or 1)
  else
    self.item.x = 0
  end

  if self.verticalAlignment == "center" then
    self.item.y = (h/2) - (self.item.height * (self.item.scaleY or 1) * 0.5)
  elseif self.verticalAlignment == "bottom" then
    self.item.y = h - self.item.height * (self.item.scaleY or 1)
  else
    self.item.y = 0
  end
end

function AlignmentPositionerComponent:eventResize(args)
  local w,h = love.graphics.getDimensions()
  if args then
    w = args[1] or w
    h = args[2] or h
  end

  if self.item then
    self:setPosition(w,h)
  end
end

return AlignmentPositionerComponent
