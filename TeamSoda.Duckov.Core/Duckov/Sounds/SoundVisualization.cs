using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Sounds
{
	// Token: 0x0200024D RID: 589
	public class SoundVisualization : MonoBehaviour
	{
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x00046818 File Offset: 0x00044A18
		private PrefabPool<SoundDisplay> DisplayPool
		{
			get
			{
				if (this._displayPool == null)
				{
					this._displayPool = new PrefabPool<SoundDisplay>(this.displayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._displayPool;
			}
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00046851 File Offset: 0x00044A51
		private void Awake()
		{
			AIMainBrain.OnPlayerHearSound += this.OnHeardSound;
			if (this.layoutCenter == null)
			{
				this.layoutCenter = (base.transform as RectTransform);
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00046883 File Offset: 0x00044A83
		private void OnDestroy()
		{
			AIMainBrain.OnPlayerHearSound -= this.OnHeardSound;
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00046898 File Offset: 0x00044A98
		private void Update()
		{
			using (IEnumerator<SoundDisplay> enumerator = this.DisplayPool.ActiveEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SoundDisplay soundDisplay = enumerator.Current;
					if (soundDisplay.Value <= 0f)
					{
						this.releaseBuffer.Enqueue(soundDisplay);
					}
					else
					{
						this.RefreshEntryPosition(soundDisplay);
					}
				}
				goto IL_71;
			}
			IL_50:
			SoundDisplay soundDisplay2 = this.releaseBuffer.Dequeue();
			if (!(soundDisplay2 == null))
			{
				this.DisplayPool.Release(soundDisplay2);
			}
			IL_71:
			if (this.releaseBuffer.Count <= 0)
			{
				return;
			}
			goto IL_50;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00046934 File Offset: 0x00044B34
		private void OnHeardSound(AISound sound)
		{
			this.Trigger(sound);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00046940 File Offset: 0x00044B40
		private void Trigger(AISound sound)
		{
			if (GameCamera.Instance == null)
			{
				return;
			}
			SoundDisplay soundDisplay = null;
			if (sound.fromCharacter != null)
			{
				foreach (SoundDisplay soundDisplay2 in this.DisplayPool.ActiveEntries)
				{
					AISound currentSount = soundDisplay2.CurrentSount;
					if (!(currentSount.fromCharacter != sound.fromCharacter) && currentSount.soundType == sound.soundType && Vector3.Distance(currentSount.pos, sound.pos) < this.retriggerDistanceThreshold)
					{
						soundDisplay = soundDisplay2;
					}
				}
			}
			if (soundDisplay == null)
			{
				soundDisplay = this.DisplayPool.Get(null);
			}
			this.RefreshEntryPosition(soundDisplay);
			soundDisplay.Trigger(sound);
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00046A10 File Offset: 0x00044C10
		private void RefreshEntryPosition(SoundDisplay e)
		{
			Vector3 pos = e.CurrentSount.pos;
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(GameCamera.Instance.renderCamera, pos);
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.layoutCenter, screenPoint, null, out vector);
			Vector2 normalized = vector.normalized;
			e.transform.localPosition = normalized * this.displayOffset;
			e.transform.rotation = Quaternion.FromToRotation(Vector2.up, normalized);
		}

		// Token: 0x04000E22 RID: 3618
		[SerializeField]
		private RectTransform layoutCenter;

		// Token: 0x04000E23 RID: 3619
		[SerializeField]
		private SoundDisplay displayTemplate;

		// Token: 0x04000E24 RID: 3620
		[SerializeField]
		private float retriggerDistanceThreshold = 1f;

		// Token: 0x04000E25 RID: 3621
		[SerializeField]
		private float displayOffset = 400f;

		// Token: 0x04000E26 RID: 3622
		private PrefabPool<SoundDisplay> _displayPool;

		// Token: 0x04000E27 RID: 3623
		private Queue<SoundDisplay> releaseBuffer = new Queue<SoundDisplay>();
	}
}
