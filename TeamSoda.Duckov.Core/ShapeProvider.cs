using System;
using UnityEngine;

// Token: 0x02000142 RID: 322
public abstract class ShapeProvider : MonoBehaviour
{
	// Token: 0x06000A49 RID: 2633
	public abstract PipeRenderer.OrientedPoint[] GenerateShape();
}
