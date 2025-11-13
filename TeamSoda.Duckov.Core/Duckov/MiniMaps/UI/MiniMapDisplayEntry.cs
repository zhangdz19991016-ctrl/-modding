using System;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x0200027E RID: 638
	public class MiniMapDisplayEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x0004BAA0 File Offset: 0x00049CA0
		public SceneReference SceneReference
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.SceneID);
				if (sceneInfo == null)
				{
					return null;
				}
				return sceneInfo.SceneReference;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0004BAC4 File Offset: 0x00049CC4
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x0004BACC File Offset: 0x00049CCC
		private RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = (base.transform as RectTransform);
				}
				return this._rectTransform;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x0004BAF3 File Offset: 0x00049CF3
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x0004BAFB File Offset: 0x00049CFB
		public MiniMapDisplay Master { get; private set; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x0004BB04 File Offset: 0x00049D04
		public bool Hide
		{
			get
			{
				return this.target != null && this.target.Hide;
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0004BB1B File Offset: 0x00049D1B
		private void Awake()
		{
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0004BB2E File Offset: 0x00049D2E
		private void OnDestroy()
		{
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0004BB41 File Offset: 0x00049D41
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Mapping entries";
			Debug.Log("Mapping entries", this);
			this.RefreshGraphics();
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0004BB5E File Offset: 0x00049D5E
		public bool NoSignal()
		{
			return this.target != null && this.target.NoSignal;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0004BB78 File Offset: 0x00049D78
		internal void Setup(MiniMapDisplay master, IMiniMapEntry cur, bool showGraphics = true)
		{
			this.Master = master;
			this.target = cur;
			if (cur.Sprite != null)
			{
				this.image.sprite = cur.Sprite;
				this.rectTransform.sizeDelta = Vector2.one * (float)cur.Sprite.texture.width * cur.PixelSize;
				this.showGraphics = showGraphics;
			}
			else
			{
				this.showGraphics = false;
			}
			if (cur.Hide)
			{
				this.showGraphics = false;
			}
			this.rectTransform.anchoredPosition = cur.Offset;
			this.sceneID = cur.SceneID;
			this.isCombined = false;
			this.RefreshGraphics();
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0004BC2C File Offset: 0x00049E2C
		internal void SetupCombined(MiniMapDisplay master, IMiniMapDataProvider dataProvider)
		{
			this.target = null;
			this.Master = master;
			if (dataProvider == null)
			{
				return;
			}
			if (dataProvider.CombinedSprite == null)
			{
				return;
			}
			this.image.sprite = dataProvider.CombinedSprite;
			this.rectTransform.sizeDelta = Vector2.one * (float)dataProvider.CombinedSprite.texture.width * dataProvider.PixelSize;
			this.rectTransform.anchoredPosition = dataProvider.CombinedCenter;
			this.sceneID = "";
			this.image.enabled = true;
			this.showGraphics = true;
			this.isCombined = true;
			this.RefreshGraphics();
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004BCE0 File Offset: 0x00049EE0
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Right)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.sceneID))
			{
				return;
			}
			Vector3 vector;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(base.transform as RectTransform, eventData.position, null, out vector);
			Vector3 worldPos;
			if (!this.Master.TryConvertToWorldPosition(eventData.position, out worldPos))
			{
				return;
			}
			MiniMapView.RequestMarkPOI(worldPos);
			eventData.Use();
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0004BD48 File Offset: 0x00049F48
		private void RefreshGraphics()
		{
			bool flag = this.ShouldShow();
			if (flag)
			{
				this.image.color = Color.white;
			}
			else
			{
				this.image.color = Color.clear;
			}
			this.image.enabled = flag;
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0004BD8D File Offset: 0x00049F8D
		public bool ShouldShow()
		{
			if (!this.showGraphics)
			{
				return false;
			}
			if (this.isCombined)
			{
				return this.showGraphics;
			}
			return MultiSceneCore.ActiveSubSceneID == this.SceneID;
		}

		// Token: 0x04000EE2 RID: 3810
		[SerializeField]
		private Image image;

		// Token: 0x04000EE3 RID: 3811
		private string sceneID;

		// Token: 0x04000EE4 RID: 3812
		private RectTransform _rectTransform;

		// Token: 0x04000EE6 RID: 3814
		private bool showGraphics;

		// Token: 0x04000EE7 RID: 3815
		private bool isCombined;

		// Token: 0x04000EE8 RID: 3816
		private IMiniMapEntry target;
	}
}
