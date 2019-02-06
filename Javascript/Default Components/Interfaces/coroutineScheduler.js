define(function()
{
  class CoroutineScheduler
  {
    constructor()
    {
      this.routines = [];
    }

    addRoutine(func, args)
    {
      this.routines.push(
      {
        current : 0,
        target : 0,
        func : func(args),
        args : args
      });
    }

    finishRoutine(func)
    {
      for(var i = 0; i < this.routines.length; ++i)
      {
        var routine = this.routines[i];
        if(routine.func == func)
        {
          var result;
          do
          {
            result = func.next();
          }while(!result.done);
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
        else if(typeof routine.target == "object")
        {
          var rval = routine.target;
          if(rval.type == "frames")
          {
            ++routine.current;
            if(routine.current < rval.value)
            {
              continue;
            }
          }
          else if(rval.type == "time")
          {
            routine.current += step;
            if(routine.current < rval.value)
            {
              continue;
            }
          }
        }

        var result = routine.func.next(routine.args);
        if(result.done)
        {
          this.routines.splice(i--, 1);
        }
        else
        {
          routine.current = 0;
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

  return CoroutineScheduler;
});
