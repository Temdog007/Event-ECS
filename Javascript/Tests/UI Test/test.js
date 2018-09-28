window.addEventListener('guiLoaded', function()
{
  var system = Systems.addSystem(new System("GUI Test"));
  var entity = system.createEntity();
  var component = entity.addComponent(GuiComponent);

  // var textout = new TypeTextElement(component,
  //   "This is some text that will be typed out overtime. This text will not be wrapped.");
  // textout.y = 32;
  // textout.width = 128;

  var button = new ButtonElement(component, 'A Button', null, null, true);
  button.x = 128;
  button.y = button.unit;
  button.width = 128;
  button.height = button.unit;
  button.fitWidth = true;
  button.click = function()
  {
    new FeedbackElement(component, "Clicky");
  };

  var image = new ImageElement(component, "An Image");
  image.img = document.getElementById("testimage");
  image.x = 160;
  image.y = 32;
  image.width = image.img.width;
  image.height = image.img.height;
  image.click = function()
  {
    new FeedbackElement(component, this.pos.toString());
  };
  image.enter = function()
  {
    new FeedbackElement(this.guiComponent, "I'm In!");
  };
  image.leave = function()
  {
    new FeedbackElement(this.guiComponent, "I'm Out!");
  };

  var hidden = new HiddenElement(component, '');
  hidden.x = 128;
  hidden.y = 128;
  hidden.width = 128;
  hidden.height = 128;
  hidden.tip = "Can't see me, but I still respond";

  var group1 = new CollapseGroupElement(component, 'Group 1');
  group1.fg = 'rgb(255,192,0)';
  group1.x = group1.unit;
  group1.y = group1.unit * 3;
  group1.width = 128;
  group1.height = group1.unit;
  group1.tip = "Drag and Drop";
  group1.drag = true;
  group1.drop = function(bucket)
  {
    if(bucket)
    {
      new FeedbackElement(component, "Dropped on " + bucket.constructor.name);
    }
    else
    {
      new FeedbackElement(component, "Dropped on nothing");
    }
  }

  for(var i = 1; i < 4; ++i)
  {
    var option = new OptionElement(component, "Option " + i, null, group1, i);
    option.x = 0;
    option.y = option.unit * i;
    option.width = 128;
    option.height = option.unit;
    option.tip = "Select " + option.value;
  }

  var group2 = new GroupElement(component, "Group 2");
  group2.x = group2.unit;
  group2.y = 128;
  group2.width = 256;
  group2.height = 256;
  group2.drag = true;
  group2.tip = "Drag right-click, and catch";
  group2.rclick = function()
  {
    new FeedbackElement(component, 'Right-Click');
    var button = new ButtonElement(component, "A dynamic button");
    button.x = component.mx;
    button.y = component.my;
    button.width = 128;
    button.height = component.style.unit;
    button.click = function()
    {
      new FeedbackElement(component, "I'll be back!");
      component.rem(this);
    }
  };
  group2.catch = function(ball)
  {
    new FeedbackElement(component, "Caught " + ball.constructor.name);
  };

  var scrollgroup = new ScrollGroupElement(component, null, null, group2);
  scrollgroup.y = scrollgroup.unit;
  scrollgroup.width = 256;
  scrollgroup.height = 256;

  scrollgroup.scrollh.tip = "Scroll (mouse or wheel)";
  scrollgroup.scrollh.hs = scrollgroup.style.unit * 2;
  scrollgroup.scrollv.tip = scrollgroup.scrollh.tip;
  scrollgroup.scrollh.drop = function()
  {
    new FeedbackElement(component, "Scrolled to : " + this.values.current + " / " + this.values.min + " - " + this.values.max);
  };
  scrollgroup.scrollv.drop = function() {this.parent.scrollh.drop();};
  scrollgroup.scrollv.hs = "auto";

  var checkbox = new CheckboxElement(component, null, null, scrollgroup);
  checkbox.shape = "circle";
  checkbox.radius = 8;
  checkbox.click = function()
  {
    CheckboxElement.prototype.click.call(this);
    this.fg = this.value ? "rgb(255,128,0)" : "rgb(255,255,255)";
  };
  checkbox.label = 'check';
});
