using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x02000388 RID: 904
	public class Tooltips : MonoBehaviour
	{
		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x0006E9E2 File Offset: 0x0006CBE2
		// (set) Token: 0x06001F74 RID: 8052 RVA: 0x0006E9E9 File Offset: 0x0006CBE9
		public static ITooltipsProvider CurrentProvider { get; private set; }

		// Token: 0x06001F75 RID: 8053 RVA: 0x0006E9F1 File Offset: 0x0006CBF1
		public static void NotifyEnterTooltipsProvider(ITooltipsProvider provider)
		{
			Tooltips.CurrentProvider = provider;
			Action<ITooltipsProvider> onEnterProvider = Tooltips.OnEnterProvider;
			if (onEnterProvider == null)
			{
				return;
			}
			onEnterProvider(provider);
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0006EA09 File Offset: 0x0006CC09
		public static void NotifyExitTooltipsProvider(ITooltipsProvider provider)
		{
			if (Tooltips.CurrentProvider != provider)
			{
				return;
			}
			Tooltips.CurrentProvider = null;
			Action<ITooltipsProvider> onExitProvider = Tooltips.OnExitProvider;
			if (onExitProvider == null)
			{
				return;
			}
			onExitProvider(provider);
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0006EA2C File Offset: 0x0006CC2C
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			Tooltips.OnEnterProvider = (Action<ITooltipsProvider>)Delegate.Combine(Tooltips.OnEnterProvider, new Action<ITooltipsProvider>(this.DoOnEnterProvider));
			Tooltips.OnExitProvider = (Action<ITooltipsProvider>)Delegate.Combine(Tooltips.OnExitProvider, new Action<ITooltipsProvider>(this.DoOnExitProvider));
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x0006EA94 File Offset: 0x0006CC94
		private void OnDestroy()
		{
			Tooltips.OnEnterProvider = (Action<ITooltipsProvider>)Delegate.Remove(Tooltips.OnEnterProvider, new Action<ITooltipsProvider>(this.DoOnEnterProvider));
			Tooltips.OnExitProvider = (Action<ITooltipsProvider>)Delegate.Remove(Tooltips.OnExitProvider, new Action<ITooltipsProvider>(this.DoOnExitProvider));
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x0006EAE1 File Offset: 0x0006CCE1
		private void Update()
		{
			if (this.contents.gameObject.activeSelf)
			{
				this.RefreshPosition();
			}
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0006EAFB File Offset: 0x0006CCFB
		private void DoOnExitProvider(ITooltipsProvider provider)
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0006EB08 File Offset: 0x0006CD08
		private void DoOnEnterProvider(ITooltipsProvider provider)
		{
			this.text.text = provider.GetTooltipsText();
			this.fadeGroup.Show();
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0006EB28 File Offset: 0x0006CD28
		private unsafe void RefreshPosition()
		{
			Vector2 screenPoint = *Mouse.current.position.value;
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, screenPoint, null, out v);
			this.contents.localPosition = v;
		}

		// Token: 0x04001579 RID: 5497
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x0400157A RID: 5498
		[SerializeField]
		private RectTransform contents;

		// Token: 0x0400157B RID: 5499
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400157C RID: 5500
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x0400157E RID: 5502
		private static Action<ITooltipsProvider> OnEnterProvider;

		// Token: 0x0400157F RID: 5503
		private static Action<ITooltipsProvider> OnExitProvider;
	}
}
