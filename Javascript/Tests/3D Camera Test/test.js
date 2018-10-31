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
      'createSphere' : "Tests/3D Camera Test/createSphere",
      'createCube' : "Tests/3D Camera Test/createCube",
      DrawableComponent : "Default Components/Interfaces/DrawableComponent"
    }
  });

require(['m4', 'shaders', 'DrawableComponent', 'systemlist', 'system', 'createSphere', 'loadTexture', 'createCube'], 
function(mat4, shaders, Component, Systems, _, createSphere, loadTexture, createCube)
{
    document.body.style.display = "inline-flex";
  var canvas = document.createElement("canvas");
  document.body.appendChild(canvas);
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

    function makeSquare(offset)
    {
        var positions = [
            0,0,0,
            1,0,0,
            0,1,0,

            0,1,0,
            1,1,0,
            1,0,0];
        for(var i = 0; i < positions.length; ++i)
        {
            positions[i] -= 0.5;
            positions[i] += offset[i % 3];
        }
        return positions
    }

    var components = 3;
    var vertices = 6;
  function makeBuffer()
  {
    var buffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, buffer);

    var positions = makeSquare([0,0,0]);
    positions = positions.concat(makeSquare([0,1,0]))
    
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

    function draw(textureName, shaderFunc, dontClear)
    {
        if(!dontClear)
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
            gl.clearDepth(1.0);
            gl.depthFunc(gl.LEQUAL);
            gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

        gl.bindTexture(gl.TEXTURE_2D, texture);

        gl.enable(gl.DEPTH_TEST);

        gl.useProgram(program);

        gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
        gl.enableVertexAttribArray(programinfo.attribLocations.vertexPosition);
        gl.vertexAttribPointer(programinfo.attribLocations.vertexPosition, components, gl.FLOAT, false, 0, 0);

        gl.bindBuffer(gl.ARRAY_BUFFER, textureBuffer);
        gl.enableVertexAttribArray(programinfo.attribLocations.textureCoord);
        gl.vertexAttribPointer(programinfo.attribLocations.textureCoord, components, gl.FLOAT, false, 0, 0);

        var mat = mat4.orthographic(0, canvas.width, canvas.height, 0, -1, 1);
        mat = mat4.scale(mat, canvas.width, canvas.height, 1);

        gl.uniformMatrix4fv(programinfo.uniformLocations.projectionMatrix, false, mat);

        gl.uniform1i(programinfo.uniformLocations.uSampler, 0);

        }

      if(!shaderFunc(gl, programinfo))
      {
        gl.drawArrays(gl.TRIANGLES, 0, vertices);
      }
    }

    var target = [0,0,0];
    var up = [0,1,0];
    function shaderFunc(gl, info)
    {
        var mat;
        if(this.data.is3d)
        {
            var camera = mat4.lookAt(this.data.cameraPosition, target, up);
            var view = mat4.inverse(camera);

            var perspective = mat4.perspective(Math.PI / 6, canvas.width / canvas.height, -1000, 1000);
            mat = mat4.multiply(perspective, view);
        }
        else
        {
            mat = mat4.orthographic(0, canvas.width, canvas.height, 0, -1, 1);
        }
        
        mat = mat4.scale(mat, canvas.width/2, canvas.height/2, 1);
        gl.uniformMatrix4fv(info.uniformLocations.projectionMatrix, false, mat);

        gl.uniform4fv(info.uniformLocations.uTint, this.data.color1);
    }

    function shaderFunc2(gl, info)
    {
        var mat;
        if(this.data.is3d)
        {
            var camera = mat4.lookAt(this.data.cameraPosition, target, up);
            var view = mat4.inverse(camera);

            var perspective = mat4.perspective(Math.PI / 6, canvas.width / canvas.height, -1000, 1000);
            mat = mat4.multiply(perspective, view);
            // mat = mat4.scale(mat, 2, 2, 1);
            mat = mat4.rotation(mat, this.data.rotation);
        }
        else
        {
            mat = mat4.orthographic(0, canvas.width, canvas.height, 0, -1, 1);
        }
        
        mat = mat4.scale(mat, canvas.width, canvas.height, 1);
        gl.uniformMatrix4fv(info.uniformLocations.projectionMatrix, false, mat);

        gl.uniform4fv(info.uniformLocations.uTint, this.data.color2);
    }

    var c = document.createElement('canvas');
    var context = c.getContext('2d');

    c.width = 800;
    c.height = 600;

    context.fillStyle = 'white';
    context.fillRect(0, 0, 800, 600);
    context.strokeStyle = 'black';
    for(var w = 0; w < 800; w += 80)
    {
        for(var h = 0; h < 600; h += 60)
        {
            context.strokeRect(w, h, 80, 60);
        }
    }

    bind("test",c);

    textures.ufo = loadTexture(gl, "ufo.png");
            
    class TestComponent extends Component
    {
        constructor(entity)
        {
            super(entity);

            this.setDefaults({
                rotation : [0,0,0],
                speed : 1000,
                rSpeed : Math.PI /16,
                rIndex : 0,
                cameraPosition : [0,0,1000],
                is3d : true,
                color1 : [1,0,0,1],
                color2 : [0,0,1,1]
            })

            

            this.shaderFunc = shaderFunc.bind(this);
            this.shaderFunc2 = shaderFunc2.bind(this);
            this.time = 0;
        }

        eventKeyDown(args)
        {
            var key = args.key;
            var speed = this.data.speed;
            if(key == "a")
            {
                this.data.cameraPosition[0] -= speed;
            }
            else if(key == "d")
            {
                this.data.cameraPosition[0] += speed;
            }
            else if(key == "w")
            {
                this.data.cameraPosition[1] += speed;
            }
            else if(key == "s")
            {
                this.data.cameraPosition[1] -= speed;
            }
            else if(key == "ArrowUp")
            {
                this.data.cameraPosition[2] -= speed;
            }
            else if(key == "ArrowDown")
            {
                this.data.cameraPosition[2] += speed;
            }
            else if(key == "q")
            {
                this.data.rotation[this.data.rIndex] -= this.data.rSpeed;
            }
            else if(key == "e")
            {
                this.data.rotation[this.data.rIndex] += this.data.rSpeed;
            }
            else if(key == "r")
            {
                ++this.data.rIndex;
                this.data.rIndex %= 3;
            }
            else if(key == "Enter")
            {
                console.log(this.data.cameraPosition);
            }
        }

        eventDraw()
        {
            draw('test', this.shaderFunc2);
            draw('test', this.shaderFunc, true);
            this.context.drawImage(canvas, 0,0);
        }
    }

    var system = Systems.addSystem("Shader Loader");
    var entity = system.createEntity();

    function sphereShader(gl, programinfo)
    {
        var camera = mat4.lookAt(this.data.cameraPosition, target, up);
        var view = mat4.inverse(camera);

        var perspective = mat4.perspective(Math.PI / 6, canvas.width / canvas.height, 1, 1000000);
        var mat = mat4.multiply(perspective, view);
        mat = mat4.rotation(mat, this.data.rotation);
        mat = mat4.scale(mat, canvas.width, canvas.height, canvas.width);
        gl.uniformMatrix4fv(programinfo.uniformLocations.projectionMatrix, false, mat);

        gl.bindBuffer(gl.ARRAY_BUFFER, this.positionBuffer);
        gl.enableVertexAttribArray(programinfo.attribLocations.vertexPosition);
        gl.vertexAttribPointer(programinfo.attribLocations.vertexPosition, 3, gl.FLOAT, false, 0, 0);

        gl.bindBuffer(gl.ARRAY_BUFFER, this.textureBuffer);
        gl.enableVertexAttribArray(programinfo.attribLocations.textureCoord);
        gl.vertexAttribPointer(programinfo.attribLocations.textureCoord, 2, gl.FLOAT, false, 0, 0);

        gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, this.indexBuffer);

        gl.uniform4fv(programinfo.uniformLocations.uTint, this.data.color);

        gl.drawElements(gl.TRIANGLES, this.vertexCount, gl.UNSIGNED_SHORT, 0);
        // gl.drawArrays(gl.POINTS, 0, this.vertexCount);

        return true;
    }

    class SphereComponent extends TestComponent
    {
        constructor(entity)
        {
            super(entity);

            this.shaderFunc = sphereShader.bind(this);
            var sphere = createSphere(10, 4);
            console.log(sphere);

            this.positionBuffer = gl.createBuffer();
            gl.bindBuffer(gl.ARRAY_BUFFER, this.positionBuffer);
            gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(sphere.verts), gl.STATIC_DRAW);
            this.vertexCount = sphere.vertices;

            this.textureBuffer = gl.createBuffer();
            gl.bindBuffer(gl.ARRAY_BUFFER, this.textureBuffer);
            gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(sphere.texcoords), gl.STATIC_DRAW);

            this.indexBuffer = gl.createBuffer();
            gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, this.indexBuffer);
            gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(sphere.indices), gl.STATIC_DRAW);

            this.set("color", [0,1,0,1]);
        }

        eventDraw()
        {
            draw('test', this.shaderFunc);
            this.context.drawImage(canvas, 0, 0);
        }
    }

    function cubeShader(gl, programinfo)
    {
        var camera = mat4.lookAt(this.data.cameraPosition, target, up);
        var view = mat4.inverse(camera);

        var perspective = mat4.perspective(Math.PI / 6, canvas.width / canvas.height, 1, 1000000);
        var mat = mat4.multiply(perspective, view);
        mat = mat4.rotation(mat, this.data.rotation);
        mat = mat4.scale(mat, canvas.width, canvas.height, canvas.width);
        gl.uniformMatrix4fv(programinfo.uniformLocations.projectionMatrix, false, mat);

        gl.bindBuffer(gl.ARRAY_BUFFER, this.buffer.position);
        gl.enableVertexAttribArray(programinfo.attribLocations.vertexPosition);
        gl.vertexAttribPointer(programinfo.attribLocations.vertexPosition, 3, gl.FLOAT, false, 0, 0);

        gl.bindBuffer(gl.ARRAY_BUFFER, this.buffer.color);
        gl.enableVertexAttribArray(programinfo.attribLocations.textureCoord);
        gl.vertexAttribPointer(programinfo.attribLocations.textureCoord, 4, gl.FLOAT, false, 0, 0);

        gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, this.buffer.indices);

        gl.uniform4fv(programinfo.uniformLocations.uTint, this.data.color);

        gl.drawElements(gl.POINTS, 36, gl.UNSIGNED_SHORT, 0);

        return true;
    }

    class CubeComponent extends TestComponent
    {
        constructor(entity)
        {
            super(entity);

            this.shaderFunc = cubeShader.bind(this);
            this.buffer = createCube(gl);

            this.set("color", [0,1,0,1]);
        }

        eventDraw()
        {
            draw('ufo', this.shaderFunc);
            this.context.drawImage(canvas, 0, 0);
        }
    }

    entity.addComponents([SphereComponent]);
});
