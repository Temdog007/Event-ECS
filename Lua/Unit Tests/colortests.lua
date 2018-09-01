
-- for k,v in pairs(colors.getColors()) do
--   print(string.format("{\"%s\", {%f, %f, %f, %f}},", k, v[1], v[2], v[3], v[4] or 1))
-- end

local LuaUnit = require("luaunit")
local colors = require("eventecscolors")
require("Unit Tests/assertions")

colorTests = {}

function colorTests.testGetColors()
  assertEquals(colors.white, {1,1,1,1})
  assertEquals(colors.black, {0,0,0,1})
  assertEquals(colors.BlAck, {0,0,0,1})
  assertEquals(colors.red, {1,0,0,1})
  assertNotEquals(colors.red, {1,0,0})
  assertIsNil(colors.error)
end

function colorTests.testAddColor()
  colors.darkBrown = {0.5, 0.25, 0.1, 1}

  assertEquals(colors.darkBrown, {0.5, 0.25, 0.1, 1})
  assertEquals(colors.DARKbrown, {0.5, 0.25, 0.1, 1})
  assertEquals(colors.darkbrown, {0.5, 0.25, 0.1, 1})
  assertNotEquals(colors.darkbrown, {0.5, 0.25, 0.1})

  colors.testColor = {}
  assertEquals(colors.testColor, {1,1,1,1})

  colors.testColor2 = 0xff00ffff
  assertEquals(colors.testColor2, {1,0,1,1})

  assertError(function() colors.error = {324,1231,"L"} end)
  assertError(function() colors.error = "L" end)
  assertError(function() colors.testColor = 1 end)
  assertError(function() colors.darkBrown = {0.5, 0.25, 0.1} end)
end

function colorTests.testInverse()
  assertIs(inverseColor, "nil")
  assertIs(colors.inverseColor, "function")
  assertEquals({colors.inverseColor(1,1,1)}, {0,0,0})
  assertEquals({colors.inverseColor("white")}, {0,0,0})
  assertError(colors.inverseColor, "error31254")
  assertEquals({colors.inverseColor({1,1,1})}, {0,0,0})

  assertEquals({colors.inverseColor(0.5,0.65,0.25, 12, "dfkal3")}, {0.5, .35, .75})
end

function colorTests.testColorToInt()
  assertIs(colors.colorToNumber, "function")
  assertError(colors.colorToNumber)
  assertError(colors.colorToNumber, 3, "54", "345rt2")
  assertNotEquals(colors.colorToNumber(0,0,1,1), 255)
  assertEquals(colors.colorToNumber(0,0,0), 255)
end

function colorTests.testAllColors()
  assertIs(colors.getAllColors, "function")

  local tab1 = colors.getAllColors(false)
  assertIs(tab1, "table")
  assertIsTrue(#tab1 > 0)
  -- for i,v in pairs(tab1) do
  --   print(string.format("%d (%s)", i, v.name))
  -- end

  local tab2 = colors.getAllColors(true)
  assertIs(tab2, "table")
  assertIsTrue(#tab2 > 0)
  for i,v in pairs(tab2) do
    print(string.format("%d (%s)", i, v.name))
  end
  assertNotEquals(tab1, tab2)
end

LuaUnit:run('colorTests')
