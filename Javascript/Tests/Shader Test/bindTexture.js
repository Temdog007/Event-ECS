define(function()
{
    function isPowerOf2(value)
    {
        return (value & (value-1)) == 0;
    }

    return function(image) {
        
        if(this instanceof Image)
        {
            image = this;
            this.texture.width = this.width;
            this.texture.height = this.height;
        }
        
        var texture = image.texture;
        var gl = image.gl;
        gl.bindTexture(gl.TEXTURE_2D, texture);
        gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA,
                      gl.RGBA, gl.UNSIGNED_BYTE, image);
  
        // WebGL1 has different requirements for power of 2 images
        // vs non power of 2 images so check if the image is a
        // power of 2 in both dimensions.
        if (isPowerOf2(image.width) && isPowerOf2(image.height)) 
        {
           // Yes, it's a power of 2. Generate mips.
           gl.generateMipmap(gl.TEXTURE_2D);
        } 
        else 
        {
           // No, it's not a power of 2. Turn of mips and set
           // wrapping to clamp to edge
           gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
           gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
           gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
        }
    }
});