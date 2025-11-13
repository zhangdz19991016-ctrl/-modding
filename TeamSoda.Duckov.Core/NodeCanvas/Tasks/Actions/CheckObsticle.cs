using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000411 RID: 1041
	public class CheckObsticle : ActionTask<AICharacterController>
	{
		// Token: 0x060025C3 RID: 9667 RVA: 0x000827A0 File Offset: 0x000809A0
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000827A4 File Offset: 0x000809A4
		protected override void OnExecute()
		{
			this.isHurtSearch = false;
			DamageInfo damageInfo = default(DamageInfo);
			if (base.agent.IsHurt(1.5f, 1, ref damageInfo) && damageInfo.fromCharacter && damageInfo.fromCharacter.mainDamageReceiver)
			{
				this.isHurtSearch = true;
			}
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000827FC File Offset: 0x000809FC
		private void Check()
		{
			this.waitingResult = true;
			Vector3 vector = this.useTransform ? this.targetTransform.value.position : this.targetPoint.value;
			vector += Vector3.up * 0.4f;
			Vector3 start = base.agent.transform.position + Vector3.up * 0.4f;
			ItemAgent_Gun gun = base.agent.CharacterMainControl.GetGun();
			if (gun && gun.muzzle)
			{
				start = gun.muzzle.position - gun.muzzle.forward * 0.1f;
			}
			LevelManager.Instance.AIMainBrain.AddCheckObsticleTask(start, vector, base.agent.CharacterMainControl.ThermalOn, this.isHurtSearch, new Action<bool>(this.OnCheckFinished));
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000828F0 File Offset: 0x00080AF0
		private void OnCheckFinished(bool result)
		{
			if (base.agent.gameObject == null)
			{
				return;
			}
			base.agent.hasObsticleToTarget = result;
			this.waitingResult = false;
			if (base.isRunning)
			{
				base.EndAction(this.alwaysSuccess || result);
			}
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x0008293E File Offset: 0x00080B3E
		protected override void OnUpdate()
		{
			if (!this.waitingResult)
			{
				this.Check();
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x0008294E File Offset: 0x00080B4E
		protected override void OnStop()
		{
			this.waitingResult = false;
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00082957 File Offset: 0x00080B57
		protected override void OnPause()
		{
		}

		// Token: 0x040019A8 RID: 6568
		public bool useTransform;

		// Token: 0x040019A9 RID: 6569
		[ShowIf("useTransform", 1)]
		public BBParameter<Transform> targetTransform;

		// Token: 0x040019AA RID: 6570
		[ShowIf("useTransform", 0)]
		public BBParameter<Vector3> targetPoint;

		// Token: 0x040019AB RID: 6571
		public bool alwaysSuccess;

		// Token: 0x040019AC RID: 6572
		private bool waitingResult;

		// Token: 0x040019AD RID: 6573
		private bool isHurtSearch;
	}
}
