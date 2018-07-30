local Component = require('component')
local class = require('classlib')
local System = require("systemList")

local valueWatcher = class('valueWatcher')

function valueWatcher:__init()
  self.lastValues = {}
  self.valueKeys = {}
end

function valueWatcher:setValues(val, ...)
  local keys = self.valueKeys
  for _, key in pairs({...}) do
    keys[key] = val
  end
end

function valueWatcher:watchValues(...)
  self:setValues(true, ...)
end

function valueWatcher:stopWatchValues(...)
  self:setValues(false, ...)
end

function valueWatcher:update(currentValues)
  local changed = false
  for value, watch in pairs(self.valueKeys) do
    if watch then
      if currentValues[value] ~= self.lastValues[value] then
        changed = true
        self.lastValues[value] = currentValues[value]
      end
    end
  end
  if changed then
    System.pushEvent("eventItemChanged", {id = currentValues.id})
  end
end

return valueWatcher
