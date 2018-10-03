define(function()
{
  return function(gl)
  {
    var buffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, buffer);
    var positions = [0,0,1,0,0,1, 0,1,1,1,1,0];
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(positions), gl.STATIC_DRAW);
    return buffer;
  };
});
