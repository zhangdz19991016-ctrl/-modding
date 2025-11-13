using System;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class MoveRing : MonoBehaviour
{
	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x00031E57 File Offset: 0x00030057
	private CharacterMainControl character
	{
		get
		{
			return this.inputManager.characterMainControl;
		}
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00031E64 File Offset: 0x00030064
	public void SetThreshold(float threshold)
	{
		this.runThreshold = threshold;
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00031E70 File Offset: 0x00030070
	public void LateUpdate()
	{
		if (!this.inputManager)
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			this.inputManager = LevelManager.Instance.InputManager;
			return;
		}
		else
		{
			if (!this.character)
			{
				this.SetMove(Vector3.zero, 0f);
				return;
			}
			base.transform.position = this.character.transform.position + Vector3.up * 0.02f;
			this.SetThreshold(this.inputManager.runThreshold);
			this.SetMove(this.inputManager.WorldMoveInput.normalized, this.inputManager.WorldMoveInput.magnitude);
			this.SetRunning(this.character.Running);
			if (this.ring.enabled != this.character.gameObject.activeInHierarchy)
			{
				this.ring.enabled = this.character.gameObject.activeInHierarchy;
			}
			return;
		}
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00031F7C File Offset: 0x0003017C
	public void SetMove(Vector3 direction, float value)
	{
		if (this.ringMat)
		{
			this.ringMat.SetVector("_Direction", direction);
			this.ringMat.SetFloat("_Distance", value);
			this.ringMat.SetFloat("_Threshold", this.runThreshold);
			return;
		}
		if (!this.ring)
		{
			return;
		}
		this.ringMat = this.ring.material;
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00031FF3 File Offset: 0x000301F3
	public void SetRunning(bool running)
	{
		this.ringMat.SetFloat("_Running", (float)(running ? 1 : 0));
	}

	// Token: 0x04000A08 RID: 2568
	public Renderer ring;

	// Token: 0x04000A09 RID: 2569
	public float runThreshold;

	// Token: 0x04000A0A RID: 2570
	private Material ringMat;

	// Token: 0x04000A0B RID: 2571
	private InputManager inputManager;
}
