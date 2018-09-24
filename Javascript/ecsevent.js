class ECSEvent
{
  constructor(name, args)
  {
    this._name = name;
    this._args = args;
  }

  get name()
  {
    return this._name;
  }

  get args()
  {
    return this._args;
  }
}
