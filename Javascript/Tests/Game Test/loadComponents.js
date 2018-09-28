function loadComponents()
{
  function loadScript(source)
  {
    var head = document.body;
    var script = document.createElement("script");
    script.src = source;
    script.async = "";
    head.appendChild(script);
  }

  var components = [
    "../../ecsevent.js",
    "../../ecsobject.js",
    "../../component.js",
    "../../entity.js",
    "../../system.js",
    "../../systemList.js",
    "../game.js",
    "../../Default Components/Interfaces/drawableComponent.js",
    "../../Default Components/Interfaces/coroutineScheduler.js",
    "../../Default Components/Interfaces/graphableComponent.js",
    "../../Default Components/Drawing/imageDrawerComponent.js",
    "../../Default Components/Test/logTestComponent.js",
    "../../Default Components/Test/eventTesterComponent.js",
    "../../Default Components/Test/coroutineTesterComponent.js",
    "../../Default Components/Test/fpsGraphComponent.js",
    "../../Default Components/Test/fpsDisplayerComponent.js",
    'test.js'];

  for(var index in components)
  {
    loadScript(components[index]);
  }
}
window.addEventListener("load", loadComponents);
