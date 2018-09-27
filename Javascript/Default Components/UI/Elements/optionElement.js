class OptionElement extends ButtonElement
{
  constructor(guiComponent, label, pos, parent, value)
  {
    super(guiComponent, label, pos, parent);
    this.value = value;
  }

  click()
  {
    this.parent.value = this.value;
  }
}
