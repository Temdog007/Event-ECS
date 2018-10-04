define(function()
{
  var vsSource = `
    attribute vec4 aVertexPosition;
    attribute vec2 aTextureCoord;

    uniform mat4 uProjectionMatrix;

    varying vec2 vTextureCoord;

    void main(){
      gl_Position = uProjectionMatrix * aVertexPosition;
      vTextureCoord = aTextureCoord;
    }
  `;

  var fsSource = `
    precision mediump float;

    varying vec2 vTextureCoord;

    uniform sampler2D uSampler;

    uniform vec4 oldColor;
    uniform vec4 newColor;

    void main(){
      //gl_FragColor = texture2D(uSampler, vTextureCoord) * tint;
      vec4 current = texture2D(uSampler, vTextureCoord);
      if(current == oldColor)
      {
        gl_FragColor = newColor;
      }
      else
      {
        gl_FragColor = current;
      }
    }
  `;

  return {
    vert : vsSource,
    frag : fsSource
  };
});
