define(function()
{
  function isPowerOf2(value)
  {
    return (value & (value-1)) == 0;
  }

  return function(gl, url)
  {
    const texture = gl.createTexture();
    texture.width = 1;
    texture.height = 1;
    gl.bindTexture(gl.TEXTURE_2D, texture);

    // Because images have to be download over the internet
    // they might take a moment until they are ready.
    // Until then put a single pixel in the texture so we can
    // use it immediately. When the image has finished downloading
    // we'll update the texture with the contents of the image.
    gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA,
                  1, 1, 0, gl.RGBA, gl.UNSIGNED_BYTE,
                  new Uint8Array([0, 0, 255, 255]));

    var image = new Image();
    image.onload = function() {
      texture.width = this.width;
      texture.height = this.height;
      gl.bindTexture(gl.TEXTURE_2D, texture);
      gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA,
                    gl.RGBA, gl.UNSIGNED_BYTE, image);

      // WebGL1 has different requirements for power of 2 images
      // vs non power of 2 images so check if the image is a
      // power of 2 in both dimensions.
      if (isPowerOf2(this.width) && isPowerOf2(this.height)) {
         // Yes, it's a power of 2. Generate mips.
         gl.generateMipmap(gl.TEXTURE_2D);
      } else {
         // No, it's not a power of 2. Turn of mips and set
         // wrapping to clamp to edge
         gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
         gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
         gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
      }
    };
    image.src = url;
    return texture;
  }
});
