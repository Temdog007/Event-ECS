define(['textElement'], function(TextElement)
{
  class TypeTextElement extends TextElement
  {
    constructor(label, pos, parent)
    {
      super('', pos, parent);
      this.values = {text : label, cursor : 0};
      this.updateInterval = 0.1;
    }

    update(dt)
    {
      this.values.cursor = TextElement.utf8char_after(this.values.text, this.values.cursor + 1) - 1;
      this.label = this.values.text.substring(0, this.values.cursor);
    }
  }
  return TypeTextElement;
});
