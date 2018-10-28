define(['drawableComponent', 'systemlist', 'system', 'loadTexture', 'createSquare', 'shaders', 'm4'],
function(DrawableComponent, Systems, System, loadTexture, createSquare, shaders, mat4)
{
    var webCanvas = document.createElement("canvas");
    var gl = webCanvas.getContext("webgl");

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
    var frag = loadShader(gl.FRAGMENT_SHADER, shaders.defFs);

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
        uSampler: gl.getUniformLocation(program, 'uSampler')
        },
    };

    var buffer = createSquare(gl);
    var textureBuffer = createSquare(gl);

    class Test extends DrawableComponent
    {
        constructor(entity)
        {
            super(entity);
            this.setDefaults({
                width : 100,
                height : 100,
                xRotation : 0,
                yRotation : 0,
                zRotation : 0,
                xInc : Math.PI / 32,
                yInc : Math.PI / 32,
                zInc : Math.PI / 16,
        });

        // var texture = loadTexture(gl, "bombing blocks screenshot (6).png");
        this.texture = loadTexture(gl, "plunger.png");
        }

        eventUpdate(args)
        {
            this.data.xRotation += this.data.xInc;
            this.data.yRotation += this.data.yInc;
            this.data.zRotation += this.data.zInc;
        }

        eventDraw()
        {
            this.drawTexture(this.texture, 100, 100);
        }

        drawTexture(texture, x, y)
        {
            // var width = texture.width;
            // var height = texture.height;
            // if(width > 0 && webCanvas.width !== width || height > 0 && webCanvas.height !== height)
            // {
            //     webCanvas.width = width;
            //     webCanvas.height = height;
            // }

            var data = this.data;

            gl.viewport(0, 0, webCanvas.width, webCanvas.height);

            gl.clearColor(0.0, 0.0, 0.0, 0.0);
            gl.clear(gl.COLOR_BUFFER_BIT);

            gl.bindTexture(gl.TEXTURE_2D, texture);

            gl.useProgram(program);

            gl.bindBuffer(gl.ARRAY_BUFFER, buffer);
            gl.enableVertexAttribArray(programinfo.attribLocations.vertexPosition);
            gl.vertexAttribPointer(programinfo.attribLocations.vertexPosition, 2, gl.FLOAT, false, 0, 0);

            gl.bindBuffer(gl.ARRAY_BUFFER, textureBuffer);
            gl.enableVertexAttribArray(programinfo.attribLocations.textureCoord);
            gl.vertexAttribPointer(programinfo.attribLocations.textureCoord, 2, gl.FLOAT, false, 0, 0);

            var mat = mat4.orthographic(0, webCanvas.width, webCanvas.height, 0, -2, 2);
            mat = mat4.translate(mat, webCanvas.width * 0.5, webCanvas.height * 0.5, 0);
            mat = mat4.xRotate(mat, this.data.xRotation);
            mat = mat4.yRotate(mat, this.data.yRotation);
            mat = mat4.zRotate(mat, this.data.zRotation);
            mat = mat4.translate(mat, -webCanvas.width * 0.5, -webCanvas.height * 0.5, 0);
            mat = mat4.scale(mat, webCanvas.width, webCanvas.height, 2);
            
            gl.uniformMatrix4fv(programinfo.uniformLocations.projectionMatrix, false, mat);

            gl.uniform1i(programinfo.uniformLocations.uSampler, 0);

            gl.drawArrays(gl.TRIANGLES, 0, 6);

            this.context.drawImage(webCanvas, 0, 0, webCanvas.width, webCanvas.height,
                x, y, data.width, data.height);
        }
    }

    var system = Systems.addSystem(new System("Test"));
    var entity = system.createEntity();
    entity.addComponent(Test);
});
  