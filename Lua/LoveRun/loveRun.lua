local systems = require("systemList")

local frameRate = 60
local drawOrders = {}
local ordersSet = {}

function setFrameRate(fps)
  assert(type(fps) == "number" and fps > 0, "FPS must be a number greater than 0")
  frameRate = fps
end

function addDrawOrder(order)
  assert(type(order) == "number", "Must add a number as a draw order")
  assert(not ordersSet[order], string.format("Draw order %d is already in the set", order))
  table.insert(drawOrders, order)
  ordersSet[order] = true
  table.sort(drawOrders)
end

function love.run()
  addDrawOrder(0)
  systems.pushEvent('eventload', love.arg.parseGameArguments(arg), arg)

  local nextTime

  -- We don't want the first frame's dt to include time taken by love.load.
  if love.timer then
    love.timer.step()
    nextTime = love.timer.getTime()
  end

  local updateArgs = {dt = 0}
  local quitArgs = {handled = false}
  local drawArgs = {drawOrder = 0}

  -- Main loop time.
  return function()
  	-- Process events.
  	if love.event then
  		love.event.pump()
  		for name, a,b,c,d,e,f in love.event.poll() do
  			if name == 'quit' then
          systems.pushEvent('eventquit', quitArgs)
          systems.flushEvents()
  				if not quitArgs.handled then
  					return a or 0
  				end
  			end
  			systems.pushEvent('event'..name, {a,b,c,d,e,f})
  		end
  	end

  	-- Update dt, as we'll be passing it to update
  	if love.timer then updateArgs.dt = love.timer.step() end

  	-- Call update and draw
  	systems.pushEvent('eventupdate', updateArgs) -- will pass 0 if love.timer is disabled
    systems.flushEvents()

  	if love.graphics and love.graphics.isActive() then
  		love.graphics.origin()
  		love.graphics.clear(love.graphics.getBackgroundColor())

      for _, o in pairs(drawOrders) do
        drawArgs.drawOrder = o
		    systems.pushEvent('eventdraw', drawArgs)
        systems.flushEvents()
      end
  		love.graphics.present()
  	end


  	if love.timer then
      nextTime = nextTime + (1 / frameRate)
      local curTime = love.timer.getTime()
      if nextTime <= curTime then
        nextTime = curTime
      else
        love.timer.sleep(nextTime - curTime)
      end
    end
  end
end
