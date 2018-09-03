-- code from https://github.com/icrawler/FPSGraph/blob/master/FPSGraph.lua

local Component = require('graphableComponent')
local class = require('classlib')

local fpsGraphComponent = class('fpsGraphComponent', Component)

function fpsGraphComponent:__init(entity)
  self:setDefault('name', classname(self))
  self:set('entity', entity)
end

function fpsGraphComponent:eventUpdate(args)
  if not args or not args.dt then return end

  local fps = 0.75/args.dt + 0.25*love.timer.getFPS()
  self:updateGraph(fps, "FPS: "..math.floor(fps*10)/10, args.dt)
end

lowerEventName(fpsGraphComponent)

return fpsGraphComponent
