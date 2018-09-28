var path = '../../Default Components/UI/';

function loadComponents()
{
  function loadScript(source)
  {
    var head = document.body;
    var script = document.createElement("script");
    script.src = source;
    head.appendChild(script);
  }

  var components =
  [
    "../../ecsevent.js",
    "../../ecsobject.js",
    "../../component.js",
    "../../entity.js",
    "../../system.js",
    "../../systemList.js",
    "../game.js",
    "../../Default Components/Interfaces/drawableComponent.js",
    "../../Default Components/Interfaces/coroutineScheduler.js",
    "../../Default Components/UI/guiComponent.js",
    'test.js'
  ];

  for(var index in components)
  {
    loadScript(components[index]);
  }
}
window.addEventListener("load", loadComponents);
