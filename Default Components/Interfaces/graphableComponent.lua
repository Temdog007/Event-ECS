-- code from https://github.com/icrawler/FPSGraph/blob/master/FPSGraph.lua

local Component = require('drawableComponent')
local class = require('classlib')

local graphableComponent = class('graphableComponent', Component)

function graphableComponent:__init(entity)
  self:setDefault('name', classname(self))
  self:set('entity', entity)

  local vals = {}
  for i = 1, math.floor(25) do table.insert(vals, 0) end

  entity:setDefaultsAndValues({
    x = 0,
    y = 0,
    width = 50,
    height = 30,
    draggable = true,
    delay = 0.5,
    vals = vals,
    vmax = 0,
    currentTime = 0,
    label = "graph",
    dx = 0,
    dy = 0,
    isDown = false,
    font = love.graphics.newFont(8)
  })
end

function graphableComponent:updateGraph(val, label, dt)

  local data = self:getData()
  data.currentTime = data.currentTime + dt

  if data.draggable then
    local mx, my = love.mouse.getPosition()
    if data.isDown or (data.x < mx and mx < data.x + data.width and
      data.y < my and my < data.y + data.height) then
        if love.mouse.isDown(1) then
          data.isDown = true
          data.x = mx - data.dx
          data.y = my - data.dy
        else
          data.isDown = false
          data.dx = mx - data.x
          data.dy = my - data.y
        end
    end
  end

  while data.currentTime >= data.delay do
    data.currentTime = data.currentTime - data.delay

    table.remove(data.vals, 1)
    table.insert(data.vals, val)

    local max = 0
    for i = 1, #data.vals do
      local v = data.vals[i]
      if v > max then max = v end
    end

    data.vmax = max
    data.label = label
  end
end

function graphableComponent:eventDraw(args)
  if not self:canDraw(args) then return end

  local data = self:getData()

  love.graphics.setFont(data.font)
  local maxval = math.ceil(data.vmax / 10) * 10 + 20
  local len = #data.vals
  local step = data.width / len

  for i = 2, len do
    local a = data.vals[i-1]
    local b = data.vals[i]
    love.graphics.line(
      step * (i-2)  + data.x,
      data.height * (-a/maxval + 1) + data.y,
      step*(i-1) + data.x,
      data.height * (-b/maxval + 1) + data.y)
  end

  love.graphics.print(data.label, data.x, data.height + data.y - 8)
end

lowerEventName(graphableComponent)

return graphableComponent
