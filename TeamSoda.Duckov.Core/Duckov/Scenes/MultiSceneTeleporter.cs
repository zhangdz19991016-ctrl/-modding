using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.Scenes
{
	// Token: 0x02000333 RID: 819
	public class MultiSceneTeleporter : InteractableBase
	{
		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001BEB RID: 7147 RVA: 0x00065CC5 File Offset: 0x00063EC5
		protected override bool ShowUnityEvents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001BEC RID: 7148 RVA: 0x00065CC8 File Offset: 0x00063EC8
		[SerializeField]
		public MultiSceneLocation Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x00065CD0 File Offset: 0x00063ED0
		private static float TimeSinceTeleportFinished
		{
			get
			{
				return Time.time - MultiSceneTeleporter.timeWhenTeleportFinished;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001BEE RID: 7150 RVA: 0x00065CDD File Offset: 0x00063EDD
		private static bool Teleportable
		{
			get
			{
				return MultiSceneTeleporter.TimeSinceTeleportFinished > 1f;
			}
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x00065CEB File Offset: 0x00063EEB
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x00065CF4 File Offset: 0x00063EF4
		private void OnDrawGizmosSelected()
		{
			Transform locationTransform = this.target.LocationTransform;
			if (locationTransform)
			{
				Gizmos.DrawLine(base.transform.position, locationTransform.position);
				Gizmos.DrawWireSphere(locationTransform.position, 0.25f);
			}
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x00065D3B File Offset: 0x00063F3B
		public void DoTeleport()
		{
			if (!MultiSceneTeleporter.Teleportable)
			{
				Debug.Log("not Teleportable");
				return;
			}
			this.TeleportTask().Forget();
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x00065D5A File Offset: 0x00063F5A
		protected override bool IsInteractable()
		{
			return MultiSceneTeleporter.Teleportable;
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x00065D64 File Offset: 0x00063F64
		private UniTask TeleportTask()
		{
			MultiSceneTeleporter.<TeleportTask>d__16 <TeleportTask>d__;
			<TeleportTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<TeleportTask>d__.<>4__this = this;
			<TeleportTask>d__.<>1__state = -1;
			<TeleportTask>d__.<>t__builder.Start<MultiSceneTeleporter.<TeleportTask>d__16>(ref <TeleportTask>d__);
			return <TeleportTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x00065DA7 File Offset: 0x00063FA7
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			this.coolTime = 2f;
			this.finishWhenTimeOut = true;
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x00065DBB File Offset: 0x00063FBB
		protected override void OnInteractFinished()
		{
			this.DoTeleport();
			base.StopInteract();
		}

		// Token: 0x040013C1 RID: 5057
		[SerializeField]
		private MultiSceneLocation target;

		// Token: 0x040013C2 RID: 5058
		[SerializeField]
		private bool teleportOnTriggerEnter;

		// Token: 0x040013C3 RID: 5059
		private const float CoolDown = 1f;

		// Token: 0x040013C4 RID: 5060
		private static float timeWhenTeleportFinished;
	}
}
