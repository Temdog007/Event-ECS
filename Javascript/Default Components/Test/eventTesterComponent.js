class EventTesterComponent extends Component
{
  eventMouseMoved(args)
  {
    console.log("%c" + args.x, "color:blue");
    console.log("%c" + args.y, "color:cyan");
  }

  eventMouseDown(args)
  {
    console.log("%c" + args.which, "color:orange");
  }

  eventMouseUp(args)
  {
    console.log("%c" + args.which, "color:orangered");
  }

  eventKeyDown(args)
  {
    console.log(args);
  }

  eventKeyUp(args)
  {
    console.log(args);
  }
}
