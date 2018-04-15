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

local System = require("system")
local loveSystem
return function(identity, executablePath, frameRate)

  if loveSystem then
    return loveSystem
  end

  executablePath = executablePath or os.execute('cd')
  assert(type(identity) == "string", "Must enter a name for the game")

  loveSystem = System()

  loveSystem.frameRate = frameRate or 60

  -- Make sure love exists.
  local love = require("love")
  require("love.filesystem")

  love.filesystem.init(executablePath)

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

    loveSystem:dispatchEvent("eventload")

    local nextTime
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
        nextTime = nextTime + (1 / loveSystem.frameRate)
        updateArgs.dt = love.timer.step()
      else
        updateArgs.dt = 0
      end

      -- Call update and draw
      loveSystem:dispatchEvent("eventupdate", updateArgs) -- will pass 0 if love.timer is disabled

      if love.graphics and love.graphics.isActive() then
      	love.graphics.origin()
      	love.graphics.clear(love.graphics.getBackgroundColor())

      	loveSystem:dispatchEvent("eventdraw")

      	love.graphics.present()
      end

      if love.timer then
        local curTime = love.timer.getTime()
        if nextTime <= curTime then
          nextTime = curTime
        else
          love.timer.sleep(nextTime - curTime)
        end
      end

    end
  end

  local debug, print, error = debug, print, error

  local utf8 = require("utf8")

  local function error_printer(msg, layer)
    if Log then
      Log("high", (debug.traceback("Error: " .. tostring(msg), 1+(layer or 1)):gsub("\n[^\n]+$", "")))
    else
  	   print((debug.traceback("Error: " .. tostring(msg), 1+(layer or 1)):gsub("\n[^\n]+$", "")))
     end
  end

  -----------------------------------------------------------
  -- The root of all calls.
  -----------------------------------------------------------

  local function runCoroutine()
    local func

    local function deferErrhand(...)
      func = error_printer(...)
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

  local co = coroutine.create(runCoroutine)
  function loveSystem:run()

    if not co then
      return false
    end

  	local rval, result = coroutine.resume(co)
    if not rval then
      print(result)
      co = nil
    end

    return rval
  end

  return loveSystem
end
