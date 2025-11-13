using System;
using Duckov.Utilities;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000420 RID: 1056
	public class SpawnAlertFx : ActionTask<AICharacterController>
	{
		// Token: 0x06002624 RID: 9764 RVA: 0x00083BBD File Offset: 0x00081DBD
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002625 RID: 9765 RVA: 0x00083BC0 File Offset: 0x00081DC0
		protected override string info
		{
			get
			{
				return string.Format("AlertFx", Array.Empty<object>());
			}
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x00083BD4 File Offset: 0x00081DD4
		protected override void OnExecute()
		{
			if (!base.agent || !base.agent.CharacterMainControl)
			{
				base.EndAction(true);
			}
			Transform rightHandSocket = base.agent.CharacterMainControl.RightHandSocket;
			if (!rightHandSocket)
			{
				base.EndAction(true);
			}
			UnityEngine.Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.AlertFxPrefab, rightHandSocket).transform.localPosition = Vector3.zero;
			if (this.alertTime.value <= 0f)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x00083C5F File Offset: 0x00081E5F
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.alertTime.value)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x00083C7B File Offset: 0x00081E7B
		protected override void OnStop()
		{
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x00083C7D File Offset: 0x00081E7D
		protected override void OnPause()
		{
		}

		// Token: 0x040019EA RID: 6634
		public BBParameter<float> alertTime = 0.2f;
	}
}
