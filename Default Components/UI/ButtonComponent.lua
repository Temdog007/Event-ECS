local uiComponent = require('uiComponent')
local class = require('classlib')
local buttonComponent = class('buttonComponent', uiComponent)

function buttonComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))

  local entity = self:getData(true)
  entity.text = ""
  entity.scaleX = 1.1
  entity.scaleY = 1.1
  entity.isClicked = false
  entity.drawingText = true
  entity.alignment = "center"
  entity.fontColor = {1,1,1,1}
  entity.main = classname(self)

  local values = entity.values or {}
  values.text = true
  values.scaleX = true
  values.scaleY = true
  values.alignment = true
  values.isClicked = true
  values.drawingText = true
  values.fontColor = true
  entity.values = values
end

function buttonComponent:eventValueChanged(args)
  local data = self:getData()
  if not args or (args.id ~= data.id and args.id ~= data.system:getID()) then return end

  if not data.enabled or not data.system:isEnabled() then self:set("isMosueOver", false) end
end

function buttonComponent:eventMouseMoved(args)
  self.uiComponent:eventMouseMoved(args)

  local entity = self:getData()
  if not entity.isMouseOver then
    entity.isClicked = false
  end
end

function buttonComponent:eventMousePressed(args)
  if not args then return end
  local x, y, b = args[1], args[2], args[3]
  if not x or not y or not b then return end

  local entity = self:getData()
  if b == 1 and self:isOver(x, y) then
    entity.isClicked = true
  end
end

function buttonComponent:eventMouseReleased(args)
  if not args then return end
  local b = args[3]
  if not b then return end

  local entity = self:getData()
  if b == 1 then
    if entity.isClicked and entity.isMouseOver and entity.action then
      entity.action()
    end
    entity.isClicked = false
  end
end

function buttonComponent:drawText()
  local entity = self:getData()
  local color = entity.fontColor
  if color then love.graphics.setColor(color) end
  love.graphics.printf(entity.text, entity.x, entity.y, entity.width, entity.alignment, 0, entity.scaleX, entity.scaleY)
end

function buttonComponent:draw()
  local entity = self:getData()
  self.uiComponent:draw()
  if entity.drawingText then self:drawText() end
end

lowerEventName(buttonComponent)

return buttonComponent
