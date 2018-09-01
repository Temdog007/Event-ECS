
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
  assertIs(inverseColor, "function")
  assertEquals({inverseColor(1,1,1)}, {0,0,0})
  assertEquals({inverseColor("white")}, {0,0,0})
  assertError(inverseColor, "error31254")
  assertEquals({inverseColor({1,1,1})}, {0,0,0})

  assertEquals({inverseColor(0.5,0.65,0.25, 12, "dfkal3")}, {0.5, .35, .75})
end

function colorTests.testAllColors()
  assertIs(colors.getAllColors, "function")
  local tab = colors.getAllColors()
  assertIs(tab, "table")
  assertIsTrue(#tab > 0)
  for k,v in pairs(tab) do
    -- print(v.name)
    print(string.format("%d (%s) => {%2.2f, %2.2f, %2.2f, %2.2f}", k, v.name, v.color[1], v.color[2], v.color[3], v.color[4]))
  end
end

LuaUnit:run('colorTests')
