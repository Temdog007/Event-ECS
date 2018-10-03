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

    uniform vec4 tint;

    void main(){
      gl_FragColor = texture2D(uSampler, vTextureCoord) * tint;
    }
  `;

  return {
    vert : vsSource,
    frag : fsSource
  };
});
