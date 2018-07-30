local Component = require('component')
local class = require('classlib')
local System = require("systemList")

local valueWatcherComponent = class('valueWatcherComponent', Component)

function valueWatcherComponent:__init(en)
  self:setDefault('name', classname(self))
  self:set('entity', en)

  local entity = self:getEntity(true)
  entity.lastValues = {}
  entity.valueKeys = {}
end

function valueWatcherComponent:setValues(val, ...)
  local entity = self:getEntity()
  local keys = entity.valueKeys
  for _, key in pairs({...}) do
    keys[key] = val
  end
end

function valueWatcherComponent:watchValues(...)
  self:setValues(true, ...)
end

function valueWatcherComponent:stopWatchValues(...)
  self:setValues(false, ...)
end

function valueWatcherComponent:eventUpdate(args)
  
  local entity = self:getEntity()
  for value, watch in pairs(entity.valueKeys) do
    if watch then
      if entity[value] ~= entity.lastValues[value] then
        System.pushEvent("eventItemChanged", {id = entity.id})
      end
    end
  end
end

function valueWatcherComponent:eventItemChanged(args)
  local f = self:get("OnValueChange")
  if f then f(args) end

  local entity = self:getEntity()
  if not args or args.id ~= entity.id then return end

  local entity = self:getEntity()
  for key in pairs(entity.valueKeys) do
    entity.lastValues[key] = entity[key]
  end
end

lowerEventName(valueWatcherComponent)

return valueWatcherComponent
