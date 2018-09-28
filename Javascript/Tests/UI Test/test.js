window.addEventListener('guiLoaded', function()
{
  var system = Systems.addSystem(new System("GUI Test"));
  var entity = system.createEntity();
  var component = entity.addComponent(GuiComponent);
  var text = new TextElement(component);
  text.width = 300;
  text.label = "This is test text";
});
