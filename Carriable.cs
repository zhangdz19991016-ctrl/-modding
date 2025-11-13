using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200009E RID: 158
public class Carriable : MonoBehaviour
{
	// Token: 0x1700011D RID: 285
	// (get) Token: 0x0600054F RID: 1359 RVA: 0x00017CD1 File Offset: 0x00015ED1
	private Inventory inventory
	{
		get
		{
			if (this.lootbox == null)
			{
				return null;
			}
			return this.lootbox.Inventory;
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00017CEE File Offset: 0x00015EEE
	public float GetWeight()
	{
		if (this.inventory)
		{
			return this.inventory.CachedWeight + this.selfWeight;
		}
		return this.selfWeight;
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x00017D18 File Offset: 0x00015F18
	public void Take(CA_Carry _carrier)
	{
		if (!_carrier)
		{
			return;
		}
		if (this.carrier)
		{
			this.carrier.StopAction();
		}
		this.droping = false;
		this.carrier = _carrier;
		if (this.inventory)
		{
			this.inventory.RecalculateWeight();
		}
		this.rb.transform.SetParent(this.carrier.characterController.modelRoot);
		this.rb.velocity = Vector3.zero;
		this.rb.transform.position = this.carrier.characterController.modelRoot.TransformPoint(this.carrier.carryPoint);
		this.rb.transform.localRotation = Quaternion.identity;
		this.SetRigidbodyActive(false);
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00017DEC File Offset: 0x00015FEC
	private void SetRigidbodyActive(bool active)
	{
		if (active)
		{
			this.rb.isKinematic = false;
			this.rb.interpolation = RigidbodyInterpolation.Interpolate;
			if (this.lootbox && this.lootbox.interactCollider)
			{
				this.lootbox.interactCollider.isTrigger = false;
				return;
			}
		}
		else
		{
			this.rb.isKinematic = true;
			this.rb.interpolation = RigidbodyInterpolation.None;
			if (this.lootbox && this.lootbox.interactCollider)
			{
				this.lootbox.interactCollider.isTrigger = true;
			}
		}
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x00017E90 File Offset: 0x00016090
	public void Drop()
	{
		if (this.carrier.Running)
		{
			this.carrier.StopAction();
		}
		this.carrier = null;
		MultiSceneCore.MoveToActiveWithScene(this.rb.gameObject, SceneManager.GetActiveScene().buildIndex);
		this.DropTask().Forget();
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00017EE8 File Offset: 0x000160E8
	public void OnCarriableUpdate(float deltaTime)
	{
		if (!this.carrier)
		{
			return;
		}
		Vector3 position = this.carrier.characterController.modelRoot.TransformPoint(this.carrier.carryPoint);
		if (this.carrier.characterController.RightHandSocket)
		{
			position.y = this.carrier.characterController.RightHandSocket.transform.position.y + this.carrier.carryPoint.y;
		}
		this.rb.transform.position = position;
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x00017F84 File Offset: 0x00016184
	private UniTaskVoid DropTask()
	{
		Carriable.<DropTask>d__14 <DropTask>d__;
		<DropTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<DropTask>d__.<>4__this = this;
		<DropTask>d__.<>1__state = -1;
		<DropTask>d__.<>t__builder.Start<Carriable.<DropTask>d__14>(ref <DropTask>d__);
		return <DropTask>d__.<>t__builder.Task;
	}

	// Token: 0x040004C0 RID: 1216
	private CA_Carry carrier;

	// Token: 0x040004C1 RID: 1217
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x040004C2 RID: 1218
	[SerializeField]
	private float selfWeight;

	// Token: 0x040004C3 RID: 1219
	public InteractableLootbox lootbox;

	// Token: 0x040004C4 RID: 1220
	private bool droping;

	// Token: 0x040004C5 RID: 1221
	private float startDropTime = -1f;

	// Token: 0x040004C6 RID: 1222
	private bool carring;
}
