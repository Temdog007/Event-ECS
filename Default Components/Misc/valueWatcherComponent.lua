local Component = require('component')
local class = require('classlib')

local valueWatcherComponent = class('valueWatcherComponent', Component)

function valueWatcherComponent:__init(en)
  self:setDefault('name', classname(self))
  self:set('entity', en)

  local entity = self:getEntity(true)
  entity.lastValues = {}
  entity.valueKeys = {}
end

function valueWatcherComponent:eventUpdate(args)
  local entity = self:getEntity()
  for value, watch in pairs(entity.valueKeys) do
    if watch then
      if entity[value] ~= entity.lastValues[value] then
        entity.system:dispatchEvent("eventItemChanged", {id = entity.id})
      end
    end
  end
end

function valueWatcherComponent:eventItemChanged(args)
  local entity = self:getEntity()
  if not args or args.id ~= entity.id then return end

  local entity = self:getEntity()
  for _, value in pairs(entity.valueKeys) do
    entity.lastValues[value] = entity[value]
  end
end

lowerEventName(valueWatcherComponent)

return valueWatcherComponent
