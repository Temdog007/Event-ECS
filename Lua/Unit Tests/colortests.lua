-- local colors = require("colorComponent")
--
-- for k,v in pairs(colors.getColors()) do
--   print(string.format("{\"%s\", {%f, %f, %f, %f}},", k, v[1], v[2], v[3], v[4] or 1))
-- end

local LuaUnit = require("luaunit")
local Colors = require("eventecscolors")
require("Unit Tests/assertions")

colorTests = {}

function colorTests.testGetColor()
  assertEquals({Colors.getColor("white")}, {1,1,1,1})
  assertEquals({Colors.getColor("black")}, {0,0,0,1})
  assertEquals({Colors.getColor("red")}, {1,0,0,1})
  assertNotEquals({Colors.getColor("red")}, {1,0,0})
  assertError(Colors.getColor, "error")
end

function colorTests.testGetColors()
  local list = Colors.getColors()
  assertNotIsNil(list)
  assertEquals(list.white, {1,1,1,1})
  assertEquals(list.black, {0,0,0,1})
  assertEquals(list.red, {1,0,0,1})
  assertNotEquals(list.red, {1,0,0})
  assertIsNil(list.error)
end

function colorTests.testAddColor()
  assertIs(Colors.addColor, "function")
  Colors.addColor("darkBrown", 0.5, 0.25, 0.1)
  local list = Colors.getColors()
  assertNotIsNil(list.darkBrown)
  assertEquals(list.darkBrown, {0.5, 0.25, 0.1, 1})
  assertNotEquals(list.darkBrown, {0.5, 0.25, 0.1})
  assertError(Colors.addColor, "error", 324,1231,"L")
  assertError(Colors.addColor, list, 324,1231,"L")
  assertError(Colors.addColor, "error", 1)
  assertError(Colors.addColor, "error", 1, 2)
  assertError(Colors.addColor, "darkBrown", 0.5, 0.25, 0.1)
end

function colorTests.testInverse()
  assertIs(Colors.inverseColor, "function")
  assertEquals({Colors.inverseColor(1,1,1)}, {0,0,0})
  assertEquals({Colors.inverseColor("white")}, {0,0,0})
  assertError(Colors.inverseColor, "error31254")
  assertEquals({Colors.inverseColor({1,1,1})}, {0,0,0})

  assertEquals({Colors.inverseColor(0.5,0.65,0.25, 12, "dfkal3")}, {0.5, .35, .75})
end

LuaUnit:run('colorTests')
