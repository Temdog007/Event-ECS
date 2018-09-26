local Component = require('drawableComponent')
local class = require('classlib')
local gui = require("Gspot")

local guiHandlerComponent = class('guiHandlerComponent', Component)

function guiHandlerComponent:__init(entity)
  self:setDefault('name', classname(self))
  self:set('entity', entity)
end

function guiHandlerComponent:eventLoad(args)
  gui:load()
end

function guiHandlerComponent:eventUpdate(args)
  if not args or not args.dt then return end

  gui:update(args.dt)
end

function guiHandlerComponent:eventDraw(args)
  if not self:canDraw(args) then return end

  gui:draw()
end

function guiHandlerComponent:eventTextInput(args)
  if not args or not args[1] then return end

  if gui.focus then
    gui:textinput(args[1])
  end
end

function guiHandlerComponent:eventKeyPressed(args)
  if not args or not args[1] or not args[2] or not args[3] then return end

  if gui.focus then
    gui:keypress(args[1])
  else
    gui:feedback(args[1])
  end
end

function guiHandlerComponent:eventMousePressed(args)
  if not args or not args[1] or not args[2] or not args[3] then return end

  gui:mousepress(args[1], args[2], args[3])
end

function guiHandlerComponent:eventMouseMoved(args)
  if not args or not args[1] or not args[2] then return end

  gui:mousemoved(args[1], args[2])
end

function guiHandlerComponent:eventMouseReleased(args)
  if not args or not args[1] or not args[2] or not args[3] then return end

  gui:mouserelease(args[1], args[2], args[3])
end

function guiHandlerComponent:eventWheelMoved(args)
  if not args or not args[1] or not args[2] then return end

  gui:mousewheel(args[1], args[2])
end

lowerEventName(guiHandlerComponent)

return guiHandlerComponent
