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
  return  string.sub(str, -string.len(endStr)) == endStr
end

function lowerEventName(t)
  for k,v in pairs(t) do
    if string.starts(k, "event") and type(v) == "function" then
      t[string.lower(k)] = v
    end
  end
end
