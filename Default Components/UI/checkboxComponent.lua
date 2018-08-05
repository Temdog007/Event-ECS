local Component = require('buttonComponent')
local class = require('classlib')

local checkboxComponent = class('checkboxComponent', Component)

function checkboxComponent:__init(en)
  self:setDefault('name', classname(self))
  self:set('entity', en)

  local entity = self:getEntity()
  entity.isChecked = false
  entity.checkedText = "Yes"
  entity.space = 25
  entity.checkedColor = {0,1,0,1}
  entity.uncheckedText = "No"
  entity.uncheckedColor = {1, 0, 0, 1}
  entity.checkedAlignment = "center"
  entity.showCheckState = true
  entity.action = function()
    self:set("isChecked", not self:get("isChecked"))
  end
  entity.main = classname(self)

  local values = entity.values or {}
  values.isChecked = true
  values.space = true
  values.checkedAlignment = true
  values.checkedText = true
  values.checkedColor = true
  values.uncheckedText = true
  values.uncheckedColor = true
  values.showCheckState = true
  entity.values = values
end

function checkboxComponent:setEnabled(enabled)
  self.uiComponent:setEnabled(enabled)
  self:updateBinding()
end

function checkboxComponent:eventSystemEnabled(args)
  if not args or args.system ~= self:get("system") then return end
  self:updateBinding()
end

function checkboxComponent:eventEntityEnabled(args)
  if not args or args.entity ~= self:get("entity") then return end
  self:updateBinding()
end

function checkboxComponent:updateBinding()
  local en = self:getEntity()
  local action =  assert(en.bindingAction)
  self:set("isChecked", action())
end

function checkboxComponent:eventAddedComponent(args)
  if not args or args.component ~= self then return end
  self:watchValues("isChecked")
end

function checkboxComponent:eventItemChanged(args)
  local entity = self:getEntity()
  if not args or args.id ~= entity.id then return end

  local action = assert(entity.checkedAction)
  action(entity.isChecked)
end

function checkboxComponent:drawCheckedText()
  local entity = self:getEntity()
  local color = entity.isChecked and entity.checkedColor or entity.uncheckedColor
  if color then love.graphics.setColor(color) end
  love.graphics.printf(entity.isChecked and entity.checkedText or entity.uncheckedText,
    entity.x, entity.y + entity.space, entity.width,
    entity.checkedAlignment, 0, entity.scaleX, entity.scaleY)
end

function checkboxComponent:draw()
  local entity = self:getEntity()
  self.buttonComponent:draw()
  if entity.showCheckState then self:drawCheckedText() end
end

lowerEventName(checkboxComponent)

return checkboxComponent
