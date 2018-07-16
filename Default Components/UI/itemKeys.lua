local valueKeys = {"x", "y", "width", "height"}

return
{
    init = function(self)
      self.lastValues = {}
      for _, value in pairs(valueKeys) do
        self.lastValues[value] = self[value]
      end
    end,

    update = function(self)
      for _, value in pairs(valueKeys) do
        if self[value] ~= self.lastValues[value] then
          self:getSystem():dispatchEvent("eventItemChanged", {item = self})
          break
        end
      end
    end,

    itemChanged = function(self, args)
      if not args or args.component ~= self then return end

      self.lastValues.x = self.x
      self.lastValues.y = self.y
      self.lastValues.width = self.width
      self.lastValues.height = self.height
    end
}
