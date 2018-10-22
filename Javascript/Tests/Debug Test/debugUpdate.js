require(['systemlist'], function(Systems)
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
    }
  }

  function removeComponent(evt)
  {
    var button = evt.srcElement;
    console.assert(button.component.remove(), "Failed to remove component");
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

        var th = document.createElement("th");
        tr.appendChild(th);

        var label = document.createElement("label");
        label.innerHTML = k;
        label.style.margin = 5;
        th.appendChild(label);

        th = document.createElement("th");
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

      ul = document.createElement("ul");
      ul.className = "Enttiy Components";
      entityDiv.appendChild(ul);
      for(var i in entity._components)
      {
        var li = document.createElement("li");
        ul.appendChild(li);

        var div = document.createElement("div");
        li.appendChild(div);

        div.appendChild(createToggleButton(entity._components[i]));

        var button = document.createElement("button");
        button.component = entity._components[i];
        button.textContent = "Remove";
        button.addEventListener("click", removeComponent);
        div.appendChild(button);
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
