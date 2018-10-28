define(['bindTexture', 'createTexture'], function(bindTexture, createTexture)
{
  return function(gl, url)
  {
    var image = new Image();
    image.gl = gl;
    image.texture = createTexture(gl);
    image.onload = bindTexture;
    image.src = url;
    return image.texture;
  }
});
