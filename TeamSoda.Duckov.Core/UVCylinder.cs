using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class UVCylinder : MonoBehaviour
{
	// Token: 0x06000A73 RID: 2675 RVA: 0x0002E6A4 File Offset: 0x0002C8A4
	private void Generate()
	{
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
		}
		this.mesh.Clear();
		new List<Vector3>();
		new List<Vector2>();
		new List<Vector3>();
		new List<int>();
		for (int i = 0; i < this.subdivision; i++)
		{
		}
	}

	// Token: 0x0400092F RID: 2351
	public float radius = 1f;

	// Token: 0x04000930 RID: 2352
	public float height = 2f;

	// Token: 0x04000931 RID: 2353
	public int subdivision = 16;

	// Token: 0x04000932 RID: 2354
	private Mesh mesh;
}
