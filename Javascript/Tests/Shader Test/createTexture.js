define(function()
{
    return function(gl)
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
                    new Uint8Array([255, 255, 255, 255]));
        
        return texture;
    }
});