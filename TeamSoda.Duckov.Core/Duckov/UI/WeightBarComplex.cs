using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C9 RID: 969
	public class WeightBarComplex : MonoBehaviour
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002361 RID: 9057 RVA: 0x0007C366 File Offset: 0x0007A566
		private CharacterMainControl Target
		{
			get
			{
				if (!this.target)
				{
					LevelManager instance = LevelManager.Instance;
					this.target = ((instance != null) ? instance.MainCharacter : null);
				}
				return this.target;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002362 RID: 9058 RVA: 0x0007C392 File Offset: 0x0007A592
		private float LightPercentage
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002363 RID: 9059 RVA: 0x0007C399 File Offset: 0x0007A599
		private float SuperHeavyPercentage
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x0007C3A0 File Offset: 0x0007A5A0
		private float MaxWeight
		{
			get
			{
				if (this.Target == null)
				{
					return 0f;
				}
				return this.Target.MaxWeight;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x0007C3C4 File Offset: 0x0007A5C4
		private float BarWidth
		{
			get
			{
				if (this.barArea == null)
				{
					return 0f;
				}
				return this.barArea.rect.width;
			}
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0007C3F8 File Offset: 0x0007A5F8
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
			if (this.Target)
			{
				this.Target.CharacterItem.onChildChanged += this.OnTargetChildChanged;
			}
			this.RefreshMarkPositions();
			this.ResetMainBar();
			this.Animate().Forget();
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0007C456 File Offset: 0x0007A656
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
			if (this.Target)
			{
				this.Target.CharacterItem.onChildChanged -= this.OnTargetChildChanged;
			}
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0007C494 File Offset: 0x0007A694
		private void RefreshMarkPositions()
		{
			if (this.lightMark == null)
			{
				return;
			}
			if (this.superHeavyMark == null)
			{
				return;
			}
			float d = this.BarWidth * this.LightPercentage;
			float d2 = this.BarWidth * this.SuperHeavyPercentage;
			this.lightMark.anchoredPosition = Vector2.right * d;
			this.superHeavyMark.anchoredPosition = Vector2.right * d2;
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0007C508 File Offset: 0x0007A708
		private void RefreshMarkStatus()
		{
			float num = 0f;
			if (this.MaxWeight > 0f)
			{
				num = this.Target.CharacterItem.TotalWeight / this.MaxWeight;
			}
			this.lightMarkToggle.SetToggle(num > this.LightPercentage);
			this.superHeavyMarkToggle.SetToggle(num > this.SuperHeavyPercentage);
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x0007C568 File Offset: 0x0007A768
		private void OnTargetChildChanged(Item item)
		{
			this.Animate().Forget();
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x0007C575 File Offset: 0x0007A775
		private void OnItemSelectionChanged()
		{
			this.Animate().Forget();
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x0007C584 File Offset: 0x0007A784
		private UniTask Animate()
		{
			WeightBarComplex.<Animate>d__33 <Animate>d__;
			<Animate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Animate>d__.<>4__this = this;
			<Animate>d__.<>1__state = -1;
			<Animate>d__.<>t__builder.Start<WeightBarComplex.<Animate>d__33>(ref <Animate>d__);
			return <Animate>d__.<>t__builder.Task;
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0007C5C8 File Offset: 0x0007A7C8
		private void ResetChangeBars()
		{
			this.positiveBar.DOKill(false);
			this.negativeBar.DOKill(false);
			this.positiveBar.sizeDelta = new Vector2(this.positiveBar.sizeDelta.x, 0f);
			this.negativeBar.sizeDelta = new Vector2(this.negativeBar.sizeDelta.x, 0f);
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x0007C639 File Offset: 0x0007A839
		private void ResetMainBar()
		{
			this.mainBar.DOKill(false);
			this.mainBar.sizeDelta = new Vector2(this.mainBar.sizeDelta.x, 0f);
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0007C670 File Offset: 0x0007A870
		private UniTask AnimateMainBar(int token)
		{
			WeightBarComplex.<AnimateMainBar>d__37 <AnimateMainBar>d__;
			<AnimateMainBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateMainBar>d__.<>4__this = this;
			<AnimateMainBar>d__.token = token;
			<AnimateMainBar>d__.<>1__state = -1;
			<AnimateMainBar>d__.<>t__builder.Start<WeightBarComplex.<AnimateMainBar>d__37>(ref <AnimateMainBar>d__);
			return <AnimateMainBar>d__.<>t__builder.Task;
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0007C6BC File Offset: 0x0007A8BC
		private UniTask AnimatePositiveBar(int token)
		{
			WeightBarComplex.<AnimatePositiveBar>d__38 <AnimatePositiveBar>d__;
			<AnimatePositiveBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimatePositiveBar>d__.<>4__this = this;
			<AnimatePositiveBar>d__.token = token;
			<AnimatePositiveBar>d__.<>1__state = -1;
			<AnimatePositiveBar>d__.<>t__builder.Start<WeightBarComplex.<AnimatePositiveBar>d__38>(ref <AnimatePositiveBar>d__);
			return <AnimatePositiveBar>d__.<>t__builder.Task;
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x0007C708 File Offset: 0x0007A908
		private UniTask AnimateNegativeBar(int token)
		{
			WeightBarComplex.<AnimateNegativeBar>d__39 <AnimateNegativeBar>d__;
			<AnimateNegativeBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateNegativeBar>d__.<>4__this = this;
			<AnimateNegativeBar>d__.token = token;
			<AnimateNegativeBar>d__.<>1__state = -1;
			<AnimateNegativeBar>d__.<>t__builder.Start<WeightBarComplex.<AnimateNegativeBar>d__39>(ref <AnimateNegativeBar>d__);
			return <AnimateNegativeBar>d__.<>t__builder.Task;
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x0007C753 File Offset: 0x0007A953
		private void SetupInvalid()
		{
			WeightBarComplex.SetSizeDeltaY(this.mainBar, 0f);
			WeightBarComplex.SetSizeDeltaY(this.positiveBar, 0f);
			WeightBarComplex.SetSizeDeltaY(this.negativeBar, 0f);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0007C788 File Offset: 0x0007A988
		private static void SetSizeDeltaY(RectTransform rectTransform, float sizeDelta)
		{
			Vector2 sizeDelta2 = rectTransform.sizeDelta;
			sizeDelta2.y = sizeDelta;
			rectTransform.sizeDelta = sizeDelta2;
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0007C7AB File Offset: 0x0007A9AB
		private static float GetSizeDeltaY(RectTransform rectTransform)
		{
			return rectTransform.sizeDelta.y;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0007C7B8 File Offset: 0x0007A9B8
		private float WeightToRectHeight(float weight)
		{
			if (this.MaxWeight <= 0f)
			{
				return 0f;
			}
			float num = weight / this.MaxWeight;
			return this.BarWidth * num;
		}

		// Token: 0x04001802 RID: 6146
		[SerializeField]
		private CharacterMainControl target;

		// Token: 0x04001803 RID: 6147
		[SerializeField]
		private RectTransform barArea;

		// Token: 0x04001804 RID: 6148
		[SerializeField]
		private RectTransform mainBar;

		// Token: 0x04001805 RID: 6149
		[SerializeField]
		private Graphic mainBarGraphic;

		// Token: 0x04001806 RID: 6150
		[SerializeField]
		private RectTransform positiveBar;

		// Token: 0x04001807 RID: 6151
		[SerializeField]
		private RectTransform negativeBar;

		// Token: 0x04001808 RID: 6152
		[SerializeField]
		private RectTransform lightMark;

		// Token: 0x04001809 RID: 6153
		[SerializeField]
		private RectTransform superHeavyMark;

		// Token: 0x0400180A RID: 6154
		[SerializeField]
		private ToggleAnimation lightMarkToggle;

		// Token: 0x0400180B RID: 6155
		[SerializeField]
		private ToggleAnimation superHeavyMarkToggle;

		// Token: 0x0400180C RID: 6156
		[SerializeField]
		private Color superLightColor;

		// Token: 0x0400180D RID: 6157
		[SerializeField]
		private Color lightColor;

		// Token: 0x0400180E RID: 6158
		[SerializeField]
		private Color superHeavyColor;

		// Token: 0x0400180F RID: 6159
		[SerializeField]
		private Color overweightColor;

		// Token: 0x04001810 RID: 6160
		[SerializeField]
		private float animateDuration = 0.1f;

		// Token: 0x04001811 RID: 6161
		[SerializeField]
		private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001812 RID: 6162
		private float targetRealBarTop;

		// Token: 0x04001813 RID: 6163
		private int currentToken;
	}
}
