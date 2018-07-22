local Component = require('drawableComponent')
local class = require('classlib')

local labelDrawerComponent = class('labelDrawerComponent', Component)

function labelDrawerComponent:__init(en)
  self:set("entity", en)
  self:setDefault("name", classname(self))
end

function labelDrawerComponent:eventDraw(args)
  local entity = self:getEntity()

  if not args or args.drawOrder ~= entity.drawOrder then return end

  local label = entity.labelComponent
  if not label then return end

  label:draw()
end

return labelDrawerComponent
