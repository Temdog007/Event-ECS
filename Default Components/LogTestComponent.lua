local Component = require("component")
local class = require("classlib")

local LogTest = class("LogTestComponent", Component)

function LogTest:__init(entity)
  self.Component:__init(entity)
end

function LogTest:eventAddedComponent(args)
  if args.component == self then
    Log("Component Added")
  end
end

function LogTest:eventRemovingComponent(args)
  if args.component == self then
    Log("Removing component")
  end
end

return  LogTest
