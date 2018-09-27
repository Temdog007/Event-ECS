class ScrollValues
{
  constructor(values)
  {
    if(values)
    {
      this.min = values.min;
      this.max = values.max;
      this.current = values.current;
      this.step = values.step;
      this.axis = values.axis;
    }
    else
    {
      this.min = 0;
      this.max = 0;
      this.current = this.min;
      this.step = 16;
      this.axis = 'vertical';
    }
  }
}
