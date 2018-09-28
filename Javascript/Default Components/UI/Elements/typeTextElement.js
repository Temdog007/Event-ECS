class TypeTextElement extends TextElement
{
  constructor(guiComponent, label, pos, parent)
  {
    super(guiComponent, label, pos, parent);
    this.values = {text : label, cursor : 1};
    this.updateInterval = 0.1;
  }

  update(dt)
  {
    this.values.cursor = TextElement.utf8char_after(this.values.text, this.values.cursor + 1) - 1;
    this.label = this.values.text.substring(0, this.values.cursor);
  }
}
