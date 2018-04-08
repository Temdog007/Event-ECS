-- Copyright (c) 2018 Temdog007
--
-- Permission is hereby granted, free of charge, to any person obtaining a copy
-- of this software and associated documentation files (the "Software"), to deal
-- in the Software without restriction, including without limitation the rights
-- to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
-- copies of the Software, and to permit persons to whom the Software is
-- furnished to do so, subject to the following conditions:
--
-- The above copyright notice and this permission notice shall be included in all
-- copies or substantial portions of the Software.
--
-- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
-- IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
-- FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
-- AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
-- LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
-- OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
-- SOFTWARE.

return function(identity)
  local System = require("system")

  assert(type(identity) == "string", "Must enter a name for the game")

  local loveSystem = System()

  -- Make sure love exists.
  local love = require("love")
  require("love.filesystem")

  local function getLow(a)
  	local m = math.huge
  	for k,v in pairs(a) do
  		if k < m then
  			m = k
  		end
  	end
  	return a[m], m
  end

  local arg0 = getLow(arg)
  love.filesystem.init(arg0)

  local exepath = love.filesystem.getExecutablePath()
  love.filesystem.setFused(false)
  love.setDeprecationOutput(true)
  love.filesystem.setIdentity(identity)

  -- Create default configuration settings.
  -- NOTE: Adding a new module to the modules list
  -- will NOT make it load, see below.
  local c = {
  	title = identity,
  	version = love._version,
  	window = {
  		width = 800,
  		height = 600,
  		x = nil,
  		y = nil,
  		minwidth = 1,
  		minheight = 1,
  		fullscreen = false,
  		fullscreentype = "desktop",
  		display = 1,
  		vsync = 1,
  		msaa = 0,
  		borderless = false,
  		resizable = false,
  		centered = true,
  		highdpi = false,
  	},
  	modules = {
  		data = true,
  		event = true,
  		keyboard = true,
  		mouse = true,
  		timer = true,
  		joystick = true,
  		touch = true,
  		image = true,
  		graphics = true,
  		audio = true,
  		math = true,
  		physics = true,
  		sound = true,
  		system = true,
  		font = true,
  		thread = true,
  		window = true,
  		video = true,
  	},
  	audio = {
  		mixwithsystem = true, -- Only relevant for Android / iOS.
  	},
  	console = false, -- Only relevant for windows.
  	identity = false,
  	appendidentity = false,
  	externalstorage = false, -- Only relevant for Android.
  	accelerometerjoystick = true, -- Only relevant for Android / iOS.
  	gammacorrect = false,
  }

  local conf = require("conf")
  if conf then
    conf(c)
  end

  -- Hack for disabling accelerometer-as-joystick on Android / iOS.
  if love._setAccelerometerAsJoystick then
  	love._setAccelerometerAsJoystick(c.accelerometerjoystick)
  end

  if love._setGammaCorrect then
  	love._setGammaCorrect(c.gammacorrect)
  end

  -- Gets desired modules.
  for k,v in ipairs{
  	"data",
  	"thread",
  	"timer",
  	"event",
  	"keyboard",
  	"joystick",
  	"mouse",
  	"touch",
  	"sound",
  	"system",
  	"audio",
  	"image",
  	"video",
  	"font",
  	"window",
  	"graphics",
  	"math",
  	"physics",
  } do
  	if c.modules[v] then
  		require("love." .. v)
  	end
  end

  -- Check the version
  c.version = tostring(c.version)
  if not love.isVersionCompatible(c.version) then
  	local major, minor, revision = c.version:match("^(%d+)%.(%d+)%.(%d+)$")
  	if (not major or not minor or not revision) or (major ~= love._version_major and minor ~= love._version_minor) then
  		local msg = ("This game indicates it was made for version '%s' of LOVE.\n"..
  			"It may not be compatible with the running version (%s)."):format(c.version, love._version)

  		print(msg)

  		if love.window then
  			love.window.showMessageBox("Compatibility Warning", msg, "warning")
  		end
  	end
  end

  -- Setup window here.
  if c.window and c.modules.window then
  	assert(love.window.setMode(c.window.width, c.window.height,
  	{
  		fullscreen = c.window.fullscreen,
  		fullscreentype = c.window.fullscreentype,
  		vsync = c.window.vsync,
  		msaa = c.window.msaa,
  		stencil = c.window.stencil,
  		depth = c.window.depth,
  		resizable = c.window.resizable,
  		minwidth = c.window.minwidth,
  		minheight = c.window.minheight,
  		borderless = c.window.borderless,
  		centered = c.window.centered,
  		display = c.window.display,
  		highdpi = c.window.highdpi,
  		x = c.window.x,
  		y = c.window.y,
  	}), "Could not set window mode")
  	love.window.setTitle(c.window.title or c.title)
  	if c.window.icon then
  		assert(love.image, "If an icon is set in love.conf, love.image must be loaded!")
  		love.window.setIcon(love.image.newImageData(c.window.icon))
  	end
  end

  if love.audio then
  	love.audio.setMixWithSystem(c.audio.mixwithsystem)
  end

  -- Our first timestep, because window creation can take some time
  if love.timer then
  	love.timer.step()
  end

  if love.filesystem then
  	love.filesystem._setAndroidSaveExternal(c.externalstorage)
  	love.filesystem.setIdentity(identity, true)
  end

  -----------------------------------------------------------
  -- Default callbacks.
  -----------------------------------------------------------

  local function run()

  	if love.timer then love.timer.step() end

  	local updateArgs = {dt = 0}
    local quitArgs = {handled = false}

  	-- Main loop time.
  	return function()
  		-- Process events.
  		if love.event then
  			love.event.pump()
  			for name, a,b,c,d,e,f in love.event.poll() do
          print(name)
  				if name == "quit" then
            quitArgs.handled = false
  					loveSystem:dispatchEvent("eventquit", quitArgs)
            if not quitArgs.handled then
  						return a or 0
  					end
  				else
            loveSystem:dispatchEvent("event"..name, {a,b,c,d,ef})
          end
  			end
  		end

      -- Update dt, as we'll be passing it to update
      if love.timer then
        updateArgs.dt = love.timer.step()
      else
        updateArgs.dt = 0
      end

      -- Call update and draw
      loveSystem:dispatchEvent("eventUpdate", updateArgs) -- will pass 0 if love.timer is disabled

      if love.graphics and love.graphics.isActive() then
      	love.graphics.origin()
      	love.graphics.clear(love.graphics.getBackgroundColor())

      	loveSystem:dispatchEvent("eventdraw")

      	love.graphics.present()
      end

      if love.timer then love.timer.sleep(0.001) end
    end
  end

  local debug, print, error = debug, print, error

  local utf8 = require("utf8")

  local function error_printer(msg, layer)
  	print((debug.traceback("Error: " .. tostring(msg), 1+(layer or 1)):gsub("\n[^\n]+$", "")))
  end

  local function errorHandler(msg)
  	msg = tostring(msg)

  	error_printer(msg, 2)

  	if not love.window or not love.graphics or not love.event then
  		return
  	end

  	if not love.graphics.isCreated() or not love.window.isOpen() then
  		local success, status = pcall(love.window.setMode, 800, 600)
  		if not success or not status then
  			return
  		end
  	end

  	-- Reset state.
  	if love.mouse then
  		love.mouse.setVisible(true)
  		love.mouse.setGrabbed(false)
  		love.mouse.setRelativeMode(false)
  		if love.mouse.isCursorSupported() then
  			love.mouse.setCursor()
  		end
  	end
  	if love.joystick then
  		-- Stop all joystick vibrations.
  		for i,v in ipairs(love.joystick.getJoysticks()) do
  			v:setVibration()
  		end
  	end
  	if love.audio then love.audio.stop() end

  	love.graphics.reset()
  	local font = love.graphics.setNewFont(14)

  	love.graphics.setColor(1, 1, 1, 1)

  	local trace = debug.traceback()

  	love.graphics.origin()

  	local sanitizedmsg = {}
  	for char in msg:gmatch(utf8.charpattern) do
  		table.insert(sanitizedmsg, char)
  	end
  	sanitizedmsg = table.concat(sanitizedmsg)

  	local err = {}

  	table.insert(err, "Error\n")
  	table.insert(err, sanitizedmsg)

  	if #sanitizedmsg ~= #msg then
  		table.insert(err, "Invalid UTF-8 string in error message.")
  	end

  	table.insert(err, "\n")

  	for l in trace:gmatch("(.-)\n") do
  		if not l:match("boot.lua") then
  			l = l:gsub("stack traceback:", "Traceback\n")
  			table.insert(err, l)
  		end
  	end

  	local p = table.concat(err, "\n")

  	p = p:gsub("\t", "")
  	p = p:gsub("%[string \"(.-)\"%]", "%1")

  	local function draw()
  		local pos = 70
  		love.graphics.clear(89/255, 157/255, 220/255)
  		love.graphics.printf(p, pos, pos, love.graphics.getWidth() - pos)
  		love.graphics.present()
  	end

  	local fullErrorText = p
  	local function copyToClipboard()
  		if not love.system then return end
  		love.system.setClipboardText(fullErrorText)
  		p = p .. "\nCopied to clipboard!"
  		draw()
  	end

  	if love.system then
  		p = p .. "\n\nPress Ctrl+C or tap to copy this error"
  	end

  	return function()
  		love.event.pump()

  		for e, a, b, c in love.event.poll() do
  			if e == "quit" then
  				return 1
  			elseif e == "keypressed" and a == "escape" then
  				return 1
  			elseif e == "keypressed" and a == "c" and love.keyboard.isDown("lctrl", "rctrl") then
  				copyToClipboard()
  			elseif e == "touchpressed" then
  				local name = love.window.getTitle()
  				if #name == 0 or name == "Untitled" then name = "Game" end
  				local buttons = {"OK", "Cancel"}
  				if love.system then
  					buttons[3] = "Copy to clipboard"
  				end
  				local pressed = love.window.showMessageBox("Quit "..name.."?", "", buttons)
  				if pressed == 1 then
  					return 1
  				elseif pressed == 3 then
  					copyToClipboard()
  				end
  			end
  		end

  		draw()

  		if love.timer then
  			love.timer.sleep(0.1)
  		end
  	end

  end

  -----------------------------------------------------------
  -- The root of all calls.
  -----------------------------------------------------------

  function loveSystem:run()
  	local func

  	local function deferErrhand(...)
  		func = errorHandler(...)
  	end

  	local function earlyinit()
  		-- NOTE: We can't assign to func directly, as we'd
  		-- overwrite the result of deferErrhand with nil on error
  		local main
  		result, main = xpcall(run, deferErrhand)
  		if result then
  			func = main
  		end
  	end

  	func = earlyinit

  	while func do
  		local _, retval = xpcall(func, deferErrhand)
  		if retval then return retval end
  		coroutine.yield()
  	end

  	return 1
  end
  return loveSystem
end
