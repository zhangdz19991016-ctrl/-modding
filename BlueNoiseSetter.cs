using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class BlueNoiseSetter : MonoBehaviour
{
	// Token: 0x06000A33 RID: 2611 RVA: 0x0002C018 File Offset: 0x0002A218
	private void Update()
	{
		Shader.SetGlobalTexture("GlobalBlueNoise", this.blueNoises[this.index]);
		this.index++;
		if (this.index >= this.blueNoises.Count)
		{
			this.index = 0;
		}
	}

	// Token: 0x040008F7 RID: 2295
	public List<Texture2D> blueNoises;

	// Token: 0x040008F8 RID: 2296
	private int index;
}
