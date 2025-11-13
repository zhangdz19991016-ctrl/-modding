using System;
using Duckov.Buffs;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class ModifierAction : EffectAction
{
	// Token: 0x060004D2 RID: 1234 RVA: 0x00015FA8 File Offset: 0x000141A8
	protected override void Awake()
	{
		base.Awake();
		this.modifier = new Modifier(this.ModifierType, this.modifierValue, this.overrideOrder, this.overrideOrderValue, base.Master);
		this.targetStatHash = this.targetStatKey.GetHashCode();
		if (this.buff)
		{
			this.buff.OnLayerChangedEvent += this.OnBuffLayerChanged;
		}
		this.OnBuffLayerChanged();
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0001601F File Offset: 0x0001421F
	private void OnBuffLayerChanged()
	{
		if (!this.buff)
		{
			return;
		}
		if (this.modifier == null)
		{
			return;
		}
		this.modifier.Value = this.modifierValue * (float)this.buff.CurrentLayers;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00016058 File Offset: 0x00014258
	protected override void OnTriggered(bool positive)
	{
		if (base.Master.Item == null)
		{
			return;
		}
		Item characterItem = base.Master.Item.GetCharacterItem();
		if (characterItem == null)
		{
			return;
		}
		if (positive)
		{
			if (this.targetStat != null)
			{
				this.targetStat.RemoveModifier(this.modifier);
				this.targetStat = null;
			}
			this.targetStat = characterItem.GetStat(this.targetStatHash);
			this.targetStat.AddModifier(this.modifier);
			return;
		}
		if (this.targetStat != null)
		{
			this.targetStat.RemoveModifier(this.modifier);
			this.targetStat = null;
		}
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00016100 File Offset: 0x00014300
	private void OnDestroy()
	{
		if (this.targetStat != null)
		{
			this.targetStat.RemoveModifier(this.modifier);
			this.targetStat = null;
		}
		if (this.buff)
		{
			this.buff.OnLayerChangedEvent -= this.OnBuffLayerChanged;
		}
	}

	// Token: 0x0400040B RID: 1035
	[SerializeField]
	private Buff buff;

	// Token: 0x0400040C RID: 1036
	public string targetStatKey;

	// Token: 0x0400040D RID: 1037
	private int targetStatHash;

	// Token: 0x0400040E RID: 1038
	public ModifierType ModifierType;

	// Token: 0x0400040F RID: 1039
	public float modifierValue;

	// Token: 0x04000410 RID: 1040
	public bool overrideOrder;

	// Token: 0x04000411 RID: 1041
	public int overrideOrderValue;

	// Token: 0x04000412 RID: 1042
	private Modifier modifier;

	// Token: 0x04000413 RID: 1043
	private Stat targetStat;
}
