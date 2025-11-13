using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Saves;
using UnityEngine;

namespace Duckov.Tasks
{
	// Token: 0x02000376 RID: 886
	public class Startup : MonoBehaviour
	{
		// Token: 0x06001ED5 RID: 7893 RVA: 0x0006CEB6 File Offset: 0x0006B0B6
		private void Awake()
		{
			this.MoveOldSaves();
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0006CEC0 File Offset: 0x0006B0C0
		private void MoveOldSaves()
		{
			string fullPathToSavesFolder = SavesSystem.GetFullPathToSavesFolder();
			if (!Directory.Exists(fullPathToSavesFolder))
			{
				Directory.CreateDirectory(fullPathToSavesFolder);
			}
			for (int i = 1; i <= 3; i++)
			{
				string saveFileName = SavesSystem.GetSaveFileName(i);
				string text = Path.Combine(Application.persistentDataPath, saveFileName);
				string text2 = Path.Combine(fullPathToSavesFolder, saveFileName);
				if (File.Exists(text) && !File.Exists(text2))
				{
					Debug.Log("Transporting:\n" + text + "\n->\n" + text2);
					SavesSystem.UpgradeSaveFileAssemblyInfo(text);
					File.Move(text, text2);
				}
			}
			string path = "Options.ES3";
			string text3 = Path.Combine(Application.persistentDataPath, path);
			string text4 = Path.Combine(fullPathToSavesFolder, path);
			if (File.Exists(text3) && !File.Exists(text4))
			{
				Debug.Log("Transporting:\n" + text3 + "\n->\n" + text4);
				SavesSystem.UpgradeSaveFileAssemblyInfo(text3);
				File.Move(text3, text4);
			}
			string globalSaveDataFileName = SavesSystem.GlobalSaveDataFileName;
			string text5 = Path.Combine(Application.persistentDataPath, globalSaveDataFileName);
			string text6 = Path.Combine(fullPathToSavesFolder, globalSaveDataFileName);
			if (!File.Exists(text5))
			{
				text5 = Path.Combine(Application.persistentDataPath, "Global.csv");
			}
			if (File.Exists(text5) && !File.Exists(text6))
			{
				Debug.Log("Transporting:\n" + text5 + "\n->\n" + text6);
				SavesSystem.UpgradeSaveFileAssemblyInfo(text5);
				File.Move(text5, text6);
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0006D00C File Offset: 0x0006B20C
		private void Start()
		{
			this.StartupFlow().Forget();
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0006D01C File Offset: 0x0006B21C
		private UniTask StartupFlow()
		{
			Startup.<StartupFlow>d__5 <StartupFlow>d__;
			<StartupFlow>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<StartupFlow>d__.<>4__this = this;
			<StartupFlow>d__.<>1__state = -1;
			<StartupFlow>d__.<>t__builder.Start<Startup.<StartupFlow>d__5>(ref <StartupFlow>d__);
			return <StartupFlow>d__.<>t__builder.Task;
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0006D060 File Offset: 0x0006B260
		private bool EvaluateWaitList()
		{
			foreach (MonoBehaviour monoBehaviour in this.waitForTasks)
			{
				if (!(monoBehaviour == null))
				{
					ITaskBehaviour taskBehaviour = monoBehaviour as ITaskBehaviour;
					if (taskBehaviour != null && !taskBehaviour.IsComplete())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001501 RID: 5377
		public List<MonoBehaviour> beforeSequence = new List<MonoBehaviour>();

		// Token: 0x04001502 RID: 5378
		public List<MonoBehaviour> waitForTasks = new List<MonoBehaviour>();
	}
}
