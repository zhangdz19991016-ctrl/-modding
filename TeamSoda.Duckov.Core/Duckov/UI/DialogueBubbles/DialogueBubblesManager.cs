using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI.DialogueBubbles
{
	// Token: 0x020003EE RID: 1006
	public class DialogueBubblesManager : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002484 RID: 9348 RVA: 0x0007F89F File Offset: 0x0007DA9F
		// (set) Token: 0x06002485 RID: 9349 RVA: 0x0007F8A6 File Offset: 0x0007DAA6
		public static DialogueBubblesManager Instance { get; private set; }

		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x06002486 RID: 9350 RVA: 0x0007F8B0 File Offset: 0x0007DAB0
		// (remove) Token: 0x06002487 RID: 9351 RVA: 0x0007F8E4 File Offset: 0x0007DAE4
		public static event Action<PointerEventData> onPointerClick;

		// Token: 0x06002488 RID: 9352 RVA: 0x0007F917 File Offset: 0x0007DB17
		private void Awake()
		{
			if (DialogueBubblesManager.Instance == null)
			{
				DialogueBubblesManager.Instance = this;
			}
			this.prefab.gameObject.SetActive(false);
			this.raycastReceiver.enabled = false;
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x0007F94C File Offset: 0x0007DB4C
		public static UniTask Show(string text, Transform target, float yOffset = -1f, bool needInteraction = false, bool skippable = false, float speed = -1f, float duration = 2f)
		{
			DialogueBubblesManager.<Show>d__11 <Show>d__;
			<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Show>d__.text = text;
			<Show>d__.target = target;
			<Show>d__.yOffset = yOffset;
			<Show>d__.needInteraction = needInteraction;
			<Show>d__.skippable = skippable;
			<Show>d__.speed = speed;
			<Show>d__.duration = duration;
			<Show>d__.<>1__state = -1;
			<Show>d__.<>t__builder.Start<DialogueBubblesManager.<Show>d__11>(ref <Show>d__);
			return <Show>d__.<>t__builder.Task;
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x0007F9C2 File Offset: 0x0007DBC2
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<PointerEventData> action = DialogueBubblesManager.onPointerClick;
			if (action == null)
			{
				return;
			}
			action(eventData);
		}

		// Token: 0x040018C6 RID: 6342
		[SerializeField]
		private DialogueBubble prefab;

		// Token: 0x040018C7 RID: 6343
		[SerializeField]
		private Graphic raycastReceiver;

		// Token: 0x040018C8 RID: 6344
		private List<DialogueBubble> bubbles = new List<DialogueBubble>();
	}
}
