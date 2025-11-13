using System;
using System.Collections.Generic;
using UnityEngine;

namespace FX
{
	// Token: 0x02000210 RID: 528
	public class PopText : MonoBehaviour
	{
		// Token: 0x06000FC3 RID: 4035 RVA: 0x0003E5D9 File Offset: 0x0003C7D9
		private void Awake()
		{
			PopText.instance = this;
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0003E5E4 File Offset: 0x0003C7E4
		private PopTextEntity GetOrCreateEntry()
		{
			PopTextEntity popTextEntity;
			if (this.inactiveEntries.Count > 0)
			{
				popTextEntity = this.inactiveEntries[0];
				this.inactiveEntries.RemoveAt(0);
			}
			popTextEntity = UnityEngine.Object.Instantiate<PopTextEntity>(this.popTextPrefab, base.transform);
			this.activeEntries.Add(popTextEntity);
			popTextEntity.gameObject.SetActive(true);
			return popTextEntity;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0003E644 File Offset: 0x0003C844
		public void InstancePop(string text, Vector3 worldPosition, Color color, float size, Sprite sprite = null)
		{
			PopTextEntity orCreateEntry = this.GetOrCreateEntry();
			orCreateEntry.Color = color;
			orCreateEntry.size = size;
			orCreateEntry.transform.localScale = Vector3.one * size;
			Transform transform = orCreateEntry.transform;
			transform.position = worldPosition;
			transform.rotation = PopText.LookAtMainCamera(worldPosition);
			float x = UnityEngine.Random.Range(-this.randomAngle, this.randomAngle);
			float z = UnityEngine.Random.Range(-this.randomAngle, this.randomAngle);
			Vector3 a = Quaternion.Euler(x, 0f, z) * Vector3.up;
			orCreateEntry.SetupContent(text, sprite);
			orCreateEntry.velocity = a * this.spawnVelocity;
			orCreateEntry.spawnTime = Time.time;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0003E6F8 File Offset: 0x0003C8F8
		private static Quaternion LookAtMainCamera(Vector3 position)
		{
			if (Camera.main)
			{
				Transform transform = Camera.main.transform;
				return Quaternion.LookRotation(-(transform.position - position), transform.up);
			}
			return Quaternion.identity;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0003E73E File Offset: 0x0003C93E
		public void Recycle(PopTextEntity entry)
		{
			entry.gameObject.SetActive(false);
			this.activeEntries.Remove(entry);
			this.inactiveEntries.Add(entry);
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0003E768 File Offset: 0x0003C968
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			Vector3 a = Vector3.up * this.gravityValue;
			bool flag = false;
			foreach (PopTextEntity popTextEntity in this.activeEntries)
			{
				if (popTextEntity == null)
				{
					flag = true;
				}
				else
				{
					Transform transform = popTextEntity.transform;
					transform.position += popTextEntity.velocity * deltaTime;
					transform.rotation = PopText.LookAtMainCamera(transform.position);
					popTextEntity.velocity += a * deltaTime;
					popTextEntity.transform.localScale = this.sizeOverLife.Evaluate(popTextEntity.timeSinceSpawn / this.lifeTime) * popTextEntity.size * Vector3.one;
					float t = Mathf.Clamp01(popTextEntity.timeSinceSpawn / this.lifeTime * 2f - 1f);
					Color color = Color.Lerp(popTextEntity.Color, popTextEntity.EndColor, t);
					popTextEntity.SetColor(color);
					if (popTextEntity.timeSinceSpawn > this.lifeTime)
					{
						this.recycleList.Add(popTextEntity);
					}
				}
			}
			if (this.recycleList.Count > 0)
			{
				foreach (PopTextEntity entry in this.recycleList)
				{
					this.Recycle(entry);
				}
				this.recycleList.Clear();
			}
			if (flag)
			{
				this.activeEntries.RemoveAll((PopTextEntity e) => e == null);
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0003E96C File Offset: 0x0003CB6C
		private void PopTest()
		{
			Vector3 worldPosition = base.transform.position;
			CharacterMainControl main = CharacterMainControl.Main;
			if (main != null)
			{
				worldPosition = main.transform.position + Vector3.up * 2f;
			}
			this.InstancePop("Test", worldPosition, Color.white, 1f, this.debugSprite);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0003E9D0 File Offset: 0x0003CBD0
		public static void Pop(string text, Vector3 worldPosition, Color color, float size, Sprite sprite = null)
		{
			if (DevCam.devCamOn)
			{
				return;
			}
			if (PopText.instance)
			{
				PopText.instance.InstancePop(text, worldPosition, color, size, sprite);
			}
		}

		// Token: 0x04000CC0 RID: 3264
		public static PopText instance;

		// Token: 0x04000CC1 RID: 3265
		public PopTextEntity popTextPrefab;

		// Token: 0x04000CC2 RID: 3266
		public List<PopTextEntity> inactiveEntries;

		// Token: 0x04000CC3 RID: 3267
		public List<PopTextEntity> activeEntries;

		// Token: 0x04000CC4 RID: 3268
		public float spawnVelocity = 5f;

		// Token: 0x04000CC5 RID: 3269
		public float gravityValue = -9.8f;

		// Token: 0x04000CC6 RID: 3270
		public float lifeTime = 1f;

		// Token: 0x04000CC7 RID: 3271
		public AnimationCurve sizeOverLife;

		// Token: 0x04000CC8 RID: 3272
		public float randomAngle = 10f;

		// Token: 0x04000CC9 RID: 3273
		public Sprite debugSprite;

		// Token: 0x04000CCA RID: 3274
		private List<PopTextEntity> recycleList = new List<PopTextEntity>();
	}
}
