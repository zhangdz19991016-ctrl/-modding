using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000FB RID: 251
[Serializable]
public struct RandomItemGenerateDescription
{
	// Token: 0x06000870 RID: 2160 RVA: 0x00025D7C File Offset: 0x00023F7C
	public UniTask<List<Item>> Generate(int count = -1)
	{
		RandomItemGenerateDescription.<Generate>d__12 <Generate>d__;
		<Generate>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<Generate>d__.<>4__this = this;
		<Generate>d__.count = count;
		<Generate>d__.<>1__state = -1;
		<Generate>d__.<>t__builder.Start<RandomItemGenerateDescription.<Generate>d__12>(ref <Generate>d__);
		return <Generate>d__.<>t__builder.Task;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00025DCC File Offset: 0x00023FCC
	private void SetDurabilityIfNeeded(Item targetItem)
	{
		if (targetItem == null)
		{
			return;
		}
		if (this.controlDurability && targetItem.UseDurability)
		{
			float num = UnityEngine.Random.Range(this.durabilityIntegrity.x, this.durabilityIntegrity.y);
			targetItem.DurabilityLoss = 1f - num;
			float num2 = UnityEngine.Random.Range(this.durability.x, this.durability.y);
			if (num2 > num)
			{
				num2 = num;
			}
			targetItem.Durability = targetItem.MaxDurability * num2;
		}
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x00025E4C File Offset: 0x0002404C
	private void RefreshPercent()
	{
		this.itemPool.RefreshPercent();
	}

	// Token: 0x040007A7 RID: 1959
	[TextArea]
	[SerializeField]
	private string comment;

	// Token: 0x040007A8 RID: 1960
	[Range(0f, 1f)]
	public float chance;

	// Token: 0x040007A9 RID: 1961
	public Vector2Int randomCount;

	// Token: 0x040007AA RID: 1962
	public bool controlDurability;

	// Token: 0x040007AB RID: 1963
	public Vector2 durability;

	// Token: 0x040007AC RID: 1964
	public Vector2 durabilityIntegrity;

	// Token: 0x040007AD RID: 1965
	public bool randomFromPool;

	// Token: 0x040007AE RID: 1966
	[SerializeField]
	public RandomContainer<RandomItemGenerateDescription.Entry> itemPool;

	// Token: 0x040007AF RID: 1967
	public RandomContainer<Tag> tags;

	// Token: 0x040007B0 RID: 1968
	public List<Tag> addtionalRequireTags;

	// Token: 0x040007B1 RID: 1969
	public List<Tag> excludeTags;

	// Token: 0x040007B2 RID: 1970
	public RandomContainer<int> qualities;

	// Token: 0x02000488 RID: 1160
	[Serializable]
	public struct Entry
	{
		// Token: 0x04001BBE RID: 7102
		[ItemTypeID]
		[SerializeField]
		public int itemTypeID;
	}
}
