window.addEventListener('guiLoaded', function()
{
  var system = Systems.addSystem(new System("GUI Test"));
  var entity = system.createEntity();
  var component = entity.addComponent(GuiComponent);

  var sometext = "This is some text that will be typed out overtime. This text will not be wrapped.";
  var textout = new TypeTextElement(sometext, {x : canvas.height / 2, width : canvas.height / 2});
  textout.fitWidth = true;

  var button = new ButtonElement('A Button', null, null, true);
  button.x = 128;
  button.y = button.unit;
  button.width = 128;
  button.height = button.unit;
  button.fitWidth = true;
  button.click = function()
  {
    new FeedbackElement("Clicky");
  };

  var image = new ImageElement("An Image");
  image.img = document.getElementById("testimage");
  image.x = 160;
  image.y = 32;
  image.width = image.img.width;
  image.height = image.img.height;
  image.click = function(x,y)
  {
    new FeedbackElement("["+ x + "," + y + "]");
  };
  image.enter = function()
  {
    new FeedbackElement("I'm In!");
  };
  image.leave = function()
  {
    new FeedbackElement("I'm Out!");
  };

  var hidden = new HiddenElement('');
  hidden.x = 128;
  hidden.y = 128;
  hidden.width = 128;
  hidden.height = 128;
  hidden.tip = "Can't see me, but I still respond";

  var group1 = new CollapseGroupElement('Group 1');
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
      new FeedbackElement("Dropped on " + bucket.constructor.name);
    }
    else
    {
      new FeedbackElement("Dropped on nothing");
    }
  }

  for(var i = 1; i < 4; ++i)
  {
    var option = new OptionElement("Option " + i, null, group1, i);
    option.x = 0;
    option.y = option.unit * i;
    option.width = 128;
    option.height = option.unit;
    option.tip = "Select " + option.value;
  }

  var group2 = new GroupElement("Group 2");
  group2.x = group2.unit;
  group2.y = 128;
  group2.width = 256;
  group2.height = 256;
  group2.drag = true;
  group2.tip = "Drag right-click, and catch";
  group2.rclick = function()
  {
    new FeedbackElement('Right-Click');
    var button = new ButtonElement("A dynamic button");
    button.x = component.mx;
    button.y = component.my;
    button.width = 128;
    button.height = component.style.unit;
    button.click = function()
    {
      new FeedbackElement("I'll be back!");
      component.rem(this);
    }
  };
  group2.catch = function(ball)
  {
    new FeedbackElement("Caught " + ball.constructor.name);
  };

  var scrollgroup = new ScrollGroupElement(null, {y : component.style.unit, width : 256, height : 256}, group2);

  scrollgroup.scrollh.tip = "Scroll (mouse or wheel)";
  scrollgroup.scrollh.hs = scrollgroup.style.unit * 2;
  scrollgroup.scrollv.tip = scrollgroup.scrollh.tip;
  var d = function(scroll)
  {
    new FeedbackElement("Scrolled to : " + scroll.values.current + " / " + scroll.values.min + " - " + scroll.values.max);
  }
  scrollgroup.scrollh.drop = function(){d(this);};
  scrollgroup.scrollv.drop = function(){d(this);};
  scrollgroup.scrollv.hs = "auto";

  var checkbox = new CheckboxElement(null, null, scrollgroup);
  checkbox.shape = "circle";
  checkbox.radius = 8;
  checkbox.click = function()
  {
    CheckboxElement.prototype.click.call(this);
    this.fg = this.value ? "rgb(255,128,0)" : "rgb(255,255,255)";
  };

  var checklabel = new TextElement('check', null, checkbox, true);
  checklabel.x = 16;
  checklabel.click = function()
  {
    this.parent.click();
  };

  var loader = new ProgressElement('Loading', {y : 16, width : 128}, scrollgroup);
  loader.updateInterval = 0.25;
  loader.fitWidth = true;
  loader.labelfg = "red";
  loader.done = function()
  {
    this.replace(new FeedbackElement('Done', {y : this.y}, this.parent, false));
  };

  for(var i = 0; i < 20; ++i)
  {
    loader.add(function()
    {
      var text = new TextElement(sometext, {width : 128}, null, true);
      return scrollgroup.addChild(text, 'grid');
    });
  }

  button = new ButtonElement("up", {x : group2.width}, group2);
  button.click = function()
  {
    var scroll = scrollgroup.scrollv;
    scroll.current = Math.max(scroll.min, scroll.current - scroll.values.step);
    scroll.drop();
  };
  button = new ButtonElement("dn", {x : group2.width, y : group2.height + group2.unit}, group2);
  button.click = function()
  {
    var scroll = scrollgroup.scrollv;
    scroll.current = Math.min(scroll.max, scroll.current + scroll.values.step);
    scroll.drop();
  };

  var input = new InputElement('Chat', {x : 64, y : canvas.height - 32, width : 256, height : component.style.unit});
  input.done = function()
  {
    new FeedbackElement('I say ' + this.value);
    this.value = '';
    this.guiComponent.unfocus();
  };

  button = new ButtonElement('Speak', {x : input.width + input.style.unit, width : 64, height : input.style.gui}, input);
  button.click = function()
  {
    this.parent.done();
  };

  text = new TextElement('Hit F1 to show/hide',
    {x : canvas.width - 128, y : button.style.unit, width : 128, height : button.style.unit});
  var showhider = new GroupElement("Mouse Below",
    {x : canvas.width - 128, y : button.style.unit * 2, width : 128, height : 64});
  var counter = new TextElement('0', {y : button.style.unit, width : 128, height : 0}, showhider);
  counter.count = 0;
  counter.update = function(dt)
  {
    if(this.guiComponent.mousein == this || this.guiComponent.mousein == this.parent)
    {
      this.count += dt;
      if(this.count > 1)
      {
        this.count = 0;
      }
      this.label = this.count;
    }
  }
  showhider.hide();
});
