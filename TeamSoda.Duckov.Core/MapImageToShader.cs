using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013A RID: 314
public class MapImageToShader : MonoBehaviour
{
	// Token: 0x06000A28 RID: 2600 RVA: 0x0002BD8C File Offset: 0x00029F8C
	private void Start()
	{
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0002BD90 File Offset: 0x00029F90
	private void Update()
	{
		if (!this.material)
		{
			this.material = base.GetComponent<Image>().material;
		}
		if (!this.material)
		{
			return;
		}
		Rect rect = this.rect.rect;
		Vector3 vector = rect.min;
		Vector3 vector2 = rect.max;
		Vector3 vector3 = new Vector3(vector.x, vector.y);
		Vector3 a = new Vector3(vector.x, vector2.y);
		Vector3 a2 = new Vector3(vector2.x, vector.y);
		Vector3 v = base.transform.TransformPoint(vector3);
		Vector3 v2 = base.transform.TransformVector(a - vector3);
		Vector3 v3 = base.transform.TransformVector(a2 - vector3);
		this.material.SetVector(this.zeroPointID, v);
		this.material.SetVector(this.upVectorID, v2);
		this.material.SetVector(this.rightVectorID, v3);
		this.material.SetFloat(this.scaleID, this.rect.lossyScale.x);
	}

	// Token: 0x040008ED RID: 2285
	public RectTransform rect;

	// Token: 0x040008EE RID: 2286
	private Material material;

	// Token: 0x040008EF RID: 2287
	private int zeroPointID = Shader.PropertyToID("_ZeroPoint");

	// Token: 0x040008F0 RID: 2288
	private int upVectorID = Shader.PropertyToID("_UpVector");

	// Token: 0x040008F1 RID: 2289
	private int rightVectorID = Shader.PropertyToID("_RightVector");

	// Token: 0x040008F2 RID: 2290
	private int scaleID = Shader.PropertyToID("_Scale");
}
