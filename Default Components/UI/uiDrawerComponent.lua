local Component = require('drawableComponent')
local class = require('classlib')

local uiDrawerComponent = class('uiDrawerComponent', Component)

function uiDrawerComponent:__init(en)
  self:setDefault('name', classname(self))
  self:set('entity', en)

  en:setDefaultsAndValues({target = ""})
  en:set("dispatchEventOnValueChange", true)
  en:get("system"):set("dispatchEventOnValueChange", true)
end

function uiDrawerComponent:eventDraw(args)
  if not self:canDraw(args) then return end

  local entity = self:get("entity")
  local target = self:get("target")
  if not target then return end

  local uiObj = entity[target]
  if not uiObj then return end

  uiObj:draw()
end

lowerEventName(uiDrawerComponent)

return uiDrawerComponent
