using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MeshBuilder
{

	public List<int> Triangles 
    {
        get;
        private set;
    }
    public List<Vector3> Vertices
    {
        get;
        private set;
    }
    public List<Vector3> Normals
    {
        get;
        private set;
    }
    public List<Vector2> UVs
    {
        get;
        private set;
    }
	
	public MeshBuilder ()
	{
		Triangles = new List<int> ();
		Vertices = new List<Vector3> ();
		Normals = new List<Vector3> ();
		UVs = new List<Vector2> ();
	}
	
	public void AddTriangleToMesh (Vector3[] triangleVertices, Vector3[] normals, Vector2[] uvs)
	{
		for (int i = 0; i < 3; i++) {
			Vector3 vertex = triangleVertices [i];
			
			/*this.Vertices.Add (vertex);
			this.Normals.Add (normals [i]);
			this.UVs.Add (uvs [i]);
            this.Triangles.Add(this.Vertices.Count - 1);*/
			
			if (!this.Vertices.Contains (vertex)) {
				this.Vertices.Add (vertex);
				this.Normals.Add (normals [i]);
				this.UVs.Add (uvs [i]);
                this.Triangles.Add(this.Vertices.Count - 1);
			} else {
                int[] indicesForVertex = IndexOfAllOccurences(this.Vertices, vertex);
                bool itAlreadyExisted = false;
                foreach (int vertexIndex in indicesForVertex)
                {
                    if (this.Normals[vertexIndex] == normals[i] && this.UVs[vertexIndex] == uvs[i])
                    {
                        this.Triangles.Add(vertexIndex);
                        itAlreadyExisted = true;
                        break;
                    }
                }

                if (!itAlreadyExisted)
                {
                    this.Vertices.Add(vertex);
                    this.Normals.Add(normals[i]);
                    this.UVs.Add(uvs[i]);
                    this.Triangles.Add(this.Vertices.Count - 1);
                }
			}
		}
	}

    int[] IndexOfAllOccurences(List<Vector3> vertices, Vector3 vertex)
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < vertices.Count; i++)
        {
            if (vertices[i] == vertex)
            {
                indices.Add(i);
            }
        }
        return indices.ToArray();
    }

    public void Combine(MeshBuilder otherBuilder)
    {
        for (int i = 0; i < otherBuilder.Triangles.Count; i += 3)
        {
            Vector3[] tri = new Vector3[] { otherBuilder.Vertices[otherBuilder.Triangles[i]], otherBuilder.Vertices[otherBuilder.Triangles[i + 1]], otherBuilder.Vertices[otherBuilder.Triangles[i + 2]] };
            Vector3[] normals = new Vector3[] { otherBuilder.Normals[otherBuilder.Triangles[i]], otherBuilder.Normals[otherBuilder.Triangles[i + 1]], otherBuilder.Normals[otherBuilder.Triangles[i + 2]] };
            Vector2[] UVs = new Vector2[] { otherBuilder.UVs[otherBuilder.Triangles[i]], otherBuilder.UVs[otherBuilder.Triangles[i + 1]], otherBuilder.UVs[otherBuilder.Triangles[i + 2]] };
            this.AddTriangleToMesh(tri, normals, UVs);
        }
    }

    /*public void Clear()
    {
        this.Triangles.Clear();
        this.Vertices.Clear();
        this.Normals.Clear();
        this.UVs.Clear();
    }*/
	
	public Mesh Build ()
	{
		//RemoveCollapsedTriangles();
		
		Mesh mesh = new Mesh ();
		
		mesh.vertices = Vertices.ToArray ();
		mesh.triangles = Triangles.ToArray ();
		mesh.normals = Normals.ToArray ();
		mesh.uv = UVs.ToArray ();
		mesh.uv2 = UVs.ToArray ();
		mesh.uv3 = UVs.ToArray ();
		//mesh.RecalculateNormals();
		mesh.RecalculateBounds ();
		mesh.Optimize ();
		
		return mesh;
	}

	private void RemoveCollapsedTriangles ()
	{
		//Debug.Log("triangle count " + triangles.Count);
		Queue<int[]> trisToBeRemoved = new Queue<int[]>();
		
		for (int i = 0; i < Triangles.Count; i+=3)
		{
			Vector3[] tri = new Vector3[]{ Vertices[Triangles[i]], Vertices[Triangles[i + 1]], Vertices[Triangles[i + 2]]};
			if (tri[0] == tri[1] || tri[0] == tri[2] || tri[1] == tri[2])
			{
				//Debug.Log(triangles[i] + ", " + triangles[i + 1] + ", " + triangles[i + 2]);
				//Debug.LogError("Collapsed triangle detected: ("+ tri[0]+ ", " + tri[1] + ", " + tri[2] + ")");
				trisToBeRemoved.Enqueue(new int[]{Triangles[i], Triangles[i + 1], Triangles[i + 2]});
			}
		}
		
		while (trisToBeRemoved.Count > 0)
		{
			int[] triToRemove = trisToBeRemoved.Dequeue();
			for (int i = 0; i < Triangles.Count; i+=3)
			{
				if (Triangles[i] == triToRemove[0] && Triangles[i + 1] == triToRemove[1] && Triangles[i + 2] == triToRemove[2])
				{
					Vector3[] tri = new Vector3[]{ Vertices[Triangles[i]], Vertices[Triangles[i + 1]], Vertices[Triangles[i + 2]]};
					Debug.LogError("Removed ("+ tri[0]+ ", " + tri[1] + ", " + tri[2] + ")");
					Triangles.RemoveAt(i + 2);
					Triangles.RemoveAt(i + 1);
					Triangles.RemoveAt(i);
					break;
				}
					
			}
		}
	}
}
