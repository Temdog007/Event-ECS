function string.starts(str, start)
  return string.sub(str, 1, string.len(start)) == start
end

function string.split(str, sep)
  local sep, fields = sep or "|", {}
  local pattern = string.format("([^%s]+)", sep)
  string.gsub(str, pattern, function(c)
    table.insert(fields, c)
  end)
  return fields
end

function string.ends(str, endStr)
  return string.sub(str, -string.len(endStr)) == endStr
end

function string.unCamelCase(str)
  local tab = {}
  for c in str:gmatch(".") do
    if #tab == 0 then
      table.insert(tab, string.upper(c))
    elseif c:match("^[A-Z]") then
      table.insert(tab, " ")
      table.insert(tab, string.upper(c))
    else
      table.insert(tab, c)
    end
  end
  return table.concat(tab, "")
end

function table.isEmpty(str)
  for _ in pairs(str) do
    return false
  end
  return true
end

function table.copy(t)
  local rval = setmetatable({}, getmetatable(t))
  for k,v in pairs(t) do
    rval[k] = v
  end
  return rval
end

function table.random(t)
  return t[love.math.random(#t)]
end

function table.contains(t, value)
  for k,v in pairs(t) do
    if k == value or v == value then
      return true
    end
  end
end

function lowerEventName(t)
  local temp = {}
  for k,v in pairs(t) do
    if string.starts(k, "event") and type(v) == "function" then
      temp[string.lower(k)] = v
    end
  end
  for k,v in pairs(temp) do
    if not t[k] then
      t[k] = v
    end
  end
end
