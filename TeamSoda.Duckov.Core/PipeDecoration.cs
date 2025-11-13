using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000144 RID: 324
[RequireComponent(typeof(PipeRenderer))]
public class PipeDecoration : MonoBehaviour
{
	// Token: 0x06000A4C RID: 2636 RVA: 0x0002C6B4 File Offset: 0x0002A8B4
	private void OnDrawGizmosSelected()
	{
		if (this.pipeRenderer == null)
		{
			this.pipeRenderer = base.GetComponent<PipeRenderer>();
		}
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x0002C6D0 File Offset: 0x0002A8D0
	private void Refresh()
	{
		if (this.pipeRenderer.splineInUse == null || this.pipeRenderer.splineInUse.Length < 1)
		{
			return;
		}
		for (int i = 0; i < this.decorations.Count; i++)
		{
			PipeDecoration.GameObjectOffset gameObjectOffset = this.decorations[i];
			Quaternion localRotation;
			Vector3 positionByOffset = this.pipeRenderer.GetPositionByOffset(gameObjectOffset.offset, out localRotation);
			Vector3 position = this.pipeRenderer.transform.localToWorldMatrix.MultiplyPoint(positionByOffset);
			if (!(gameObjectOffset.gameObject == null))
			{
				gameObjectOffset.gameObject.transform.position = position;
				gameObjectOffset.gameObject.transform.localRotation = localRotation;
				gameObjectOffset.gameObject.transform.Rotate(this.rotate);
				gameObjectOffset.gameObject.transform.localScale = this.scale * this.uniformScale;
			}
		}
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x0002C7BC File Offset: 0x0002A9BC
	public void OnValidate()
	{
		if (this.pipeRenderer == null)
		{
			this.pipeRenderer = base.GetComponent<PipeRenderer>();
		}
		this.Refresh();
	}

	// Token: 0x04000906 RID: 2310
	public PipeRenderer pipeRenderer;

	// Token: 0x04000907 RID: 2311
	[HideInInspector]
	public List<PipeDecoration.GameObjectOffset> decorations = new List<PipeDecoration.GameObjectOffset>();

	// Token: 0x04000908 RID: 2312
	public Vector3 rotate;

	// Token: 0x04000909 RID: 2313
	public Vector3 scale = Vector3.one;

	// Token: 0x0400090A RID: 2314
	public float uniformScale = 1f;

	// Token: 0x020004AC RID: 1196
	[Serializable]
	public class GameObjectOffset
	{
		// Token: 0x04001C6F RID: 7279
		public GameObject gameObject;

		// Token: 0x04001C70 RID: 7280
		public float offset;
	}
}
