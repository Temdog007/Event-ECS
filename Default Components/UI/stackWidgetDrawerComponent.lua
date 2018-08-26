local Component = require('uiComponent')
local class = require('classlib')

local stackWidgetDrawerComponent = class('stackWidgetDrawerComponent', Component)

function stackWidgetDrawerComponent:__init(entity)
  self:setDefault('name', classname(self))
  self:set('entity', entity)

  entity:setDefault("main", classname(self))
  entity:setDefaultsAndValues({
    debugDraw = false
  })
end

local function widgetDraw(widget)
  for _, item in ipairs(widget.items) do
    if item:isEnabled() then
      local target = item[item:get("main") or 0]
      if target then target:draw() end
    end
  end
end

function stackWidgetDrawerComponent:eventDraw(args)
  if not self:canDraw(args)  then return end

  self:draw()
end

function stackWidgetDrawerComponent:draw()
  local entity = self:getData()

  if entity.debugDraw then
    love.graphics.setColor(entity.bgColor)
    love.graphics.rectangle("fill", entity.x, entity.y, entity.width, entity.height)
  end

  if entity.useScissor then
    love.graphics.setScissor(entity.scissorX, entity.scissorY,
                    entity.scissorWidth, entity.scissorHeight)
    widgetDraw(entity)
    love.graphics.setScissor()
  else
    widgetDraw(entity)
  end
end

lowerEventName(stackWidgetDrawerComponent)

return stackWidgetDrawerComponent
