using System;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class ItemPropertiesDisplay : MonoBehaviour
{
	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0002EBF8 File Offset: 0x0002CDF8
	private PrefabPool<LabelAndValue> EntryPool
	{
		get
		{
			if (this._entryPool == null)
			{
				this._entryPool = new PrefabPool<LabelAndValue>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._entryPool;
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x0002EC31 File Offset: 0x0002CE31
	private void Awake()
	{
		this.entryTemplate.gameObject.SetActive(false);
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x0002EC44 File Offset: 0x0002CE44
	internal void Setup(Item targetItem)
	{
		this.EntryPool.ReleaseAll();
		if (targetItem == null)
		{
			return;
		}
		foreach (ValueTuple<string, string, Polarity> valueTuple in targetItem.GetPropertyValueTextPair())
		{
			this.EntryPool.Get(null).Setup(valueTuple.Item1, valueTuple.Item2, valueTuple.Item3);
		}
	}

	// Token: 0x0400094C RID: 2380
	[SerializeField]
	private LabelAndValue entryTemplate;

	// Token: 0x0400094D RID: 2381
	private PrefabPool<LabelAndValue> _entryPool;
}
