local Component = require('component')
local class = require('classlib')

local ButtonComponent = class('ButtonComponent', Component)

function ButtonComponent:__init(entity)
  self.Component:__init(entity, self)

  self.text = ""
  self.isMouseOver = false
  self.x = 0
  self.y = 0
  self.width = 100
  self.height = 25
end

function ButtonComponent:eventMouseMoved(args)
  if not args then return end
  local x, y = args[1], args[2]
  if not x or not y then return end

  self.isMouseOver = self.x < x and x < self.x + self.width and
                self.y < y and y < self.y + self.height
end

return ButtonComponent
