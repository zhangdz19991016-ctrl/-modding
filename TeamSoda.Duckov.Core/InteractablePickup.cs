using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class InteractablePickup : InteractableBase
{
	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000716 RID: 1814 RVA: 0x000201C5 File Offset: 0x0001E3C5
	public DuckovItemAgent ItemAgent
	{
		get
		{
			return this.itemAgent;
		}
	}

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06000717 RID: 1815 RVA: 0x000201D0 File Offset: 0x0001E3D0
	// (remove) Token: 0x06000718 RID: 1816 RVA: 0x00020204 File Offset: 0x0001E404
	public static event Action<InteractablePickup, CharacterMainControl> OnPickupSuccess;

	// Token: 0x06000719 RID: 1817 RVA: 0x00020237 File Offset: 0x0001E437
	protected override bool IsInteractable()
	{
		return true;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0002023C File Offset: 0x0001E43C
	public void OnInit()
	{
		if (this.itemAgent && this.itemAgent.Item && this.sprite)
		{
			this.sprite.sprite = this.itemAgent.Item.Icon;
		}
		this.overrideInteractName = true;
		base.InteractName = this.itemAgent.Item.DisplayNameRaw;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x000202B0 File Offset: 0x0001E4B0
	protected override void OnInteractStart(CharacterMainControl character)
	{
		bool flag = character.PickupItem(this.itemAgent.Item);
		try
		{
			if (flag)
			{
				Action<InteractablePickup, CharacterMainControl> onPickupSuccess = InteractablePickup.OnPickupSuccess;
				if (onPickupSuccess != null)
				{
					onPickupSuccess(this, character);
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		base.StopInteract();
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00020304 File Offset: 0x0001E504
	public void Throw(Vector3 direction, float randomAngle)
	{
		this.throwStartPoint = base.transform.position;
		if (!this.rb)
		{
			this.rb = base.gameObject.AddComponent<Rigidbody>();
		}
		this.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
		this.rb.constraints = RigidbodyConstraints.FreezeRotation;
		if (direction.magnitude < 0.1f)
		{
			direction = Vector3.zero;
		}
		else
		{
			direction.y = 0f;
			direction.Normalize();
			direction = Quaternion.Euler(0f, UnityEngine.Random.Range(-randomAngle, randomAngle) * 0.5f, 0f) * direction;
			direction *= UnityEngine.Random.Range(0.5f, 1f) * 3f;
			direction.y = 2.5f;
		}
		this.rb.velocity = direction;
		this.DestroyRigidbody().Forget();
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x000203EB File Offset: 0x0001E5EB
	protected override void OnDestroy()
	{
		this.destroied = true;
		base.OnDestroy();
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x000203FC File Offset: 0x0001E5FC
	private UniTaskVoid DestroyRigidbody()
	{
		InteractablePickup.<DestroyRigidbody>d__15 <DestroyRigidbody>d__;
		<DestroyRigidbody>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<DestroyRigidbody>d__.<>4__this = this;
		<DestroyRigidbody>d__.<>1__state = -1;
		<DestroyRigidbody>d__.<>t__builder.Start<InteractablePickup.<DestroyRigidbody>d__15>(ref <DestroyRigidbody>d__);
		return <DestroyRigidbody>d__.<>t__builder.Task;
	}

	// Token: 0x040006BF RID: 1727
	[SerializeField]
	private DuckovItemAgent itemAgent;

	// Token: 0x040006C0 RID: 1728
	public SpriteRenderer sprite;

	// Token: 0x040006C1 RID: 1729
	private Rigidbody rb;

	// Token: 0x040006C2 RID: 1730
	private Vector3 throwStartPoint;

	// Token: 0x040006C3 RID: 1731
	private bool destroied;
}
