using System;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class TagUtilities
{
	// Token: 0x06000537 RID: 1335 RVA: 0x00017A1C File Offset: 0x00015C1C
	public static Tag TagFromString(string name)
	{
		name = name.Trim();
		Tag tag = GameplayDataSettings.Tags.AllTags.FirstOrDefault((Tag e) => e != null && e.name == name);
		if (tag == null)
		{
			Debug.LogError("未找到Tag: " + name);
		}
		return tag;
	}
}
