define(function()
{
  var vsSource = `
  precision mediump float;
  
    attribute vec4 aVertexPosition;
    attribute vec2 aTextureCoord;

    uniform mat4 uProjectionMatrix;

    varying vec2 vTextureCoord;

    void main(){
      gl_Position = uProjectionMatrix * aVertexPosition;
      vTextureCoord = aTextureCoord;
    }
  `;

  var defFs = `
    precision mediump float;

    varying vec2 vTextureCoord;

    uniform sampler2D uSampler;

    void main(){
      // gl_FragColor = texture2D(uSampler, vTextureCoord);
      gl_FragColor = vec4(1,0,0,1);
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

  var blur = `
  precision mediump float;
  varying vec2 vTextureCoord;
  uniform sampler2D uSampler;
  uniform vec2 direction;

  void main()
  {
    vec4 c = vec4(0.0);

    c += vec4(0.011109) * texture2D(uSampler, vTextureCoord + vec2(-30.000000) * direction);
    c += vec4(0.014921) * texture2D(uSampler, vTextureCoord + vec2(-29.000000) * direction);
    c += vec4(0.019841) * texture2D(uSampler, vTextureCoord + vec2(-28.000000) * direction);
    c += vec4(0.026121) * texture2D(uSampler, vTextureCoord + vec2(-27.000000) * direction);
    c += vec4(0.034047) * texture2D(uSampler, vTextureCoord + vec2(-26.000000) * direction);
    c += vec4(0.043937) * texture2D(uSampler, vTextureCoord + vec2(-25.000000) * direction);
    c += vec4(0.056135) * texture2D(uSampler, vTextureCoord + vec2(-24.000000) * direction);
    c += vec4(0.071005) * texture2D(uSampler, vTextureCoord + vec2(-23.000000) * direction);
    c += vec4(0.088922) * texture2D(uSampler, vTextureCoord + vec2(-22.000000) * direction);
    c += vec4(0.110251) * texture2D(uSampler, vTextureCoord + vec2(-21.000000) * direction);
    c += vec4(0.135335) * texture2D(uSampler, vTextureCoord + vec2(-20.000000) * direction);
    c += vec4(0.164474) * texture2D(uSampler, vTextureCoord + vec2(-19.000000) * direction);
    c += vec4(0.197899) * texture2D(uSampler, vTextureCoord + vec2(-18.000000) * direction);
    c += vec4(0.235746) * texture2D(uSampler, vTextureCoord + vec2(-17.000000) * direction);
    c += vec4(0.278037) * texture2D(uSampler, vTextureCoord + vec2(-16.000000) * direction);
    c += vec4(0.324652) * texture2D(uSampler, vTextureCoord + vec2(-15.000000) * direction);
    c += vec4(0.375311) * texture2D(uSampler, vTextureCoord + vec2(-14.000000) * direction);
    c += vec4(0.429557) * texture2D(uSampler, vTextureCoord + vec2(-13.000000) * direction);
    c += vec4(0.486752) * texture2D(uSampler, vTextureCoord + vec2(-12.000000) * direction);
    c += vec4(0.546074) * texture2D(uSampler, vTextureCoord + vec2(-11.000000) * direction);
    c += vec4(0.606531) * texture2D(uSampler, vTextureCoord + vec2(-10.000000) * direction);
    c += vec4(0.666977) * texture2D(uSampler, vTextureCoord + vec2(-9.000000) * direction);
    c += vec4(0.726149) * texture2D(uSampler, vTextureCoord + vec2(-8.000000) * direction);
    c += vec4(0.782705) * texture2D(uSampler, vTextureCoord + vec2(-7.000000) * direction);
    c += vec4(0.835270) * texture2D(uSampler, vTextureCoord + vec2(-6.000000) * direction);
    c += vec4(0.882497) * texture2D(uSampler, vTextureCoord + vec2(-5.000000) * direction);
    c += vec4(0.923116) * texture2D(uSampler, vTextureCoord + vec2(-4.000000) * direction);
    c += vec4(0.955997) * texture2D(uSampler, vTextureCoord + vec2(-3.000000) * direction);
    c += vec4(0.980199) * texture2D(uSampler, vTextureCoord + vec2(-2.000000) * direction);
    c += vec4(0.995012) * texture2D(uSampler, vTextureCoord + vec2(-1.000000) * direction);
    c += vec4(1.000000) * texture2D(uSampler, vTextureCoord + vec2(0.000000) * direction);
    c += vec4(0.995012) * texture2D(uSampler, vTextureCoord + vec2(1.000000) * direction);
    c += vec4(0.980199) * texture2D(uSampler, vTextureCoord + vec2(2.000000) * direction);
    c += vec4(0.955997) * texture2D(uSampler, vTextureCoord + vec2(3.000000) * direction);
    c += vec4(0.923116) * texture2D(uSampler, vTextureCoord + vec2(4.000000) * direction);
    c += vec4(0.882497) * texture2D(uSampler, vTextureCoord + vec2(5.000000) * direction);
    c += vec4(0.835270) * texture2D(uSampler, vTextureCoord + vec2(6.000000) * direction);
    c += vec4(0.782705) * texture2D(uSampler, vTextureCoord + vec2(7.000000) * direction);
    c += vec4(0.726149) * texture2D(uSampler, vTextureCoord + vec2(8.000000) * direction);
    c += vec4(0.666977) * texture2D(uSampler, vTextureCoord + vec2(9.000000) * direction);
    c += vec4(0.606531) * texture2D(uSampler, vTextureCoord + vec2(10.000000) * direction);
    c += vec4(0.546074) * texture2D(uSampler, vTextureCoord + vec2(11.000000) * direction);
    c += vec4(0.486752) * texture2D(uSampler, vTextureCoord + vec2(12.000000) * direction);
    c += vec4(0.429557) * texture2D(uSampler, vTextureCoord + vec2(13.000000) * direction);
    c += vec4(0.375311) * texture2D(uSampler, vTextureCoord + vec2(14.000000) * direction);
    c += vec4(0.324652) * texture2D(uSampler, vTextureCoord + vec2(15.000000) * direction);
    c += vec4(0.278037) * texture2D(uSampler, vTextureCoord + vec2(16.000000) * direction);
    c += vec4(0.235746) * texture2D(uSampler, vTextureCoord + vec2(17.000000) * direction);
    c += vec4(0.197899) * texture2D(uSampler, vTextureCoord + vec2(18.000000) * direction);
    c += vec4(0.164474) * texture2D(uSampler, vTextureCoord + vec2(19.000000) * direction);
    c += vec4(0.135335) * texture2D(uSampler, vTextureCoord + vec2(20.000000) * direction);
    c += vec4(0.110251) * texture2D(uSampler, vTextureCoord + vec2(21.000000) * direction);
    c += vec4(0.088922) * texture2D(uSampler, vTextureCoord + vec2(22.000000) * direction);
    c += vec4(0.071005) * texture2D(uSampler, vTextureCoord + vec2(23.000000) * direction);
    c += vec4(0.056135) * texture2D(uSampler, vTextureCoord + vec2(24.000000) * direction);
    c += vec4(0.043937) * texture2D(uSampler, vTextureCoord + vec2(25.000000) * direction);
    c += vec4(0.034047) * texture2D(uSampler, vTextureCoord + vec2(26.000000) * direction);
    c += vec4(0.026121) * texture2D(uSampler, vTextureCoord + vec2(27.000000) * direction);
    c += vec4(0.019841) * texture2D(uSampler, vTextureCoord + vec2(28.000000) * direction);
    c += vec4(0.014921) * texture2D(uSampler, vTextureCoord + vec2(29.000000) * direction);
    c += vec4(0.011109) * texture2D(uSampler, vTextureCoord + vec2(30.000000) * direction);
    gl_FragColor = c * vec4(0.039985);
  }
  `;

  var threshold = `
    precision mediump float;
    varying vec2 vTextureCoord;
    uniform sampler2D uSampler;
    uniform float min_luma;
    void main()
    {
      vec4 c = texture2D(uSampler, vTextureCoord);
      float luma = dot(vec3(0.299, 0.587, 0.114), c.rgb);
      gl_FragColor = c * step(min_luma, luma);
    }
  `;

  var godsray = `
    precision mediump float;
    varying vec2 vTextureCoord;
    uniform sampler2D uSampler;

    uniform float exposure;
    uniform float decay;
    uniform float density;
    uniform float weight;
    uniform vec2 light_position;
    uniform float samples;

    const int max = 100;
    void main()
    {
      vec2 uv = vTextureCoord;
      vec4 color = texture2D(uSampler, uv); 
      
      vec2 offset = (uv - light_position) * density / samples;
      float ill = decay;
      vec4 c = vec4(0.0, 0.0, 0.0, 1.0);

      for(int i = 0; i < max; ++i)
      {
        if(i > int(samples))
        {
          break;
        }
        uv -= offset;
        c += texture2D(uSampler, uv) * ill * weight;
        ill *= decay;
      }

      gl_FragColor = vec4(c.rgb * exposure + color.rgb, color.a);
    }
  `;

  return {
    vert : vsSource,
    frag : fsSource,
    defFs : defFs,
    blur : blur,
    threshold : threshold,
    godsray : godsray
  };
});
