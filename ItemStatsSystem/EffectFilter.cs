using System;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000010 RID: 16
	[RequireComponent(typeof(Effect))]
	public class EffectFilter : EffectComponent, ISelfValidator
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000032DF File Offset: 0x000014DF
		protected override Color ActiveLabelColor
		{
			get
			{
				return DuckovUtilitiesSettings.Colors.EffectFilter;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000032EB File Offset: 0x000014EB
		public override string DisplayName
		{
			get
			{
				return "未命名过滤器(" + base.GetType().Name + ")";
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003307 File Offset: 0x00001507
		// (set) Token: 0x06000067 RID: 103 RVA: 0x0000330F File Offset: 0x0000150F
		protected bool IgnoreNegativeTrigger
		{
			get
			{
				return this.ignoreNegativeTrigger;
			}
			set
			{
				this.ignoreNegativeTrigger = value;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003318 File Offset: 0x00001518
		public bool Evaluate(EffectTriggerEventContext context)
		{
			return !base.enabled || (!context.positive && this.IgnoreNegativeTrigger) || this.OnEvaluate(context);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000333D File Offset: 0x0000153D
		protected virtual bool OnEvaluate(EffectTriggerEventContext context)
		{
			return true;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003340 File Offset: 0x00001540
		public override void Validate(SelfValidationResult result)
		{
			base.Validate(result);
			if (base.Master != null && !base.Master.Filters.Contains(this))
			{
				result.AddError("Master 中不包含本 Filter。").WithFix("将此 Filter 添加到 Master 中。", delegate()
				{
					base.Master.AddEffectComponent(this);
				}, true);
			}
		}

		// Token: 0x0400002C RID: 44
		[SerializeField]
		private bool ignoreNegativeTrigger = true;
	}
}
