require(['systemlist'], function(Systems)
{
  function clear(evt)
  {
    while(systemsContent.hasChildNodes())
    {
      systemsContent.removeChild(systemsContent.lastChild);
    }
    while(entitiesContent.hasChildNodes())
    {
      entitiesContent.removeChild(entitiesContent.lastChild);
    }
  }

  refreshButton.addEventListener("click", clear);

  function getSelectedSystem()
  {
    var radioButtons = document.getElementsByClassName("SystemRadioButton");
    for(var i in radioButtons)
    {
      var button = radioButtons[i];
      if(button.checked)
      {
        return button.system;
      }
    }
  }

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

  function updateEntityData()
  {
    if(this.type == "checkbox")
    {
      this.entity.data[this.key] = this.checked;
    }
    else
    {
      this.entity.data[this.key] = this.value;
    }
  }

  function onEntityDataChange(evt)
  {
    var button = evt.srcElement;
    button.updateEntityData();
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

  function addComponent(evt)
  {
    if(evt.keyCode == 13)
    {
      var text = evt.srcElement;
      text.entity.addComponent(text.value);
      var system = getSelectedSystem();
      clear();
      updateHTML(system);
    }
  }

  function removeComponent(evt)
  {
    var button = evt.srcElement;
    console.assert(button.component.remove(), "Failed to remove component");
    var system = getSelectedSystem();
    clear();
    updateHTML(system);
  }

  function reloadComponent(evt)
  {
    var button = evt.srcElement;
    var name = button.component.name;
    var system = getSelectedSystem();
    console.assert(button.component.remove(), "Failed to remove component");
    require.undef(name);
    require([name.charAt(0).toLowerCase() + name.substring(1)], function(Component)
    {
      button.component = button.entity.addComponent(Component);
      console.log("Reloaded: " + Component.name);
      updateHTML(system);
    });
    clear();
  }

  function updateEntityDivVisibility()
  {
    var system = getSelectedSystem();

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
    if(entityDiv)
    {
      var inputs = entityDiv.getElementsByClassName("EntityDataValue");
      for(var i in inputs)
      {
        var input = inputs[i];
        if(input.key && input.entity)
        {
          if(input.type == "checkbox")
          {
            input.checked = input.entity.data[input.key];
          }
          else
          {
            input.value = input.entity.data[input.key];
          }
        }
      }
    }
    else
    {
      entityDiv = document.createElement("div");
      entityDiv.id = entity.id;
      entityDiv.className = "Entity";
      entityDiv.style.display = "none";
      entityDiv.style.border = "10px solid black";
      entityDiv.style.margin = "5px";

      entityDiv.appendChild(createToggleButton(entity));

      var t = document.createElement("input");
      t.type = "text";
      t.size = "50";
      t.entity = entity;
      t.placeholder = "Enter name of component to add to this entity";
      t.addEventListener("keyup", addComponent);
      entityDiv.appendChild(t);

      var en = document.createElement("h3");
      en.innerHTML = "Data";
      entityDiv.appendChild(en);

      var table = document.createElement("table");
      table.style.margin = "5px";
      table.style.display = "inherit";
      entityDiv.appendChild(table);

      var tr = document.createElement("tr");
      table.appendChild(tr);

      var th = document.createElement("th");
      th.innerHTML = "Key";
      tr.appendChild(th);

      th = document.createElement("th");
      th.innerHTML = "Value";
      tr.appendChild(th);

      for(var k in entity.data)
      {
        tr = document.createElement("tr");
        table.appendChild(tr);

        var data = entity.data[k];
        var type = typeof data;
        if(type != "string" && type != "number" && type != "boolean"){
          continue;
        }

        var th = document.createElement("td");
        tr.appendChild(th);

        var label = document.createElement("label");
        label.innerHTML = k;
        label.style.margin = 5;
        th.appendChild(label);

        th = document.createElement("td");
        tr.appendChild(th);

        var el = document.createElement("input");
        el.className = 'EntityDataValue';
        el.style.width = "100%";
        el.value = data;
        el.key = k;
        el.entity = entity;
        el.updateEntityData = updateEntityData;
        el.addEventListener("change", onEntityDataChange);
        th.appendChild(el);

        if(type == "number")
        {
          el.type = "number";
          el.step = "any";
        }
        else if(type == "string")
        {
          el.type = "text";
        }
        else if(type == "boolean")
        {
          el.type = "checkbox";
          el.checked = data;
        }
      }

      en = document.createElement("h3");
      en.innerHTML = "Components";
      entityDiv.appendChild(en);

      table = document.createElement("table");
      table.style.margin = "5px";
      table.style.display = "inherit";
      entityDiv.appendChild(table);

      tr = document.createElement("tr");
      table.appendChild(tr);

      th = document.createElement("th");
      th.innerHTML = "Name";
      tr.appendChild(th);

      th = document.createElement("th");
      th.innerHTML = "Reload";
      tr.appendChild(th);

      th = document.createElement("th");
      th.innerHTML = "Remove";
      tr.appendChild(th);

      for(var i in entity._components)
      {
        var tr = document.createElement("tr");
        table.appendChild(tr);

        th = document.createElement("td");
        tr.appendChild(th);
        th.appendChild(createToggleButton(entity._components[i]));

        th = document.createElement("td");
        tr.appendChild(th);
        var button = document.createElement("button");
        button.component = entity._components[i];
        button.textContent = "Remove";
        button.addEventListener("click", removeComponent);
        th.appendChild(button);

        th = document.createElement("td");
        tr.appendChild(th);
        button = document.createElement("button");
        button.component = entity._components[i];
        button.entity = entity;
        button.textContent = "Reload";
        button.addEventListener("click", reloadComponent);
        th.appendChild(button);
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

  function updateHTML(system)
  {
    var system = system || getSelectedSystem();

    Systems.forEachSystem(updateSystem);
    updateEntityDivVisibility();

    if(system)
    {
      var systems = document.getElementsByClassName("SystemRadioButton");
      for(var i in systems)
      {
        var div = systems[i];
        if(div.system && !div.checked && div.system.id == system.id)
        {
          div.checked = true;
          updateHTML(system);
          break;
        }
      }
    }
  }

  updateButton.addEventListener("click", updateHTML);

  eventDispatcher.addEventListener("keyup", function(evt)
  {
    evt.preventDefault();
    if(evt.keyCode == 13)
    {
      Systems.pushEvent(evt.srcElement.value);
    }
  });

  function autoUpdateHTML()
  {
    if(autoUpdateCheckbox.checked)
    {
      updateHTML();
    }
    setTimeout(autoUpdateHTML, autoUpdateInterval.value);
  }

  setTimeout(autoUpdateHTML, autoUpdateInterval.value);
});