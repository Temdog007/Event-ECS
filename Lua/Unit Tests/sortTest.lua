local function tabPrint(t)
  for k,v in pairs(t) do print(k,v) end
  print()
end

local test ={3,5,234,123,52,6435,2}
tabPrint(test)
table.sort(test)
tabPrint(test)
