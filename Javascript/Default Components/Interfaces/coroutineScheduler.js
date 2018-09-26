class CoroutineScheduler
{
  constructor()
  {
    this.routines = [];
  }

  addRoutine(func)
  {
    this.routines.push(
    {
      current : 0,
      target : 0,
      func : func()
    });
  }

  finishRoutine(func)
  {
    for(var i = 0; i < this.routines.length; ++i)
    {
      var routine = this.routines[i];
      if(routine.func == func)
      {
        var result = false;
        while(!done)
        {
          result = func.next();
        }
        break;
      }
    }
  }

  cancelRoutine(func)
  {
    for(var i = 0; i < this.routines.length; ++i)
    {
      var routine = this.routines[i];
      if(routine.func == func)
      {
        this.routines.splice(i, 1);
        break;
      }
    }
  }

  hasRoutine(func)
  {
    for(var i = 0; i < this.routines.length; ++i)
    {
      var routine = this.routines[i];
      if(routine.func == func)
      {
        return true;
      }
    }
    return false;
  }

  clear()
  {
    this.routines = [];
  }

  update(step)
  {
    for(var i = 0; i < this.routines.length; ++i)
    {
      var routine = this.routines[i];
      if(typeof routine.target == "number")
      {
        routine.current += step;
        if(routine.current < routine.target)
        {
          continue;
        }
      }

      var result = routine.func.next();
      if(result.done)
      {
        this.routines.splice(i, 1);
      }
      else
      {
        routine.current = 0;
        routine.target = result.value;
        if(result.value == null)
        {
          routine.target = 0;
        }
        else
        {
          routine.target = result.value;
        }
      }
    }
  }
}
