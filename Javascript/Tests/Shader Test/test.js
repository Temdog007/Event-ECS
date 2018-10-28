require.config({
  baseUrl : '../../',
  paths :
  {
    loadTexture : "Tests/Shader Test/loadTexture",
    createSquare : "Tests/Shader Test/createSquare",
    shaders : "Tests/Shader Test/shaders",
    m4 : 'Tests/Shader Test/m4',
    'bindTexture' : "Tests/Shader Test/bindTexture",
    DrawableComponent : "Default Components/Interfaces/DrawableComponent"
  }
});

require(['DrawableComponent', 'game', 'systemlist', 'system', 'loadTexture', 'createSquare', 'shaders', 'm4', 'bindTexture'],
function(DrawableComponent, Game, Systems, System, loadTexture, createSquare, shaders, mat4, bindTexture)
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

  var vert = loadShader(gl.VERTEX_SHADER, shaders.vert);
  var frag = loadShader(gl.FRAGMENT_SHADER, shaders.frag);

  var program = gl.createProgram();
  gl.attachShader(program, vert);
  gl.attachShader(program, frag);
  gl.linkProgram(program);

  if(!gl.getProgramParameter(program, gl.LINK_STATUS))
  {
    console.log(gl.getProgramInfoLog(program));
  }

  var programinfo ={
    attribLocations: {
      vertexPosition: gl.getAttribLocation(program, 'aVertexPosition'),
      textureCoord: gl.getAttribLocation(program, 'aTextureCoord')
    },
    uniformLocations: {
      projectionMatrix: gl.getUniformLocation(program, 'uProjectionMatrix'),
      uSampler: gl.getUniformLocation(program, 'uSampler'),
      oldColor : gl.getUniformLocation(program, 'oldColor'),
      newColor : gl.getUniformLocation(program, 'newColor')
    },
  };

  var buffer = createSquare(gl);
  var textureBuffer = createSquare(gl);

  function loadCanvas(canvas)
  {
    const texture = gl.createTexture();
    texture.width = canvas.width;
    texture.height = canvas.height;
    gl.bindTexture(gl.TEXTURE_2D, texture);

    // Because images have to be download over the internet
    // they might take a moment until they are ready.
    // Until then put a single pixel in the texture so we can
    // use it immediately. When the image has finished downloading
    // we'll update the texture with the contents of the image.
    gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA,
                  1, 1, 0, gl.RGBA, gl.UNSIGNED_BYTE,
                  new Uint8Array([0, 0, 255, 255]));
    
    canvas.texture = texture;
    canvas.gl = gl;
    bindTexture(canvas);
  }

  // Shaders don't work with canvases
  var testCanvas = document.createElement("canvas");
  var testContext = testCanvas.getContext("2d");
  testContext.fillRect(0, 0, 300, 100);
  loadCanvas(testCanvas);

  class Test extends DrawableComponent
  {
    constructor(entity)
    {
      super(entity);
      this.setDefaults({
        width : 100,
        height : 100,
        oldColor : [1,0,1,1],
        newColor : [0,1,0,1]
      });

      // var texture = loadTexture(gl, "bombing blocks screenshot (6).png");
      this.texture = loadTexture(gl, "plunger.png");
    }

    eventUpdate(args)
    {
      this.data.newColor[1] += args.dt * 0.001;
      this.data.newColor[1] %= 1;
    }

    eventDraw()
    {
      this.drawTexture(this.texture, 100, 100);
      this.drawTexture(testCanvas.texture, 300, 300);
    }

    drawTexture(texture, x, y)
    {
      var width = texture.width;
      var height = texture.height;
      if(width > 0 && canvas.width !== width || height > 0 && canvas.height !== height)
      {
        canvas.width = width;
        canvas.height = height;
      }

      var data = this.data;

      gl.viewport(0, 0, canvas.width, canvas.height);

      gl.clearColor(0.0, 0.0, 0.0, 1.0);
      gl.clear(gl.COLOR_BUFFER_BIT);

      gl.bindTexture(gl.TEXTURE_2D, texture);

      gl.useProgram(program);

      gl.bindBuffer(gl.ARRAY_BUFFER, buffer);
      gl.enableVertexAttribArray(programinfo.attribLocations.vertexPosition);
      gl.vertexAttribPointer(programinfo.attribLocations.vertexPosition, 2, gl.FLOAT, false, 0, 0);

      gl.bindBuffer(gl.ARRAY_BUFFER, textureBuffer);
      gl.enableVertexAttribArray(programinfo.attribLocations.textureCoord);
      gl.vertexAttribPointer(programinfo.attribLocations.textureCoord, 2, gl.FLOAT, false, 0, 0);

      var mat = mat4.orthographic(0, canvas.width, canvas.height, 0, -1, 1);
      mat = mat4.scale(mat, canvas.width, canvas.height, 1);

      gl.uniformMatrix4fv(programinfo.uniformLocations.projectionMatrix, false, mat);

      gl.uniform4fv(programinfo.uniformLocations.newColor, data.newColor);
      gl.uniform4fv(programinfo.uniformLocations.oldColor, data.oldColor);

      gl.uniform1i(programinfo.uniformLocations.uSampler, 0);

      gl.drawArrays(gl.TRIANGLES, 0, 6);

      this.context.drawImage(canvas, 0, 0, canvas.width, canvas.height,
        x, y, data.width, data.height);
    }
  }

  var system = Systems.addSystem(new System("Test"));
  var entity = system.createEntity();
  var component = entity.addComponent(Test);
});
