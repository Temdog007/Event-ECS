local frameRate = 60

function setFrameRate(fps)
  assert(type(fps) == "number" and fps > 0, "FPS must be a number greater than 0")
  frameRate = fps
end

function love.run()
	broadcastEvent('eventload', love.arg.parseGameArguments(arg), arg)

  local nextTime

	-- We don't want the first frame's dt to include time taken by love.load.
	if love.timer then
    love.timer.step()
    nextTime = love.timer.getTime()
  end

	local updateArgs = {dt = 0}
  local quitArgs = {handled = false}

	-- Main loop time.
	return function()
		-- Process events.
		if love.event then
			love.event.pump()
			for name, a,b,c,d,e,f in love.event.poll() do
				if name == 'quit' then
          broadcastEvent('eventquit', quitArgs)
					if not quitArgs.handled then
						return a or 0
					end
				end
				broadcastEvent('eventname', {a,b,c,d,e,f})
			end
		end

		-- Update dt, as we'll be passing it to update
		if love.timer then updateArgs.dt = love.timer.step() end

		-- Call update and draw
		broadcastEvent('eventupdate', updateArgs) -- will pass 0 if love.timer is disabled

		if love.graphics and love.graphics.isActive() then
			love.graphics.origin()
			love.graphics.clear(love.graphics.getBackgroundColor())

			broadcastEvent('eventdraw')

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
