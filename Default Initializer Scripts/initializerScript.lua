return function(isDebug)
	local System = require(isDebug and "debugSystem" or "system")
	return System("Default")
end