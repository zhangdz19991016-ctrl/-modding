using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000320 RID: 800
	public class BuildingContextMenu : MonoBehaviour
	{
		// Token: 0x06001ABE RID: 6846 RVA: 0x000613B1 File Offset: 0x0005F5B1
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.recycleButton.onPointerClick += this.OnRecycleButtonClicked;
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x000613DB File Offset: 0x0005F5DB
		private void OnRecycleButtonClicked(BuildingContextMenuEntry entry)
		{
			if (this.Target == null)
			{
				return;
			}
			BuildingManager.ReturnBuilding(this.Target.GUID, null).Forget<bool>();
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x00061402 File Offset: 0x0005F602
		// (set) Token: 0x06001AC1 RID: 6849 RVA: 0x0006140A File Offset: 0x0005F60A
		public Building Target { get; private set; }

		// Token: 0x06001AC2 RID: 6850 RVA: 0x00061413 File Offset: 0x0005F613
		public void Setup(Building target)
		{
			this.Target = target;
			if (target == null)
			{
				this.Hide();
				return;
			}
			this.nameText.text = target.DisplayName;
			this.Show();
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x00061444 File Offset: 0x0005F644
		private void LateUpdate()
		{
			if (this.Target == null)
			{
				this.Hide();
				return;
			}
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(GameCamera.Instance.renderCamera, this.Target.transform.position);
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, screenPoint, null, out v);
			this.rectTransform.localPosition = v;
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x000614B1 File Offset: 0x0005F6B1
		private void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x000614BF File Offset: 0x0005F6BF
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400132A RID: 4906
		private RectTransform rectTransform;

		// Token: 0x0400132B RID: 4907
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x0400132C RID: 4908
		[SerializeField]
		private BuildingContextMenuEntry recycleButton;
	}
}
