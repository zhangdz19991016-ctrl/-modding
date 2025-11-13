using System;
using Duckov.Economy;
using Duckov.Quests;
using Duckov.Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x020003BF RID: 959
	public class MapSelectionEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x0007A74E File Offset: 0x0007894E
		public Cost Cost
		{
			get
			{
				return this.cost;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x0007A756 File Offset: 0x00078956
		public bool ConditionsSatisfied
		{
			get
			{
				return this.conditions == null || this.conditions.Satisfied();
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x0007A76D File Offset: 0x0007896D
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x0007A775 File Offset: 0x00078975
		public int BeaconIndex
		{
			get
			{
				return this.beaconIndex;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x0007A77D File Offset: 0x0007897D
		public Sprite FullScreenImage
		{
			get
			{
				return this.fullScreenImage;
			}
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x0007A785 File Offset: 0x00078985
		public void Setup(MapSelectionView master)
		{
			this.master = master;
			this.Refresh();
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x0007A794 File Offset: 0x00078994
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0007A79C File Offset: 0x0007899C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.ConditionsSatisfied)
			{
				return;
			}
			this.master.NotifyEntryClicked(this, eventData);
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x0007A7B4 File Offset: 0x000789B4
		private void Refresh()
		{
			SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.sceneID);
			this.displayNameText.text = sceneInfo.DisplayName;
			this.lockedIndicator.gameObject.SetActive(!this.ConditionsSatisfied);
			this.costDisplay.Setup(this.cost, 1);
			this.costDisplay.gameObject.SetActive(!this.cost.IsFree);
		}

		// Token: 0x040017A4 RID: 6052
		[SerializeField]
		private MapSelectionView master;

		// Token: 0x040017A5 RID: 6053
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x040017A6 RID: 6054
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x040017A7 RID: 6055
		[SerializeField]
		private GameObject lockedIndicator;

		// Token: 0x040017A8 RID: 6056
		[SerializeField]
		private Condition[] conditions;

		// Token: 0x040017A9 RID: 6057
		[SerializeField]
		private Cost cost;

		// Token: 0x040017AA RID: 6058
		[SerializeField]
		[SceneID]
		private string sceneID;

		// Token: 0x040017AB RID: 6059
		[SerializeField]
		private int beaconIndex;

		// Token: 0x040017AC RID: 6060
		[SerializeField]
		private Sprite fullScreenImage;
	}
}
