using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DA RID: 986
	public class MaterialPropertyFade : FadeElement
	{
		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060023E9 RID: 9193 RVA: 0x0007DE50 File Offset: 0x0007C050
		// (set) Token: 0x060023EA RID: 9194 RVA: 0x0007DE58 File Offset: 0x0007C058
		public AnimationCurve ShowCurve
		{
			get
			{
				return this.showCurve;
			}
			set
			{
				this.showCurve = value;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x0007DE61 File Offset: 0x0007C061
		// (set) Token: 0x060023EC RID: 9196 RVA: 0x0007DE69 File Offset: 0x0007C069
		public AnimationCurve HideCurve
		{
			get
			{
				return this.hideCurve;
			}
			set
			{
				this.hideCurve = value;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x0007DE74 File Offset: 0x0007C074
		private Material Material
		{
			get
			{
				if (this._material == null && this.renderer != null)
				{
					this._material = UnityEngine.Object.Instantiate<Material>(this.renderer.material);
					this.renderer.material = this._material;
				}
				return this._material;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x0007DECA File Offset: 0x0007C0CA
		// (set) Token: 0x060023EF RID: 9199 RVA: 0x0007DED2 File Offset: 0x0007C0D2
		public float Duration
		{
			get
			{
				return this.duration;
			}
			internal set
			{
				this.duration = value;
			}
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x0007DEDB File Offset: 0x0007C0DB
		private void Awake()
		{
			if (this.renderer == null)
			{
				this.renderer = base.GetComponent<Image>();
			}
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0007DEF7 File Offset: 0x0007C0F7
		private void OnDestroy()
		{
			if (this._material)
			{
				UnityEngine.Object.Destroy(this._material);
			}
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0007DF14 File Offset: 0x0007C114
		protected override UniTask HideTask(int token)
		{
			MaterialPropertyFade.<HideTask>d__20 <HideTask>d__;
			<HideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<HideTask>d__.<>4__this = this;
			<HideTask>d__.token = token;
			<HideTask>d__.<>1__state = -1;
			<HideTask>d__.<>t__builder.Start<MaterialPropertyFade.<HideTask>d__20>(ref <HideTask>d__);
			return <HideTask>d__.<>t__builder.Task;
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x0007DF5F File Offset: 0x0007C15F
		protected override void OnSkipHide()
		{
			if (this.Material == null)
			{
				return;
			}
			this.Material.SetFloat(this.propertyName, this.propertyRange.x);
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x0007DF8C File Offset: 0x0007C18C
		protected override void OnSkipShow()
		{
			if (this.Material == null)
			{
				return;
			}
			this.Material.SetFloat(this.propertyName, this.propertyRange.y);
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0007DFBC File Offset: 0x0007C1BC
		protected override UniTask ShowTask(int token)
		{
			MaterialPropertyFade.<ShowTask>d__23 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.token = token;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<MaterialPropertyFade.<ShowTask>d__23>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x0007E084 File Offset: 0x0007C284
		[CompilerGenerated]
		internal static float <HideTask>g__TimeSinceFadeBegun|20_0(ref MaterialPropertyFade.<>c__DisplayClass20_0 A_0)
		{
			return Time.unscaledTime - A_0.timeWhenFadeBegun;
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x0007E092 File Offset: 0x0007C292
		[CompilerGenerated]
		internal static float <ShowTask>g__TimeSinceFadeBegun|23_0(ref MaterialPropertyFade.<>c__DisplayClass23_0 A_0)
		{
			return Time.unscaledTime - A_0.timeWhenFadeBegun;
		}

		// Token: 0x0400185B RID: 6235
		[SerializeField]
		private Image renderer;

		// Token: 0x0400185C RID: 6236
		[SerializeField]
		private string propertyName = "t";

		// Token: 0x0400185D RID: 6237
		[SerializeField]
		private Vector2 propertyRange = new Vector2(0f, 1f);

		// Token: 0x0400185E RID: 6238
		[SerializeField]
		private float duration = 0.5f;

		// Token: 0x0400185F RID: 6239
		[SerializeField]
		private AnimationCurve showCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001860 RID: 6240
		[SerializeField]
		private AnimationCurve hideCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001861 RID: 6241
		private Material _material;
	}
}
