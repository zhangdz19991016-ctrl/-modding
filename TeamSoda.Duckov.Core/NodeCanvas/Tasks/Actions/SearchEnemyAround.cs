using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200041B RID: 1051
	public class SearchEnemyAround : ActionTask<AICharacterController>
	{
		// Token: 0x06002602 RID: 9730 RVA: 0x000835FF File Offset: 0x000817FF
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x00083604 File Offset: 0x00081804
		protected override void OnExecute()
		{
			DamageInfo damageInfo = default(DamageInfo);
			this.isHurtSearch = false;
			if (base.agent.IsHurt(1.5f, 1, ref damageInfo) && damageInfo.fromCharacter && damageInfo.fromCharacter.mainDamageReceiver)
			{
				this.isHurtSearch = true;
			}
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x0008365C File Offset: 0x0008185C
		private void Search()
		{
			this.waitingSearchResult = true;
			float num = this.useSight ? base.agent.sightAngle : this.searchAngle.value;
			float num2 = this.useSight ? (base.agent.sightDistance * this.sightDistanceMultiplier.value) : this.searchDistance.value;
			if (this.isHurtSearch)
			{
				num2 *= 2f;
			}
			if (this.affactByNightVisionAbility && base.agent.CharacterMainControl)
			{
				float nightVisionAbility = base.agent.CharacterMainControl.NightVisionAbility;
				num *= Mathf.Lerp(TimeOfDayController.NightViewAngleFactor, 1f, nightVisionAbility);
			}
			bool flag = this.useSight || this.checkObsticle;
			this.searchStartTimeMarker = Time.time;
			bool thermalOn = base.agent.CharacterMainControl.ThermalOn;
			LevelManager.Instance.AIMainBrain.AddSearchTask(base.agent.transform.position + Vector3.up * 1.5f, base.agent.CharacterMainControl.CurrentAimDirection, num, num2, base.agent.CharacterMainControl.Team, flag, thermalOn, this.isHurtSearch, this.searchPickup ? base.agent.wantItem : -1, new Action<DamageReceiver, InteractablePickup>(this.OnSearchFinished));
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000837BC File Offset: 0x000819BC
		private void OnSearchFinished(DamageReceiver dmgReceiver, InteractablePickup pickup)
		{
			if (base.agent.gameObject == null)
			{
				return;
			}
			float time = Time.time;
			float num = this.searchStartTimeMarker;
			if (dmgReceiver != null)
			{
				this.result.value = dmgReceiver;
			}
			else if (this.setNullIfNotFound)
			{
				this.result.value = null;
			}
			if (pickup != null)
			{
				this.pickupResult.value = pickup;
			}
			this.waitingSearchResult = false;
			if (base.isRunning)
			{
				base.EndAction(this.alwaysSuccess || this.result.value != null || this.pickupResult != null);
			}
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x00083869 File Offset: 0x00081A69
		protected override void OnUpdate()
		{
			if (!this.waitingSearchResult)
			{
				this.Search();
			}
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x00083879 File Offset: 0x00081A79
		protected override void OnStop()
		{
			this.waitingSearchResult = false;
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x00083882 File Offset: 0x00081A82
		protected override void OnPause()
		{
		}

		// Token: 0x040019D2 RID: 6610
		public bool useSight;

		// Token: 0x040019D3 RID: 6611
		public bool affactByNightVisionAbility;

		// Token: 0x040019D4 RID: 6612
		[ShowIf("useSight", 0)]
		public BBParameter<float> searchAngle = 180f;

		// Token: 0x040019D5 RID: 6613
		[ShowIf("useSight", 0)]
		public BBParameter<float> searchDistance;

		// Token: 0x040019D6 RID: 6614
		[ShowIf("useSight", 1)]
		public BBParameter<float> sightDistanceMultiplier = 1f;

		// Token: 0x040019D7 RID: 6615
		[ShowIf("useSight", 0)]
		public bool checkObsticle = true;

		// Token: 0x040019D8 RID: 6616
		public BBParameter<DamageReceiver> result;

		// Token: 0x040019D9 RID: 6617
		public BBParameter<InteractablePickup> pickupResult;

		// Token: 0x040019DA RID: 6618
		public bool searchPickup;

		// Token: 0x040019DB RID: 6619
		public bool alwaysSuccess;

		// Token: 0x040019DC RID: 6620
		public bool setNullIfNotFound;

		// Token: 0x040019DD RID: 6621
		private bool waitingSearchResult;

		// Token: 0x040019DE RID: 6622
		private float searchStartTimeMarker;

		// Token: 0x040019DF RID: 6623
		private bool isHurtSearch;
	}
}
