class OptionElement extends ButtonElement
{
  constructor(label, pos, parent, value)
  {
    super(label, pos, parent);
    this.value = value;
  }

  click()
  {
    this.parent.value = this.value;
  }
}
