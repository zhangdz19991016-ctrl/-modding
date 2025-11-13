using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.PerkTrees;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class PerkTreeManager : MonoBehaviour
{
	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0003AA7A File Offset: 0x00038C7A
	public static PerkTreeManager Instance
	{
		get
		{
			return PerkTreeManager.instance;
		}
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x0003AA81 File Offset: 0x00038C81
	private void Awake()
	{
		if (PerkTreeManager.instance == null)
		{
			PerkTreeManager.instance = this;
			return;
		}
		Debug.LogError("检测到多个PerkTreeManager");
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0003AAA4 File Offset: 0x00038CA4
	public static PerkTree GetPerkTree(string id)
	{
		if (PerkTreeManager.instance == null)
		{
			return null;
		}
		PerkTree perkTree = PerkTreeManager.instance.perkTrees.FirstOrDefault((PerkTree e) => e != null && e.ID == id);
		if (perkTree == null)
		{
			Debug.LogError("未找到PerkTree id:" + id);
		}
		return perkTree;
	}

	// Token: 0x04000BFB RID: 3067
	private static PerkTreeManager instance;

	// Token: 0x04000BFC RID: 3068
	public List<PerkTree> perkTrees;
}
