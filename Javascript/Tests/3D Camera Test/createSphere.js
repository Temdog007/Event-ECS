define(['m4'], function(mat4)
{
  return function(radius, segments)
  {
    var verts = [];
    var beta = Math.PI * 2 / segments;

    for(var xRotation = 0; xRotation < Math.PI * 2; xRotation += beta)
    {
      for(var zRotation = 0; zRotation < Math.PI * 2; zRotation += beta)
      { 
        var startPoint = {
          x : Math.cos(zRotation) * radius,
          y : Math.sin(zRotation) * radius,
          z : 0
        }
        var mat = mat4.xRotation(xRotation);
        mat = mat4.translate(mat, startPoint.x, startPoint.y, startPoint.z);
        
        var point = {
          x : mat[12],
          y : mat[13],
          z : mat[14]
        };
        verts.push(point.x, point.y, point.z);
      }
    }

    var vertices = segments * segments;

    var indices = [];
    var texcoords = [];
    for(var i = 0; i < verts.length; i += 3)
    {
      var vertexIndex = i / 3;

      var nextVertex = (vertexIndex + 1) % vertices;
      var prevVertex = (vertexIndex - 1 + vertices) % vertices;
      var nextCircle = (vertexIndex + segments) % vertices;
      var prevCircle = (vertexIndex - segments + vertices) % vertices;

      indices.push(vertexIndex, nextVertex, nextCircle);
      indices.push(vertexIndex, nextVertex, prevCircle);
      indices.push(vertexIndex, prevVertex, nextCircle);
      indices.push(vertexIndex, prevVertex, prevCircle);

      if(prevVertex >= vertices)
      {
        console.log("prevVertex", vertexIndex, prevVertex);
      }
      if(prevCircle >= vertices)
      {
        console.log("vertex", vertexIndex, prevCircle);
      }
    }
    
    for(var i = 0; i < vertices; i += 3) // every three vertices needs a coresspond color
    {
      var index = i / 3;
      var x = (index % segments) / segments;
      var y = (verts[i+1] + radius) / radius;
      texcoords.push(x, y);
    }

    return {
      verts : verts,
      indices : indices,
      texcoords : texcoords,
      vertices : vertices
    }
  }
})