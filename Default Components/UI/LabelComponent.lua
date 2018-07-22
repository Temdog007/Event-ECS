local Component = require('uiComponent')
local class = require('classlib')
local Entity = require("entity")

local labelComponent = class('labelComponent', Component)

function labelComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))

  local entity = self:getEntity(true)

  entity.text = ""
  entity.alignment = "center"
  entity.scaleX = 1
  entity.scaleY = 1
  entity.fontColor = {1,1,1,1}
  entity.bgColor = {0,0,0,0}
  entity.highlightColor = {0,0,0,0}
  entity.pressedColor = {0,0,0,0}
  entity.draw = {ui = self, draw = labelComponent.draw}

  local values = entity.values or {}
  values.text = true
  values.alignment = true
  values.scaleX = true
  values.scaleY = true
  values.fontColor = true
  entity.values = values
end

function labelComponent:draw()
  local entity = self:getEntity()
  self.uiComponent:draw()

  local color = entity.fontColor
  if color then love.graphics.setColor(color) end

  love.graphics.printf(entity.text, entity.x, entity.y, entity.width, entity.alignment, 0, entity.scaleX, entity.scaleY)
end

return labelComponent
