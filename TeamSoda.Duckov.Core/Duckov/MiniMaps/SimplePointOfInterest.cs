using System;
using Duckov.Scenes;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Duckov.MiniMaps
{
	// Token: 0x0200027B RID: 635
	public class SimplePointOfInterest : MonoBehaviour, IPointOfInterest
	{
		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06001420 RID: 5152 RVA: 0x0004AF24 File Offset: 0x00049124
		// (remove) Token: 0x06001421 RID: 5153 RVA: 0x0004AF5C File Offset: 0x0004915C
		public event Action<PointerEventData> OnClicked;

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0004AF91 File Offset: 0x00049191
		// (set) Token: 0x06001423 RID: 5155 RVA: 0x0004AF99 File Offset: 0x00049199
		public float ScaleFactor
		{
			get
			{
				return this.scaleFactor;
			}
			set
			{
				this.scaleFactor = value;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x0004AFA2 File Offset: 0x000491A2
		// (set) Token: 0x06001425 RID: 5157 RVA: 0x0004AFAA File Offset: 0x000491AA
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x0004AFB3 File Offset: 0x000491B3
		// (set) Token: 0x06001427 RID: 5159 RVA: 0x0004AFBB File Offset: 0x000491BB
		public Color ShadowColor
		{
			get
			{
				return this.shadowColor;
			}
			set
			{
				this.shadowColor = value;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0004AFC4 File Offset: 0x000491C4
		// (set) Token: 0x06001429 RID: 5161 RVA: 0x0004AFCC File Offset: 0x000491CC
		public float ShadowDistance
		{
			get
			{
				return this.shadowDistance;
			}
			set
			{
				this.shadowDistance = value;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x0004AFD5 File Offset: 0x000491D5
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x0004AFE2 File Offset: 0x000491E2
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x0004AFEC File Offset: 0x000491EC
		public int OverrideScene
		{
			get
			{
				if (this.followActiveScene && MultiSceneCore.ActiveSubScene != null)
				{
					return MultiSceneCore.ActiveSubScene.Value.buildIndex;
				}
				if (!string.IsNullOrEmpty(this.overrideSceneID))
				{
					return SceneInfoCollection.GetBuildIndex(this.overrideSceneID);
				}
				return -1;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0004B040 File Offset: 0x00049240
		// (set) Token: 0x0600142E RID: 5166 RVA: 0x0004B048 File Offset: 0x00049248
		public bool IsArea
		{
			get
			{
				return this.isArea;
			}
			set
			{
				this.isArea = value;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x0004B051 File Offset: 0x00049251
		// (set) Token: 0x06001430 RID: 5168 RVA: 0x0004B059 File Offset: 0x00049259
		public float AreaRadius
		{
			get
			{
				return this.areaRadius;
			}
			set
			{
				this.areaRadius = value;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x0004B062 File Offset: 0x00049262
		// (set) Token: 0x06001432 RID: 5170 RVA: 0x0004B06A File Offset: 0x0004926A
		public bool HideIcon
		{
			get
			{
				return this.hideIcon;
			}
			set
			{
				this.hideIcon = value;
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0004B073 File Offset: 0x00049273
		private void OnEnable()
		{
			PointsOfInterests.Register(this);
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0004B07B File Offset: 0x0004927B
		private void OnDisable()
		{
			PointsOfInterests.Unregister(this);
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0004B083 File Offset: 0x00049283
		public void Setup(Sprite icon = null, string displayName = null, bool followActiveScene = false, string overrideSceneID = null)
		{
			if (icon != null)
			{
				this.icon = icon;
			}
			this.displayName = displayName;
			this.followActiveScene = followActiveScene;
			this.overrideSceneID = overrideSceneID;
			PointsOfInterests.Unregister(this);
			PointsOfInterests.Register(this);
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0004B0B7 File Offset: 0x000492B7
		public void SetColor(Color color)
		{
			this.color = color;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0004B0C0 File Offset: 0x000492C0
		public bool SetupMultiSceneLocation(MultiSceneLocation location, bool moveToMainScene = true)
		{
			Vector3 position;
			if (!location.TryGetLocationPosition(out position))
			{
				return false;
			}
			base.transform.position = position;
			this.overrideSceneID = location.SceneID;
			if (moveToMainScene && MultiSceneCore.MainScene != null)
			{
				SceneManager.MoveGameObjectToScene(base.gameObject, MultiSceneCore.MainScene.Value);
			}
			return true;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0004B120 File Offset: 0x00049320
		public static SimplePointOfInterest Create(Vector3 position, string sceneID, string displayName, Sprite icon = null, bool hideIcon = false)
		{
			GameObject gameObject = new GameObject("POI_" + displayName);
			gameObject.transform.position = position;
			SimplePointOfInterest simplePointOfInterest = gameObject.AddComponent<SimplePointOfInterest>();
			simplePointOfInterest.overrideSceneID = sceneID;
			simplePointOfInterest.displayName = displayName;
			simplePointOfInterest.hideIcon = hideIcon;
			simplePointOfInterest.icon = icon;
			SceneManager.MoveGameObjectToScene(gameObject, MultiSceneCore.MainScene.Value);
			return simplePointOfInterest;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0004B180 File Offset: 0x00049380
		public void NotifyClicked(PointerEventData pointerEventData)
		{
			Action<PointerEventData> onClicked = this.OnClicked;
			if (onClicked == null)
			{
				return;
			}
			onClicked(pointerEventData);
		}

		// Token: 0x04000ECD RID: 3789
		[SerializeField]
		private Sprite icon;

		// Token: 0x04000ECE RID: 3790
		[SerializeField]
		private Color color = Color.white;

		// Token: 0x04000ECF RID: 3791
		[SerializeField]
		private Color shadowColor = Color.white;

		// Token: 0x04000ED0 RID: 3792
		[SerializeField]
		private float shadowDistance;

		// Token: 0x04000ED1 RID: 3793
		[LocalizationKey("Default")]
		[SerializeField]
		private string displayName = "";

		// Token: 0x04000ED2 RID: 3794
		[SerializeField]
		private bool followActiveScene;

		// Token: 0x04000ED3 RID: 3795
		[SceneID]
		[SerializeField]
		private string overrideSceneID;

		// Token: 0x04000ED4 RID: 3796
		[SerializeField]
		private bool isArea;

		// Token: 0x04000ED5 RID: 3797
		[SerializeField]
		private float areaRadius;

		// Token: 0x04000ED6 RID: 3798
		[SerializeField]
		private float scaleFactor = 1f;

		// Token: 0x04000ED8 RID: 3800
		[SerializeField]
		private bool hideIcon;
	}
}
