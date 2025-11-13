using System;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x02000280 RID: 640
	public class PointOfInterestEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0004C176 File Offset: 0x0004A376
		public MonoBehaviour Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0004C180 File Offset: 0x0004A380
		internal void Setup(MiniMapDisplay master, MonoBehaviour target, MiniMapDisplayEntry minimapEntry)
		{
			this.rectTransform = (base.transform as RectTransform);
			this.master = master;
			this.target = target;
			this.minimapEntry = minimapEntry;
			this.pointOfInterest = null;
			this.icon.sprite = this.defaultIcon;
			this.icon.color = this.defaultColor;
			this.areaDisplay.color = this.defaultColor;
			Color color = this.defaultColor;
			color.a *= 0.1f;
			this.areaFill.color = color;
			this.caption = target.name;
			this.icon.gameObject.SetActive(true);
			IPointOfInterest pointOfInterest = target as IPointOfInterest;
			if (pointOfInterest == null)
			{
				return;
			}
			this.pointOfInterest = pointOfInterest;
			this.icon.gameObject.SetActive(!this.pointOfInterest.HideIcon);
			this.icon.sprite = ((pointOfInterest.Icon != null) ? pointOfInterest.Icon : this.defaultIcon);
			this.icon.color = pointOfInterest.Color;
			if (this.shadow)
			{
				this.shadow.Color = pointOfInterest.ShadowColor;
				this.shadow.OffsetDistance = pointOfInterest.ShadowDistance;
			}
			string value = this.pointOfInterest.DisplayName;
			this.caption = pointOfInterest.DisplayName;
			if (string.IsNullOrEmpty(value))
			{
				this.displayName.gameObject.SetActive(false);
			}
			else
			{
				this.displayName.gameObject.SetActive(true);
				this.displayName.text = this.pointOfInterest.DisplayName;
			}
			if (pointOfInterest.IsArea)
			{
				this.areaDisplay.gameObject.SetActive(true);
				this.rectTransform.sizeDelta = this.pointOfInterest.AreaRadius * Vector2.one * 2f;
				this.areaDisplay.color = pointOfInterest.Color;
				color = pointOfInterest.Color;
				color.a *= 0.1f;
				this.areaFill.color = color;
				this.areaDisplay.BorderWidth = this.areaLineThickness / this.ParentLocalScale;
			}
			else
			{
				this.icon.enabled = true;
				this.areaDisplay.gameObject.SetActive(false);
			}
			this.RefreshPosition();
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0004C3E8 File Offset: 0x0004A5E8
		private void RefreshPosition()
		{
			this.cachedWorldPosition = this.target.transform.position;
			Vector3 centerOfObjectScene = MiniMapCenter.GetCenterOfObjectScene(this.target);
			Vector3 vector = this.target.transform.position - centerOfObjectScene;
			Vector3 point = new Vector2(vector.x, vector.z);
			Vector3 position = this.minimapEntry.transform.localToWorldMatrix.MultiplyPoint(point);
			base.transform.position = position;
			this.UpdateScale();
			this.UpdateRotation();
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x0600147D RID: 5245 RVA: 0x0004C478 File Offset: 0x0004A678
		private float ParentLocalScale
		{
			get
			{
				return base.transform.parent.localScale.x;
			}
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0004C48F File Offset: 0x0004A68F
		private void Update()
		{
			this.UpdateScale();
			this.UpdatePosition();
			this.UpdateRotation();
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0004C4A4 File Offset: 0x0004A6A4
		private void UpdateScale()
		{
			float d = (this.pointOfInterest != null) ? this.pointOfInterest.ScaleFactor : 1f;
			this.iconContainer.localScale = Vector3.one * d / this.ParentLocalScale;
			if (this.pointOfInterest != null && this.pointOfInterest.IsArea)
			{
				this.areaDisplay.BorderWidth = this.areaLineThickness / this.ParentLocalScale;
				this.areaDisplay.FalloffDistance = 1f / this.ParentLocalScale;
			}
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0004C531 File Offset: 0x0004A731
		private void UpdatePosition()
		{
			if (this.cachedWorldPosition != this.target.transform.position)
			{
				this.RefreshPosition();
			}
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0004C556 File Offset: 0x0004A756
		private void UpdateRotation()
		{
			base.transform.rotation = Quaternion.identity;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0004C568 File Offset: 0x0004A768
		public void OnPointerClick(PointerEventData eventData)
		{
			this.pointOfInterest.NotifyClicked(eventData);
			if (CheatMode.Active && UIInputManager.Ctrl && UIInputManager.Alt && UIInputManager.Shift)
			{
				if (MiniMapCenter.GetSceneID(this.target) == null)
				{
					return;
				}
				CharacterMainControl.Main.SetPosition(this.target.transform.position);
			}
		}

		// Token: 0x04000EF5 RID: 3829
		private RectTransform rectTransform;

		// Token: 0x04000EF6 RID: 3830
		private MiniMapDisplay master;

		// Token: 0x04000EF7 RID: 3831
		private MonoBehaviour target;

		// Token: 0x04000EF8 RID: 3832
		private IPointOfInterest pointOfInterest;

		// Token: 0x04000EF9 RID: 3833
		private MiniMapDisplayEntry minimapEntry;

		// Token: 0x04000EFA RID: 3834
		[SerializeField]
		private Transform iconContainer;

		// Token: 0x04000EFB RID: 3835
		[SerializeField]
		private Sprite defaultIcon;

		// Token: 0x04000EFC RID: 3836
		[SerializeField]
		private Color defaultColor = Color.white;

		// Token: 0x04000EFD RID: 3837
		[SerializeField]
		private Image icon;

		// Token: 0x04000EFE RID: 3838
		[SerializeField]
		private TrueShadow shadow;

		// Token: 0x04000EFF RID: 3839
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04000F00 RID: 3840
		[SerializeField]
		private ProceduralImage areaDisplay;

		// Token: 0x04000F01 RID: 3841
		[SerializeField]
		private Image areaFill;

		// Token: 0x04000F02 RID: 3842
		[SerializeField]
		private float areaLineThickness = 1f;

		// Token: 0x04000F03 RID: 3843
		[SerializeField]
		private string caption;

		// Token: 0x04000F04 RID: 3844
		private Vector3 cachedWorldPosition = Vector3.zero;
	}
}
