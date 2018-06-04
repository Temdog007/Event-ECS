local Component = require('component')
local class = require('classlib')

local SliderComponent = class('SliderComponent', Component)

function SliderComponent:__init(entity)
  self.Component:__init(entity, self)
  self.min = 0
  self.max = 100
  self.value = 0

  self.text = ""
  self.isMouseOver = false
  self.isClicked = false
  self.vertical = false
  self.x = 0
  self.y = 0
  self.width = 100
  self.height = 100
end

function SliderComponent:isOver(x, y)
  return self.x < x and x < self.x + self.width and
          self.y < y and y < self.y + self.height
end

function SliderComponent:updatePosition(x, y)
  if self.isClicked then
    local minX, maxX = self:getXBoundary()
    local minY, maxY = self:getYXBoundary()

    if self.isMouseOver then
      if self.vertical then
        self.value = self.min + (self.max - self.min) * (y - minY) / (maxY - minY)
      else
        self.value = self.min + (self.max - self.min) * (x - minX) / (maxX - minX)
      end
    else
      love.mouse.setPosition(math.min(maxX, math.max(minX, x)), math.min(maxY, math.max(minY, y)))
    end
  end
end

function SliderComponent:getXBoundary()
  return self.x, self.x + self.width
end

function SliderComponent:getYXBoundary()
  return self.y, self.y + self.height
end

function SliderComponent:getPercentage()
  return (self.value - self.min) / (self.max - self.min)
end

function SliderComponent:eventMouseMoved(args)
  if not args then return end
  local x, y = args[1], args[2]
  if not x or not y then return end

  self.isMouseOver = self:isOver(x, y)
  self:updatePosition(x,y)
end

function SliderComponent:eventMousePressed(args)
  if not args then return end
  local x, y, b = args[1], args[2], args[3]
  if not x or not y or not b then return end

  if b == 1 and self:isOver(x, y) then
    self.isClicked = true
    self:updatePosition(x, y)
  end
end

function SliderComponent:eventMouseReleased(args)
  if not args then return end
  local b = args[3]
  if not b then return end

  if b == 1 then
    self.isClicked = false
  end
end

return SliderComponent
