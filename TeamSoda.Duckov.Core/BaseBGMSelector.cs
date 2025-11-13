using System;
using System.Collections.Generic;
using System.IO;
using Duckov;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class BaseBGMSelector : MonoBehaviour
{
	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060001B0 RID: 432 RVA: 0x00008663 File Offset: 0x00006863
	// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000866A File Offset: 0x0000686A
	[LocalizationKey("Default")]
	private string BGMInfoFormatKey
	{
		get
		{
			return "BGMInfoFormat";
		}
		set
		{
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000866C File Offset: 0x0000686C
	private string BGMInfoFormat
	{
		get
		{
			return this.BGMInfoFormatKey.ToPlainText();
		}
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00008679 File Offset: 0x00006879
	private void Awake()
	{
		SavesSystem.OnCollectSaveData += this.Save;
		this.Load(false);
		this.ScanCustomBGM();
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00008699 File Offset: 0x00006899
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x000086AC File Offset: 0x000068AC
	private void ScanCustomBGM()
	{
		string path = Path.Combine(Application.streamingAssetsPath, "Music");
		string searchPattern = "*.mp3";
		foreach (string text in Directory.EnumerateFiles(path, searchPattern))
		{
			string musicName;
			string author;
			BaseBGMSelector.ParseMusicFileName(Path.GetFileNameWithoutExtension(text), out musicName, out author);
			BaseBGMSelector.Entry item = new BaseBGMSelector.Entry
			{
				musicName = musicName,
				author = author,
				filePath = text
			};
			this.entries.Add(item);
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00008748 File Offset: 0x00006948
	private static void ParseMusicFileName(string fileName, out string musicName, out string authorName)
	{
		musicName = fileName;
		authorName = "Unknown";
		if (fileName.Contains('-'))
		{
			int num = fileName.LastIndexOf('-');
			authorName = fileName.Substring(0, num).Trim();
			if (num + 1 < fileName.Length)
			{
				musicName = fileName.Substring(num + 1).Trim();
			}
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000879C File Offset: 0x0000699C
	private void Update()
	{
		if (this.waitForStinger && LevelManager.AfterInit && !AudioManager.IsStingerPlaying)
		{
			this.waitForStinger = false;
			this.Set(this.index, false, true);
		}
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x000087C9 File Offset: 0x000069C9
	private void Load(bool play = false)
	{
		this.index = SavesSystem.Load<int>("BaseBGMSelector");
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x000087DB File Offset: 0x000069DB
	private void Save()
	{
		SavesSystem.Save<int>("BaseBGMSelector", this.index);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x000087F0 File Offset: 0x000069F0
	public void Set(int index, bool showInfo = false, bool play = true)
	{
		this.waitForStinger = false;
		if (index < 0 || index >= this.entries.Count)
		{
			int num = index;
			index = Mathf.Clamp(index, 0, this.entries.Count - 1);
			Debug.LogError(string.Format("[BGM Selector] Index {0} Out Of Range,clampped to {1}", num, index));
		}
		BaseBGMSelector.Entry entry = this.entries[index];
		AudioManager.StopBGM();
		if (play)
		{
			if (string.IsNullOrWhiteSpace(entry.filePath))
			{
				AudioManager.PlayBGM(entry.switchName);
			}
			else
			{
				AudioManager.PlayCustomBGM(entry.filePath, true);
			}
		}
		if (showInfo)
		{
			string text = this.BGMInfoFormat.Format(new
			{
				name = entry.musicName,
				author = entry.author,
				index = index + 1
			});
			this.proxy.Pop(text, 200f);
		}
	}

	// Token: 0x060001BB RID: 443 RVA: 0x000088BC File Offset: 0x00006ABC
	public void Set(string switchName)
	{
		int num = this.GetIndex(switchName);
		if (num < 0)
		{
			return;
		}
		this.Set(num, false, true);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x000088E0 File Offset: 0x00006AE0
	public int GetIndex(string switchName)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].switchName == switchName)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000891F File Offset: 0x00006B1F
	public void SetNext()
	{
		this.index++;
		if (this.index >= this.entries.Count)
		{
			this.index = 0;
		}
		this.Set(this.index, true, true);
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00008957 File Offset: 0x00006B57
	public void SetPrevious()
	{
		this.index--;
		if (this.index < 0)
		{
			this.index = this.entries.Count - 1;
		}
		this.Set(this.index, true, true);
	}

	// Token: 0x04000166 RID: 358
	[SerializeField]
	private string switchGroupName = "BGM";

	// Token: 0x04000167 RID: 359
	[SerializeField]
	private DialogueBubbleProxy proxy;

	// Token: 0x04000168 RID: 360
	public List<BaseBGMSelector.Entry> entries;

	// Token: 0x04000169 RID: 361
	private int index;

	// Token: 0x0400016A RID: 362
	private const string savekey = "BaseBGMSelector";

	// Token: 0x0400016B RID: 363
	private bool waitForStinger = true;

	// Token: 0x02000433 RID: 1075
	[Serializable]
	public struct Entry
	{
		// Token: 0x04001A46 RID: 6726
		public string musicName;

		// Token: 0x04001A47 RID: 6727
		public string author;

		// Token: 0x04001A48 RID: 6728
		public string switchName;

		// Token: 0x04001A49 RID: 6729
		public string filePath;
	}
}
