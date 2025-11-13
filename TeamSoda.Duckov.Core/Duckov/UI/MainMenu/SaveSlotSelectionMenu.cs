using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.UI.MainMenu
{
	// Token: 0x020003F5 RID: 1013
	public class SaveSlotSelectionMenu : MonoBehaviour
	{
		// Token: 0x060024B9 RID: 9401 RVA: 0x000801B9 File Offset: 0x0007E3B9
		private void OnEnable()
		{
			UIInputManager.OnCancel += this.OnCancel;
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000801CC File Offset: 0x0007E3CC
		private void OnDisable()
		{
			UIInputManager.OnCancel -= this.OnCancel;
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000801DF File Offset: 0x0007E3DF
		private void OnCancel(UIInputEventData data)
		{
			data.Use();
			this.Finish();
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000801F0 File Offset: 0x0007E3F0
		internal UniTask Execute()
		{
			SaveSlotSelectionMenu.<Execute>d__6 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<SaveSlotSelectionMenu.<Execute>d__6>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x00080233 File Offset: 0x0007E433
		public void Finish()
		{
			this.finished = true;
		}

		// Token: 0x040018F3 RID: 6387
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040018F4 RID: 6388
		[SerializeField]
		private GameObject oldSaveIndicator;

		// Token: 0x040018F5 RID: 6389
		internal bool finished;
	}
}
