define(['component', 'systemlist'], function(Component, Systems)
{
  function updateSystemForeground()
  {
    this.style.backgroundColor = this.ecsObject.enabled ? "silver" : "dimGray";
  }

  function toggleEnabled(evt)
  {
    var button = evt.srcElement;
    button.ecsObject.enabled = !button.ecsObject.enabled;
    button.updateForeground();
  }

  function createToggleButton(ecsObject)
  {
    var toggleButton = document.createElement("button");
    toggleButton.ecsObject = ecsObject;
    toggleButton.style.textAlign = "center";
    toggleButton.updateForeground = updateSystemForeground;
    toggleButton.textContent = ecsObject.name + " (ID " + ecsObject.id + ")";
    toggleButton.addEventListener("click", toggleEnabled);
    toggleButton.updateForeground();
    return toggleButton;
  }

  function updateEntityDivVisibility()
  {
    var system;
    var radioButtons = document.getElementsByClassName("SystemRadioButton");
    for(var i in radioButtons)
    {
      var button = radioButtons[i];
      if(button.checked)
      {
        system = button.system;
        break;
      }
    }

    var divs = document.getElementsByClassName("Entity");
    for(var i in divs)
    {
      var div = divs[i];
      if(div.style)
      {
        div.style.display = div.system == system ? "block" : "none";
      }
    }
  }

  function updateEntity(entity)
  {
    var entityDiv = document.getElementById(entity.id);
    if(!entityDiv)
    {
      entityDiv = document.createElement("div");
      entityDiv.id = entity.id;
      entityDiv.className = "Entity";
      entityDiv.style.display = "none";

      entityDiv.appendChild(createToggleButton(entity));

      var en = document.createElement("h3");
      en.innerHTML = "Data";
      entityDiv.appendChild(en);

      var ul = document.createElement("ul");
      ul.className = "Enttiy Data";
      entityDiv.appendChild(ul);

      for(var k in entity.data)
      {
        var d = document.createElement("li");
        d.innerHTML = k + ": " + entity.data[k];
        ul.appendChild(d);
      }

      en = document.createElement("h3");
      en.innerHTML = "Components";
      entityDiv.appendChild(en);

      ul = document.createElement("ul");
      ul.className = "Enttiy Components";
      entityDiv.appendChild(ul);
      for(var i in entity._components)
      {
        var d = document.createElement("li");
        d.appendChild(createToggleButton(entity._components[i]));
        ul.appendChild(d);
      }

      entitiesContent.appendChild(entityDiv);
    }

    return entityDiv;
  }

  function updateSystem(system)
  {
    var systemDiv = document.getElementById(system.id);
    if(!systemDiv)
    {
      systemDiv = document.createElement("div");
      systemDiv.id = system.id;
      systemDiv.className = "System";

      var radioButton = document.createElement("input");
      radioButton.type = "radio";
      radioButton.name = "System";
      radioButton.system = system;
      radioButton.className = "SystemRadioButton";
      radioButton.addEventListener("click", updateHTML);
      systemDiv.appendChild(radioButton);

      systemDiv.appendChild(createToggleButton(system));

      systemsContent.appendChild(systemDiv);
    }

    for(var i in system._entities)
    {
      var div = updateEntity(system._entities[i]);
      div.system = system;
    }
  }

  function updateHTML()
  {
    Systems.forEachSystem(updateSystem);
    updateEntityDivVisibility();
  }

  class DataDisplayerComponent extends Component
  {
    constructor(entity)
    {
      super(entity);

      this.setDefaults({
        updateRate : 1000,
        current : 0,
        autoUpdate : false
      });

      var comp = this;
      updateButton.addEventListener("click", updateHTML);
      autoUpdateCheckbox.addEventListener("click", function(evt)
      {
        var checkbox = evt.srcElement;
        comp.data.autoUpdate = checkbox.checked;
      });
      autoUpdateInterval.addEventListener("change", function(evt)
      {
        var text = evt.srcElement;
        var value = text.value;
        if(value)
        {
          comp.data.current = 0;
          comp.data.updateRate = value;
        }
      });
      eventDispatcher.addEventListener("keyup", function(evt)
      {
        evt.preventDefault();
        if(evt.keyCode == 13)
        {
          Systems.pushEvent(evt.srcElement.value);
        }
      });
    }

    eventUpdate(args)
    {
      var data = this.data;
      if(!data.autoUpdate){return;}

      data.current += args.dt;
      if(data.current > data.updateRate)
      {
        updateHTML();
        data.current = 0;
      }
    }

    get systemData()
    {
      return Component.Systems.toSimpleObject();
    }

    get systemDataStr()
    {
      return this.systemData.toString();
    }
  }

  return DataDisplayerComponent;
});
