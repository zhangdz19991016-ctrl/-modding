using System;
using System.Collections.Generic;
using Duckov.MiniMaps;
using Duckov.Scenes;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000341 RID: 833
	public class MapElementForTask : MonoBehaviour
	{
		// Token: 0x06001CD6 RID: 7382 RVA: 0x0006846D File Offset: 0x0006666D
		public void SetVisibility(bool _visable)
		{
			if (this.visable == _visable)
			{
				return;
			}
			this.visable = _visable;
			if (MultiSceneCore.Instance == null)
			{
				LevelManager.OnLevelInitialized += this.OnLevelInitialized;
				return;
			}
			this.SyncVisibility();
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000684A5 File Offset: 0x000666A5
		private void OnLevelInitialized()
		{
			this.SyncVisibility();
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000684AD File Offset: 0x000666AD
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000684C0 File Offset: 0x000666C0
		private void OnDisable()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000684D3 File Offset: 0x000666D3
		private void SyncVisibility()
		{
			if (this.visable)
			{
				if (this.pointsInstance != null && this.pointsInstance.Count > 0)
				{
					this.DespawnAll();
				}
				this.Spawn();
				return;
			}
			this.DespawnAll();
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x00068508 File Offset: 0x00066708
		private void Spawn()
		{
			foreach (MultiSceneLocation location in this.locations)
			{
				this.SpawnOnePoint(location, this.name);
			}
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x00068564 File Offset: 0x00066764
		private void SpawnOnePoint(MultiSceneLocation _location, string name)
		{
			if (this.pointsInstance == null)
			{
				this.pointsInstance = new List<SimplePointOfInterest>();
			}
			if (MultiSceneCore.Instance == null)
			{
				return;
			}
			Vector3 vector;
			if (!_location.TryGetLocationPosition(out vector))
			{
				return;
			}
			SimplePointOfInterest simplePointOfInterest = new GameObject("MapElement:" + name).AddComponent<SimplePointOfInterest>();
			Debug.Log("Spawning " + simplePointOfInterest.name + " for task", this);
			simplePointOfInterest.Color = this.iconColor;
			simplePointOfInterest.ShadowColor = this.shadowColor;
			simplePointOfInterest.ShadowDistance = this.shadowDistance;
			if (this.range > 0f)
			{
				simplePointOfInterest.IsArea = true;
				simplePointOfInterest.AreaRadius = this.range;
			}
			simplePointOfInterest.Setup(this.icon, name, false, null);
			simplePointOfInterest.SetupMultiSceneLocation(_location, true);
			this.pointsInstance.Add(simplePointOfInterest);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x00068638 File Offset: 0x00066838
		public void DespawnAll()
		{
			if (this.pointsInstance == null || this.pointsInstance.Count == 0)
			{
				return;
			}
			foreach (SimplePointOfInterest simplePointOfInterest in this.pointsInstance)
			{
				UnityEngine.Object.Destroy(simplePointOfInterest.gameObject);
			}
			this.pointsInstance.Clear();
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000686B0 File Offset: 0x000668B0
		public void DespawnPoint(SimplePointOfInterest point)
		{
			if (this.pointsInstance != null && this.pointsInstance.Contains(point))
			{
				this.pointsInstance.Remove(point);
			}
			UnityEngine.Object.Destroy(point.gameObject);
		}

		// Token: 0x0400140B RID: 5131
		private bool visable;

		// Token: 0x0400140C RID: 5132
		public new string name;

		// Token: 0x0400140D RID: 5133
		public List<MultiSceneLocation> locations;

		// Token: 0x0400140E RID: 5134
		public float range;

		// Token: 0x0400140F RID: 5135
		private List<SimplePointOfInterest> pointsInstance;

		// Token: 0x04001410 RID: 5136
		public Sprite icon;

		// Token: 0x04001411 RID: 5137
		public Color iconColor = Color.white;

		// Token: 0x04001412 RID: 5138
		public Color shadowColor = Color.white;

		// Token: 0x04001413 RID: 5139
		public float shadowDistance;
	}
}
