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

require(['m4', 'shaders', 'DrawableComponent', 'systemlist', 'system'], 
function(mat4, shaders, Component, Systems)
{
  var canvas = document.createElement("canvas");
  var gl = canvas.getContext("webgl");

  if(gl == null)
  {
    alert(`Unable to initialize WebGL. Your browser or machine may not support it.
      Shader support is not available and will affect the game.`);
  }

  function loadShader(type, source)
  {
    var shader = gl.createShader(type);

    gl.shaderSource(shader, source);
    gl.compileShader(shader);
    if(!gl.getShaderParameter(shader, gl.COMPILE_STATUS))
    {
      console.log(gl.getShaderInfoLog(shader), source);
      return null;
    }
    return shader;
  }

    var vert = loadShader(gl.VERTEX_SHADER, shaders.vert);
    var frag = loadShader(gl.FRAGMENT_SHADER, shaders.defFs);

    var program = gl.createProgram();
    gl.attachShader(program, vert);
    gl.attachShader(program, frag);
    gl.linkProgram(program);
    if(!gl.getProgramParameter(program, gl.LINK_STATUS))
    {
      console.log(gl.getProgramInfoLog(program));
    }

    var programinfo = {
        program : program,
        attribLocations: {
        vertexPosition: gl.getAttribLocation(program, 'aVertexPosition'),
        textureCoord: gl.getAttribLocation(program, 'aTextureCoord')
        },
        uniformLocations: {
        projectionMatrix: gl.getUniformLocation(program, 'uProjectionMatrix'),
        uSampler: gl.getUniformLocation(program, 'uSampler'),
        uTint: gl.getUniformLocation(program, 'uTint')
        },
    };

  function makeBuffer()
  {
    var buffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, buffer);
    var positions = [0,0,1,0,0,1,  0,1,1,1,1,0];
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(positions), gl.STATIC_DRAW);
    return buffer;
  }

  function isPowerOf2(value)
  {
    return (value & (value - 1)) == 0;
  }

  var positionBuffer = makeBuffer();
  var textureBuffer = makeBuffer();

  var textures = {};

    function bind(name, image)
    {
        if(typeof name == "object")
        {
            throw "Key cannot be an object";
        }

        var texture = gl.createTexture();
        gl.bindTexture(gl.TEXTURE_2D, texture);
        gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, image);
        if(isPowerOf2(image.width) && isPowerOf2(image.height))
        {
            gl.generateMipmap(gl.TEXTURE_2D);
        }
        else
        {
            gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
            gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
            gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
        }
        texture.width = image.width;
        texture.height = image.height;
        textures[name] = texture;
        return texture;
    }

    function draw(textureName, shaderFunc)
    {
      var texture = textures[textureName];
      if(texture == null){return;} //image hasn't been loaded yet

        var width = texture.width;
        var height = texture.height;
        if(width > 0 && canvas.width !== width || height > 0 && canvas.height !== height)
        {
            canvas.width = width;
            canvas.height = height;
        }

        gl.viewport(0, 0, canvas.width, canvas.height);
      
        gl.clearColor(0.0, 0.0, 0.0, 0.0);
        gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

      gl.bindTexture(gl.TEXTURE_2D, texture);

      gl.useProgram(program);

      gl.enable(gl.DEPTH_TEST);

      gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
      gl.enableVertexAttribArray(programinfo.attribLocations.vertexPosition);
      gl.vertexAttribPointer(programinfo.attribLocations.vertexPosition, 2, gl.FLOAT, false, 0, 0);

      gl.bindBuffer(gl.ARRAY_BUFFER, textureBuffer);
      gl.enableVertexAttribArray(programinfo.attribLocations.textureCoord);
      gl.vertexAttribPointer(programinfo.attribLocations.textureCoord, 2, gl.FLOAT, false, 0, 0);

      var mat = mat4.orthographic(0, canvas.width, canvas.height, 0, -1, 1);
      mat = mat4.scale(mat, canvas.width, canvas.height, 1);

      gl.uniformMatrix4fv(programinfo.uniformLocations.projectionMatrix, false, mat);

      gl.uniform1i(programinfo.uniformLocations.uSampler, 0);

      shaderFunc(gl, programinfo);

      gl.drawArrays(gl.TRIANGLES, 0, 6);
    }

    function shaderFunc(gl, info)
    {
        var mat;
        if(this.data.is3d)
        {
            mat = mat4.perspective(Math.PI / 3, canvas.width / canvas.height, 1, 2000);
            mat = mat4.translate(mat, -canvas.width * 0.5, -canvas.height * 0.5, this.data.z);
        }
        else
        {
            mat = mat4.orthographic(0, canvas.width, canvas.height, 0, -1, 1);
        }
        
        mat = mat4.scale(mat, canvas.width, canvas.height, 1);
        gl.uniformMatrix4fv(info.uniformLocations.projectionMatrix, false, mat);

        gl.uniform4fv(info.uniformLocations.uTint, this.data.color);
    }

    class TestComponent extends Component
    {
        constructor(entity)
        {
            super(entity);

            this.setDefaults({
                x : 100,
                y : 100,
                z : 0,
                is3d : false,
                color : [1,0,0,1]
            })

            var canvas = document.createElement('canvas');
            var context = canvas.getContext('2d');

            canvas.width = 100;
            canvas.height = 100;
            context.fillStyle = 'white';
            context.fillRect(0, 0, 100, 100);

            bind("test", canvas);

            this.shaderFunc = shaderFunc.bind(this);
            this.time = 0;
        }

        eventUpdate(args)
        {
            this.time += args.dt * 0.001;
            this.time %= 1;
            this.data.z = -1000 + 999 * this.time;
        }

        eventDraw()
        {
            draw('test', this.shaderFunc);
            this.context.drawImage(canvas, this.data.y, this.data.y);
        }
    }

    var system = Systems.addSystem("Shader Loader");
    
    var entity = system.createEntity();
    entity.data.is3d = true;
    entity.addComponent(TestComponent);
    entity.data.x = 0;
    entity.data.y = 0;

    var entity2 = system.createEntity();
    entity2.data.x = 0;
    entity2.data.y = 0;
    entity2.data.color = [0,0,1,0.5];
    entity2.addComponent(TestComponent);
});
