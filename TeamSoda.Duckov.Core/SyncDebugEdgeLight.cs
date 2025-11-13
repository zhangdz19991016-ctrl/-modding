using System;
using Soda;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class SyncDebugEdgeLight : MonoBehaviour
{
	// Token: 0x0600066F RID: 1647 RVA: 0x0001D49C File Offset: 0x0001B69C
	private void Awake()
	{
		DebugView.OnDebugViewConfigChanged += this.OnDebugConfigChanged;
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0001D4AF File Offset: 0x0001B6AF
	private void OnDestroy()
	{
		DebugView.OnDebugViewConfigChanged -= this.OnDebugConfigChanged;
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0001D4C2 File Offset: 0x0001B6C2
	private void OnDebugConfigChanged(DebugView debugView)
	{
		if (debugView == null)
		{
			return;
		}
		base.gameObject.SetActive(debugView.EdgeLightActive);
	}
}
