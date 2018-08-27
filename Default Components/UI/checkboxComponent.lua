local Component = require('buttonComponent')
local class = require('classlib')

local checkboxComponent = class('checkboxComponent', Component)

function checkboxComponent:__init(en)
  self:setDefault('name', classname(self))
  self:set('entity', en)

  local entity = self:getData()
  entity.isChecked = false
  entity.checkedText = "Yes"
  entity.uncheckedText = "No"
  entity.action = function()
    self:set("isChecked", not self:get("isChecked"))
    self:get("checkedAction")(self:get("isChecked"))
  end
  entity.main = classname(self)

  local values = entity.values or {}
  values.isChecked = true
  values.checkedText = true
  values.uncheckedText = true
  entity.values = values
end

function checkboxComponent:draw()
  local entity = self:getData()
  self.buttonComponent:draw(string.format("%s: %s", entity.text, entity.isChecked and entity.checkedText or entity.uncheckedText))
end

lowerEventName(checkboxComponent)

return checkboxComponent
