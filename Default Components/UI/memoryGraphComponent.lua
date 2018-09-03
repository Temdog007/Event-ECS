-- code from https://github.com/icrawler/FPSGraph/blob/master/FPSGraph.lua

local Component = require('graphableComponent')
local class = require('classlib')

local memoryGraphComponent = class('memoryGraphComponent', Component)

function memoryGraphComponent:__init(entity)
  self:setDefault('name', classname(self))
  self:set('entity', entity)
end

function memoryGraphComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local mem = collectgarbage("count")
  self:updateGraph(mem, "Memory (KB): "..math.floor(mem*10)/10, args.dt)
end

lowerEventName(memoryGraphComponent)

return memoryGraphComponent
