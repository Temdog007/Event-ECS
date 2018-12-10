require(['systemlist'], function(Systems)
{
  function clearDiv(div)
  {
    if(!div){return;}
    while(div.hasChildNodes())
    {
      div.removeChild(div.firstChild);
    }
  }

  function removeDeadSystems(elements)
  {
    for(var el of elements)
    {
      if(!Systems.hasSystem(el.system))
      {
        el.parentNode.removeChild(el);
      }
    }
  }

  function removeDeadEntities(elements)
  {
    for(var el of elements)
    {
      if(!el.entity.system.hasEntity(el.entity))
      {
        el.parentNode.removeChild(el);
      }
    }
  }

  function clear()
  {
    removeDeadSystems(document.getElementsByClassName("SystemOption"));
    removeDeadEntities(document.getElementsByClassName("EntityOption"));
    clearDiv(document.getElementById("entityContent"));
  }

  function getSelectedSystem()
  {
    var options = document.getElementsByClassName("SystemOption");
    for(var i in options)
    {
      var option = options[i];
      if(option.selected)
      {
        return option.system;
      }
    }
  }

  function getSelectedEntity()
  {
    var options = document.getElementsByClassName("EntityOption");
    for(var i in options)
    {
      var option = options[i];
      if(option.selected)
      {
        return option.entity;
      }
    }
  }

  function updateSystemForeground()
  {
    if(!this.ecsObject){return;}
    this.style.backgroundColor = this.ecsObject.enabled ? "silver" : "dimGray";
  }

  function toggleEnabled(evt)
  {
    var button = evt.srcElement;
    if(!button.ecsObject){return;}
    button.ecsObject.enabled = !button.ecsObject.enabled;
    button.updateForeground();
  }

  function updateEntityData()
  {
    if(this.type == "checkbox")
    {
      this.dataTable[this.key] = this.checked;
    }
    else if(this.type == "number")
    {
      this.dataTable[this.key] = parseFloat(this.value);
    }
    else
    {
      this.dataTable[this.key] = this.value;
    }
  }

  function removeEntity(evt)
  {
    var button = evt.srcElement;
    console.assert(button.entity.remove(), "Failed to remove entity");
    clear();
    updateHTML();
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
    if(ecsObject)
    {
      toggleButton.textContent = ecsObject.name + " (ID " + ecsObject.id + ")";
    }
    toggleButton.addEventListener("click", toggleEnabled);
    toggleButton.updateForeground();
    return toggleButton;
  }

  var system_enabler = createToggleButton();
  system_enabler.textContent = "System Enabled";
  system_enabler_div.appendChild(system_enabler);

  function addComponent(evt)
  {
    if(evt.keyCode == 13)
    {
      var text = evt.srcElement;
      text.entity.addComponent(text.value);
      updateHTML();
    }
  }

  function removeComponent(evt)
  {
    var button = evt.srcElement;
    console.assert(button.component.remove(), "Failed to remove component");
    clear();
    updateHTML();
  }

  function reloadComponent(evt)
  {
    var button = evt.srcElement;
    var name = button.component.name;
    console.assert(button.component.remove(), "Failed to remove component");
    name = name.charAt(0).toLowerCase() + name.substring(1);
    require.undef(name);
    require([name], function(Component)
    {
      button.component = button.entity.addComponent(Component);
      console.log("Reloaded: " + Component.name);
      clear();
      updateHTML();
    });
  }

  function updateEntityDivVisibility()
  {
    var entity = getSelectedEntity();

    var divs = document.getElementsByClassName("Entity");
    for(var i in divs)
    {
      var div = divs[i];
      if(div.style)
      {
        div.style.display = div.entity == entity ? "block" : "none";
      }
    }
  }

  function createTable(entity, dataTable, level)
  {
    level = level || 0;
    if(level > 1){return;}

    var table = document.createElement("table");
    table.style.margin = "5px";
    table.style.display = "inherit";

    var tr = document.createElement("tr");
    table.appendChild(tr);

    var th = document.createElement("th");
    th.innerHTML = "Key";
    tr.appendChild(th);

    th = document.createElement("th");
    th.innerHTML = "Value";
    tr.appendChild(th);

    for(var k in dataTable)
    {
      tr = document.createElement("tr");
      table.appendChild(tr);

      var data = dataTable[k];
      var type = typeof data;

      var th = document.createElement("td");
      tr.appendChild(th);

      var label = document.createElement("label");
      label.innerHTML = k;
      label.style.margin = 5;
      th.appendChild(label);

      th = document.createElement("td");
      tr.appendChild(th);

      if(type == "number" || type == "string" || type == "boolean")
      {
        var el = document.createElement("input");
        el.className = 'EntityDataValue';
        el.style.width = "100%";
        el.value = data;
        el.key = k;
        el.dataTable = dataTable;
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
      else
      {
        var t = createTable(entity, data, level + 1);
        if(t)
        {
          th.appendChild(t);
        }
        else
        {
          table.removeChild(tr);
        }
      }
    }
    return table;
  }

  function updateComponent(parent, component)
  {
    var tr = document.getElementById(component.id);
    if(!tr)
    {
      tr = document.createElement("tr");
      tr.id = component.id;
      parent.appendChild(tr);

      th = document.createElement("td");
      tr.appendChild(th);
      th.appendChild(createToggleButton(component));

      th = document.createElement("td");
      tr.appendChild(th);
      var button = document.createElement("button");
      button.component = component;
      button.textContent = "Remove";
      button.addEventListener("click", removeComponent);
      th.appendChild(button);

      th = document.createElement("td");
      tr.appendChild(th);
      button = document.createElement("button");
      button.component = component;
      button.entity = parent.entity;
      button.textContent = "Reload";
      button.addEventListener("click", reloadComponent);
      th.appendChild(button);
    }
  }

  function updateEntity(entity, onlyVisible)
  {
    var entityDiv = document.getElementById(entity.id+"div");
    if(entityDiv && (!onlyVisible || entityDiv.style.display != "none"))
    {
      var inputs = entityDiv.getElementsByClassName("EntityDataValue");
      for(var i in inputs)
      {
        var input = inputs[i];
        if(input.key && input.dataTable)
        {
          input.value = input.dataTable[input.key];
          if(input.type == "checkbox")
          {
            input.checked = input.dataTable[input.key];
          }
        }
      }
    }
    else
    {
      entityDiv = document.createElement("div");
      entityDiv.entity = entity;
      entityDiv.id = entity.id+"div";
      entityDiv.className = "Entity";
      entityDiv.style.display = "none";
      entityDiv.style.border = "10px solid black";
      entityDiv.style.margin = "5px";

      entityDiv.appendChild(createToggleButton(entity));

      var k = document.createElement("button");
      k.textContent = "Remove";
      k.style.background = "red";
      k.style.color = "white";
      k.entity = entity;
      k.addEventListener("click", removeEntity);
      entityDiv.appendChild(k);

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

      try{entityDiv.appendChild(createTable(entity, entity.data));}
      catch(e){console.log(e, entity);}

      en = document.createElement("h3");
      en.innerHTML = "Components";
      entityDiv.appendChild(en);

      table = document.createElement("table");
      table.entity = entity;
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

      entityDiv.table = table;

      entityContent.appendChild(entityDiv);
    }

    for(var i in entity._components)
    {
      updateComponent(entityDiv.table, entity._components[i]);
    }

    return entityDiv;
  }

  function updateSystem(system, onlyVisible)
  {
    var systemOption = document.getElementById(system.id);
    if(!systemOption)
    {
      systemOption = document.createElement("option");
      systemOption.id = system.id;
      systemOption.textContent = system.name;
      systemOption.system = system;
      systemOption.className = "SystemOption";

      system_select.appendChild(systemOption);
    }

    var selectedSystem = getSelectedSystem();
    var selected = selectedSystem == system;
    if(selected)
    {
      system_enabler.ecsObject = system;
    }

    var selectedEntity = getSelectedEntity();
    for(var i in system._entities)
    {
      var entity = system._entities[i];
      var entityOption = document.getElementById(entity.id);
      if(!entityOption)
      {
        entityOption = document.createElement("option");
        entityOption.id = entity.id;
        entityOption.textContent = entity.name;
        entityOption.system = system;
        entityOption.entity = entity;
        entityOption.className = "EntityOption";

        entity_select.appendChild(entityOption);
      }
      if(selected)
      {
        entityOption.removeAttribute("disabled");
        if(selectedEntity === undefined)
        {
          var options = document.getElementsByClassName("EntityOption");
          for(var option of options)
          {
            if(option.entity == entity)
            {
              option.selected = true;
              selectedEntity = entity;
              break;
            }
          }
        }
      }
      else
      {
        entityOption.setAttribute("disabled", 0);
        if(entity == selectedEntity)
        {
          var options = document.getElementsByClassName("EntityOption");
          for(var option of options)
          {
            if(option.entity == entity)
            {
              option.selected = false;
              break;
            }
          }
        }
      }
      updateEntity(entity, onlyVisible == true);
    }
  }

  function updateHTML(onlyVisible)
  {
    if(typeof onlyVisible != "boolean")
    {
      onlyVisible = false;
    }
    Systems.forEachSystem(updateSystem, onlyVisible);
    updateEntityDivVisibility();
  }

  function completeUpdate()
  {
    clear()
    updateHTML();
  }

  updateButton.addEventListener("click", completeUpdate);

  system_select.addEventListener("change", updateHTML);

  entity_select.addEventListener("change", updateHTML);

  eventDispatcher.addEventListener("keyup", function(evt)
  {
    evt.preventDefault();
    if(evt.keyCode == 13)
    {
      Systems.pushEvent(evt.srcElement.value);
    }
  });

  entityAdder.addEventListener("keyup", function(evt)
  {
    evt.preventDefault();
    if(evt.keyCode == 13)
    {
      var system = getSelectedSystem();
      if(system)
      {
        system.createEntity(evt.srcElement.value);
        clear();
        updateHTML();
      }
    }
  });

  function autoUpdateHTML()
  {
    if(autoUpdateCheckbox.checked && debugMenu.style.display != 'none')
    {
      updateHTML(true);
    }
    setTimeout(autoUpdateHTML, autoUpdateInterval.value);
  }

  setTimeout(autoUpdateHTML, 10000);

  sideButton.addEventListener('click', function()
  {
    if(debugMenu.style.left == 0)
    {
      debugMenu.style.left = 0;
      debugMenu.style.right = null;
    }
    else
    {
      debugMenu.style.left = null;
      debugMenu.style.right = 0;
    }
  })
});
