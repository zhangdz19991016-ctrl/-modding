using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000207 RID: 519
public class ForceUnmaskable : MonoBehaviour
{
	// Token: 0x06000F4B RID: 3915 RVA: 0x0003CD60 File Offset: 0x0003AF60
	private void OnEnable()
	{
		MaskableGraphic[] components = base.GetComponents<MaskableGraphic>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].maskable = false;
		}
	}
}
