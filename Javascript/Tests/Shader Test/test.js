require.config({
  baseUrl : '../../',
  paths :
  {
    loadTexture : "Tests/Shader Test/loadTexture",
    createSquare : "Tests/Shader Test/createSquare",
    shaders : "Tests/Shader Test/shaders",
    m4 : 'Tests/Shader Test/m4',
    'bindTexture' : "Tests/Shader Test/bindTexture",
    'createTexture' : "Tests/Shader Test/createTexture",
    DrawableComponent : "Default Components/Interfaces/DrawableComponent"
  }
});

require(['DrawableComponent', 'game', 'systemlist', 'system', 'loadTexture', 'createSquare', 'shaders', 'm4', 'bindTexture', 'createTexture'],
function(DrawableComponent, _, Systems, System, loadTexture, createSquare, shaders, mat4, bindTexture, createTexture)
{
  var canvas = document.createElement("canvas");
  var gl = canvas.getContext("webgl");

  if(gl == null)
  {
    alert("Unable to initialize WebGL. Your browser or machine may not support it.");
  }

  function loadShader(type, source)
  {
    var shader = gl.createShader(type);

    gl.shaderSource(shader, source);
    gl.compileShader(shader);
    if(!gl.getShaderParameter(shader, gl.COMPILE_STATUS))
    {
      console.log(gl.getShaderInfoLog(shader));
      return null;
    }
    return shader;
  }

  function createProgram(vertShader)
  {
    var vert = loadShader(gl.VERTEX_SHADER, shaders.vert);
    var frag = loadShader(gl.FRAGMENT_SHADER, vertShader);

    var program = gl.createProgram();
    gl.attachShader(program, vert);
    gl.attachShader(program, frag);
    gl.linkProgram(program);
    if(!gl.getProgramParameter(program, gl.LINK_STATUS))
  {
    console.log(gl.getProgramInfoLog(program));
  }

    program.info ={
      attribLocations: {
        vertexPosition: gl.getAttribLocation(program, 'aVertexPosition'),
        textureCoord: gl.getAttribLocation(program, 'aTextureCoord')
      },
      uniformLocations: {
        projectionMatrix: gl.getUniformLocation(program, 'uProjectionMatrix'),
        uSampler: gl.getUniformLocation(program, 'uSampler'),
        oldColor : gl.getUniformLocation(program, 'oldColor'),
        newColor : gl.getUniformLocation(program, 'newColor'),
        direction : gl.getUniformLocation(program, 'direction'),
        min_luma : gl.getUniformLocation(program, 'min_luma'),
        exposure : gl.getUniformLocation(program, 'exposure'),
        decay : gl.getUniformLocation(program, 'decay'),
        density : gl.getUniformLocation(program, 'density'),
        weight : gl.getUniformLocation(program, 'weight'),
        samples : gl.getUniformLocation(program, 'samples'),
        light_position : gl.getUniformLocation(program, 'light_position')
      },
    };

    return program;
  }

  var programs = {};
  for(var i in shaders)
  {
    if(i != "vert")
    {
      programs[i] = createProgram(shaders[i]);
    }
  }

  var buffer = createSquare(gl);
  var textureBuffer = createSquare(gl);

  function loadCanvas(canvas)
  {
    canvas.texture = createTexture(gl);
    canvas.gl = gl;
    bindTexture(canvas);
  }

  function createTestCanvas(vertShader)
  {
    var testCanvas = document.createElement("canvas");
    testCanvas.width = 64;
    testCanvas.height = 64;
    var testContext = testCanvas.getContext("2d");
    testContext.fillStyle = "white";
    testContext.fillRect(16, 16, 32, 32);
    // testContext.fillRect(0, 0, 64, 64);
    loadCanvas(testCanvas);
    testCanvas.texture.width = 64;
    testCanvas.texture.height = 64;
    testCanvas.texture.oldColor = [0,0,0,1];
    testCanvas.texture.newColor = [1,1,0,1];
    testCanvas.texture.program = programs[vertShader];
    return testCanvas;
  }

  class Test extends DrawableComponent
  {
    constructor(entity)
    {
      super(entity);
      this.setDefaults({
        width : 100,
        height : 100,
        min_luma : 0.2
      });

      // var texture = loadTexture(gl, "bombing blocks screenshot (6).png");
      this.texture = loadTexture(gl, "plunger.png");
      this.texture.oldColor = [1,0,1,1];
      this.texture.newColor = [0,1,1,1];
      this.texture.program = programs.frag;

      this.texture2 = createTexture(gl);
      this.texture2.oldColor = [0,0,1,1];
      this.texture2.newColor = [0,1,1,1];
      this.texture2.program = programs.defFs;

      this.data.textures = [this.texture, this.texture2];
      this.data.textures.push(createTestCanvas("threshold").texture)
      this.data.textures.push(createTestCanvas("blur").texture)
      this.data.textures.push(createTestCanvas("blur").texture)
      this.data.textures.push(createTestCanvas("frag").texture)
      this.data.textures.push(createTestCanvas("blur").texture)
      this.data.textures.push(createTestCanvas("blur").texture)
      this.data.textures[3].dirWidth = true;
      this.data.textures[4].dirhw = true;
      this.data.textures[6].dirHeight = true;
      this.data.textures[7].dirwh = true;
      

      this.data.textures.push(createTestCanvas("godsray").texture)
      function set(t)
      {
        t.exposure = 0.25;
        t.decay = 0.95;
        t.density = 0.15;
        t.weight = 0.5;
        t.light_position = [0.5, 0.5];
        t.samples = 70;
        t.ray = true;
      }
      set(this.data.textures[8]);

      this.data.positions = [
        {x : 100, y : 100},
        {x : 200, y : 200},
        {x : 400, y : 300},
        {x : 400, y : 300},
        {x : 400, y : 300},
        {x : 400, y : 300},
        {x : 400, y : 300},
        {x : 400, y : 300},
        {x : 600, y : 400}
      ]
    }

    eventUpdate(args)
    {
      for(var i in this.data.textures)
      {
        var data = this.data.textures[i];
        data.newColor[1] += args.dt * 0.001;
        data.newColor[1] %= 1;
      }
      this.data.min_luma += (args.dt * 0.001) 
      this.data.min_luma %= 1;

      var tex = this.data.textures[8];
      // tex.light_position[0] += args.dt * 0.001;
      tex.light_position[1] += args.dt * 0.001;
      // tex.light_position[0] %= 1;
      tex.light_position[1] %= 1;
    }

    eventDraw()
    {
      this.context.save();
      for(var i  = 0; i < this.data.textures.length; ++i)
      {
        var pos = this.data.positions[i];
        if(i == 2)
        {
          this.context.globalCompositeOperation = "lighter";
        }
        if(i == 8)
        {
          this.context.globalCompositeOperation = "source-over";
        }
        this.drawTexture(this.data.textures[i], pos.x, pos.y);
      }
      this.context.restore();
    }

    drawTexture(texture, x, y, c)
    {
      c = c || canvas;

      var width = texture.width;
      var height = texture.height;
      if(width > 0 && c.width !== width || height > 0 && c.height !== height)
      {
        c.width = width;
        c.height = height;
      }

      var data = this.data;

      gl.viewport(0, 0, c.width, c.height);

      gl.clearColor(0.0, 0.0, 0.0, 1.0);
      gl.clear(gl.COLOR_BUFFER_BIT);

      gl.bindTexture(gl.TEXTURE_2D, texture);

      var program = texture.program || programs.defFs;
      gl.useProgram(program);
      var programinfo = program.info;

      gl.bindBuffer(gl.ARRAY_BUFFER, buffer);
      gl.enableVertexAttribArray(programinfo.attribLocations.vertexPosition);
      gl.vertexAttribPointer(programinfo.attribLocations.vertexPosition, 2, gl.FLOAT, false, 0, 0);

      gl.bindBuffer(gl.ARRAY_BUFFER, textureBuffer);
      gl.enableVertexAttribArray(programinfo.attribLocations.textureCoord);
      gl.vertexAttribPointer(programinfo.attribLocations.textureCoord, 2, gl.FLOAT, false, 0, 0);

      var mat = mat4.orthographic(0, c.width, c.height, 0, -1, 1);
      mat = mat4.scale(mat, c.width, c.height, 1);

      gl.uniformMatrix4fv(programinfo.uniformLocations.projectionMatrix, false, mat);

      gl.uniform4fv(programinfo.uniformLocations.newColor, texture.newColor);
      gl.uniform4fv(programinfo.uniformLocations.oldColor, texture.oldColor);
      gl.uniform1f(programinfo.uniformLocations.min_luma, data.min_luma);
      if(texture.dirWidth)
      {
        gl.uniform2fv(programinfo.uniformLocations.direction, [1 / width, 0]);
      }
      else if(texture.dirHeight)
      {
        gl.uniform2fv(programinfo.uniformLocations.direction, [0, 1/ height]);
      }
      else if(texture.dirwh)
      {
        return;
        // gl.uniform2fv(programinfo.uniformLocations.direction, [1 / width, -1 / height]);
      }
      else if(texture.dirhw)
      {
        return;
        // gl.uniform2fv(programinfo.uniformLocations.direction, [1 / width, 1 / height]);
      }
      else if(texture.ray)
      {
        gl.uniform1f(programinfo.uniformLocations.exposure, texture.exposure);
        gl.uniform1f(programinfo.uniformLocations.decay, texture.decay);
        gl.uniform1f(programinfo.uniformLocations.density, texture.density);
        gl.uniform1f(programinfo.uniformLocations.weight, texture.weight);
        gl.uniform1f(programinfo.uniformLocations.samples, texture.samples);
        gl.uniform2fv(programinfo.uniformLocations.light_position, texture.light_position);
      }

      gl.uniform1i(programinfo.uniformLocations.uSampler, 0);

      gl.drawArrays(gl.TRIANGLES, 0, 6);

      this.context.drawImage(c, 0, 0, c.width, c.height,
          x, y, data.width, data.height);
    }
    
  }

  var system = Systems.addSystem(new System("Test"));
  var entity = system.createEntity();
  entity.addComponent(Test);
});
