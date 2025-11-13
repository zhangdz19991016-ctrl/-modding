using System;
using System.Collections.Generic;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200010D RID: 269
public class PlayerPositionBackupManager : MonoBehaviour
{
	// Token: 0x14000047 RID: 71
	// (add) Token: 0x0600093D RID: 2365 RVA: 0x000295B0 File Offset: 0x000277B0
	// (remove) Token: 0x0600093E RID: 2366 RVA: 0x000295E4 File Offset: 0x000277E4
	private static event Action OnStartRecoverEvent;

	// Token: 0x0600093F RID: 2367 RVA: 0x00029617 File Offset: 0x00027817
	private void Awake()
	{
		this.backups = new List<PlayerPositionBackupManager.PlayerPositionBackupEntry>();
		MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		PlayerPositionBackupManager.OnStartRecoverEvent += this.OnStartRecover;
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x00029646 File Offset: 0x00027846
	private void OnDestroy()
	{
		MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		PlayerPositionBackupManager.OnStartRecoverEvent -= this.OnStartRecover;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0002966C File Offset: 0x0002786C
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.mainCharacter)
		{
			this.mainCharacter = CharacterMainControl.Main;
		}
		if (!this.mainCharacter)
		{
			return;
		}
		this.backupTimer -= Time.deltaTime;
		if (this.backupTimer < 0f && this.CheckCanBackup())
		{
			this.BackupCurrentPos();
		}
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x000296D4 File Offset: 0x000278D4
	private bool CheckCanBackup()
	{
		if (!this.mainCharacter)
		{
			return false;
		}
		if (!this.mainCharacter.IsOnGround)
		{
			return false;
		}
		if (Mathf.Abs(this.mainCharacter.Velocity.y) > 2f)
		{
			return false;
		}
		int count = this.backups.Count;
		if (count > 0)
		{
			Vector3 position = this.backups[count - 1].position;
			if (Vector3.Distance(this.mainCharacter.transform.position, position) < this.minBackupDistance)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00029762 File Offset: 0x00027962
	private void OnSubSceneLoaded(MultiSceneCore multiSceneCore, Scene scene)
	{
		this.backups.Clear();
		this.backupTimer = this.backupTimeSpace;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0002977C File Offset: 0x0002797C
	public void BackupCurrentPos()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.mainCharacter)
		{
			return;
		}
		this.backupTimer = this.backupTimeSpace;
		PlayerPositionBackupManager.PlayerPositionBackupEntry item = default(PlayerPositionBackupManager.PlayerPositionBackupEntry);
		item.position = this.mainCharacter.transform.position;
		item.sceneID = SceneManager.GetActiveScene().buildIndex;
		this.backups.Add(item);
		if (this.backups.Count > this.listSize)
		{
			this.backups.RemoveAt(0);
		}
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00029809 File Offset: 0x00027A09
	public static void StartRecover()
	{
		Action onStartRecoverEvent = PlayerPositionBackupManager.OnStartRecoverEvent;
		if (onStartRecoverEvent == null)
		{
			return;
		}
		onStartRecoverEvent();
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0002981C File Offset: 0x00027A1C
	private void OnStartRecover()
	{
		if (this.mainCharacter.CurrentAction != null && this.mainCharacter.CurrentAction.Running)
		{
			this.mainCharacter.CurrentAction.StopAction();
		}
		this.mainCharacter.Interact(this.backupInteract);
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00029870 File Offset: 0x00027A70
	public void SetPlayerToBackupPos()
	{
		if (this.backups.Count <= 0)
		{
			return;
		}
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		Vector3 position = this.mainCharacter.transform.position;
		ref PlayerPositionBackupManager.PlayerPositionBackupEntry ptr = this.backups[this.backups.Count - 1];
		this.backups.RemoveAt(this.backups.Count - 1);
		Vector3 position2 = ptr.position;
		if (Vector3.Distance(position, position2) > this.minBackupDistance)
		{
			this.mainCharacter.SetPosition(position2);
			return;
		}
		this.SetPlayerToBackupPos();
	}

	// Token: 0x04000856 RID: 2134
	private List<PlayerPositionBackupManager.PlayerPositionBackupEntry> backups;

	// Token: 0x04000857 RID: 2135
	private CharacterMainControl mainCharacter;

	// Token: 0x04000858 RID: 2136
	public float backupTimeSpace = 3f;

	// Token: 0x04000859 RID: 2137
	public float minBackupDistance = 3f;

	// Token: 0x0400085A RID: 2138
	private float backupTimer = 3f;

	// Token: 0x0400085B RID: 2139
	public InteractableBase backupInteract;

	// Token: 0x0400085C RID: 2140
	public int listSize = 20;

	// Token: 0x0200049A RID: 1178
	private struct PlayerPositionBackupEntry
	{
		// Token: 0x04001C13 RID: 7187
		public int sceneID;

		// Token: 0x04001C14 RID: 7188
		public Vector3 position;
	}
}
