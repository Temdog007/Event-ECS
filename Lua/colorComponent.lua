local Component = require("component")
local class = require("classlib")

local ColorComponent = class("ColorComponent", Component)

function ColorComponent:__init(entity)
  self.Component:__init(entity, self)
  self.r = 1
  self.g = 1
  self.b = 1
  self.a = 1
end

function ColorComponent:eventSetColor(args)
  if not args then return end
  if args.r then
    if not (type(args.r) == "number") then
      return
    end
  end
  if args.g then
    if not assert(type(args.g) == "number") then
      return
    end
  end
  if args.b then
    if not assert(type(args.b) == "number") then
      return
    end
  end
  if args.a then
    if not assert(type(args.a) == "number")then
      return
    end
  end

	self.r = args.r or self.r
  self.g = args.g or self.g
  self.b = args.b or self.b
  self.a = args.a or self.a
  self:getEntity():dispatchEvent("eventColorChanged", {color = self, entity = self:getEntity()})
end

function ColorComponent:eventSetBackground(args)
  local color = self:getEntity().ColorComponent
  if not color then return end
  love.graphics.setBackgroundColor(color.r, color.g, color.b, color.a)
end

return ColorComponent
