using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DuckovSkills
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	public static class ModConfigAPI
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00004E10 File Offset: 0x00003010
		private static bool CheckVersionCompatibility()
		{
			if (ModConfigAPI.versionChecked)
			{
				return ModConfigAPI.isVersionCompatible;
			}
			bool result;
			try
			{
				FieldInfo field = ModConfigAPI.modBehaviourType.GetField("VERSION", BindingFlags.Static | BindingFlags.Public);
				if (field != null && field.FieldType == typeof(int))
				{
					int num = (int)field.GetValue(null);
					ModConfigAPI.isVersionCompatible = (num == 1);
					if (!ModConfigAPI.isVersionCompatible)
					{
						Debug.LogError(string.Format("[{0}] 版本不匹配！API版本: {1}, ModConfig版本: {2}", ModConfigAPI.TAG, 1, num));
						result = false;
					}
					else
					{
						Debug.Log(string.Format("[{0}] 版本检查通过: {1}", ModConfigAPI.TAG, 1));
						ModConfigAPI.versionChecked = true;
						result = true;
					}
				}
				else
				{
					Debug.LogWarning("[" + ModConfigAPI.TAG + "] 未找到版本信息字段，跳过版本检查");
					ModConfigAPI.isVersionCompatible = true;
					ModConfigAPI.versionChecked = true;
					result = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[" + ModConfigAPI.TAG + "] 版本检查失败: " + ex.Message);
				ModConfigAPI.isVersionCompatible = false;
				ModConfigAPI.versionChecked = true;
				result = false;
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004F2C File Offset: 0x0000312C
		public static bool Initialize()
		{
			bool result;
			try
			{
				if (ModConfigAPI.isInitialized)
				{
					result = true;
				}
				else
				{
					ModConfigAPI.modBehaviourType = ModConfigAPI.FindTypeInAssemblies("ModConfig.ModBehaviour");
					if (ModConfigAPI.modBehaviourType == null)
					{
						Debug.LogWarning("[" + ModConfigAPI.TAG + "] ModConfig.ModBehaviour 类型未找到，ModConfig 可能未加载");
						result = false;
					}
					else
					{
						ModConfigAPI.optionsManagerType = ModConfigAPI.FindTypeInAssemblies("ModConfig.OptionsManager_Mod");
						if (ModConfigAPI.optionsManagerType == null)
						{
							Debug.LogWarning("[" + ModConfigAPI.TAG + "] ModConfig.OptionsManager_Mod 类型未找到");
							result = false;
						}
						else if (!ModConfigAPI.CheckVersionCompatibility())
						{
							Debug.LogWarning("[" + ModConfigAPI.TAG + "] ModConfig version mismatch!!!");
							result = false;
						}
						else
						{
							foreach (string text in new string[]
							{
								"AddDropdownList",
								"AddInputWithSlider",
								"AddBoolDropdownList",
								"AddOnOptionsChangedDelegate",
								"RemoveOnOptionsChangedDelegate"
							})
							{
								if (ModConfigAPI.modBehaviourType.GetMethod(text, BindingFlags.Static | BindingFlags.Public) == null)
								{
									Debug.LogError(string.Concat(new string[]
									{
										"[",
										ModConfigAPI.TAG,
										"] 必要方法 ",
										text,
										" 未找到"
									}));
									return false;
								}
							}
							ModConfigAPI.isInitialized = true;
							Debug.Log("[" + ModConfigAPI.TAG + "] ModConfigAPI 初始化成功");
							result = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[" + ModConfigAPI.TAG + "] 初始化失败: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000050DC File Offset: 0x000032DC
		private static Type FindTypeInAssemblies(string typeName)
		{
			Type result;
			try
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in assemblies)
				{
					try
					{
						if (assembly.FullName.Contains("ModConfig"))
						{
							Debug.Log("[" + ModConfigAPI.TAG + "] 找到 ModConfig 相关程序集: " + assembly.FullName);
						}
						Type type = assembly.GetType(typeName);
						if (type != null)
						{
							Debug.Log(string.Concat(new string[]
							{
								"[",
								ModConfigAPI.TAG,
								"] 在程序集 ",
								assembly.FullName,
								" 中找到类型 ",
								typeName
							}));
							return type;
						}
					}
					catch (Exception)
					{
					}
				}
				Debug.LogWarning(string.Format("[{0}] 在所有程序集中未找到类型 {1}，已加载程序集数量: {2}", ModConfigAPI.TAG, typeName, assemblies.Length));
				foreach (Assembly assembly2 in from a in assemblies
				where a.FullName.Contains("ModConfig")
				select a)
				{
					Debug.Log("[" + ModConfigAPI.TAG + "] ModConfig 相关程序集: " + assembly2.FullName);
				}
				result = null;
			}
			catch (Exception ex)
			{
				Debug.LogError("[" + ModConfigAPI.TAG + "] 程序集扫描失败: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000052A4 File Offset: 0x000034A4
		public static bool SafeAddOnOptionsChangedDelegate(Action<string> action)
		{
			if (!ModConfigAPI.Initialize())
			{
				return false;
			}
			if (action == null)
			{
				Debug.LogWarning("[" + ModConfigAPI.TAG + "] 不能添加空的事件委托");
				return false;
			}
			bool result;
			try
			{
				ModConfigAPI.modBehaviourType.GetMethod("AddOnOptionsChangedDelegate", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[]
				{
					action
				});
				Debug.Log("[" + ModConfigAPI.TAG + "] 成功添加选项变更事件委托");
				result = true;
			}
			catch (Exception ex)
			{
				Debug.LogError("[" + ModConfigAPI.TAG + "] 添加选项变更事件委托失败: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00005350 File Offset: 0x00003550
		public static bool SafeRemoveOnOptionsChangedDelegate(Action<string> action)
		{
			if (!ModConfigAPI.Initialize())
			{
				return false;
			}
			if (action == null)
			{
				Debug.LogWarning("[" + ModConfigAPI.TAG + "] 不能移除空的事件委托");
				return false;
			}
			bool result;
			try
			{
				ModConfigAPI.modBehaviourType.GetMethod("RemoveOnOptionsChangedDelegate", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[]
				{
					action
				});
				Debug.Log("[" + ModConfigAPI.TAG + "] 成功移除选项变更事件委托");
				result = true;
			}
			catch (Exception ex)
			{
				Debug.LogError("[" + ModConfigAPI.TAG + "] 移除选项变更事件委托失败: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000053FC File Offset: 0x000035FC
		public static bool SafeAddDropdownList(string modName, string key, string description, SortedDictionary<string, object> options, Type valueType, object defaultValue)
		{
			key = modName + "_" + key;
			if (!ModConfigAPI.Initialize())
			{
				return false;
			}
			bool result;
			try
			{
				ModConfigAPI.modBehaviourType.GetMethod("AddDropdownList", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[]
				{
					modName,
					key,
					description,
					options,
					valueType,
					defaultValue
				});
				Debug.Log(string.Concat(new string[]
				{
					"[",
					ModConfigAPI.TAG,
					"] 成功添加下拉列表: ",
					modName,
					".",
					key
				}));
				result = true;
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[",
					ModConfigAPI.TAG,
					"] 添加下拉列表失败 ",
					modName,
					".",
					key,
					": ",
					ex.Message
				}));
				result = false;
			}
			return result;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000054F4 File Offset: 0x000036F4
		public static bool SafeAddInputWithSlider(string modName, string key, string description, Type valueType, object defaultValue, Vector2? sliderRange = null)
		{
			key = modName + "_" + key;
			if (!ModConfigAPI.Initialize())
			{
				return false;
			}
			bool result;
			try
			{
				MethodInfo method = ModConfigAPI.modBehaviourType.GetMethod("AddInputWithSlider", BindingFlags.Static | BindingFlags.Public);
				object[] array2;
				if (sliderRange == null)
				{
					object[] array = new object[6];
					array[0] = modName;
					array[1] = key;
					array[2] = description;
					array[3] = valueType;
					array2 = array;
					array[4] = defaultValue;
				}
				else
				{
					object[] array3 = new object[6];
					array3[0] = modName;
					array3[1] = key;
					array3[2] = description;
					array3[3] = valueType;
					array3[4] = defaultValue;
					array2 = array3;
					array3[5] = sliderRange.Value;
				}
				object[] parameters = array2;
				method.Invoke(null, parameters);
				Debug.Log(string.Concat(new string[]
				{
					"[",
					ModConfigAPI.TAG,
					"] 成功添加滑条输入框: ",
					modName,
					".",
					key
				}));
				result = true;
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[",
					ModConfigAPI.TAG,
					"] 添加滑条输入框失败 ",
					modName,
					".",
					key,
					": ",
					ex.Message
				}));
				result = false;
			}
			return result;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00005620 File Offset: 0x00003820
		public static bool SafeAddBoolDropdownList(string modName, string key, string description, bool defaultValue)
		{
			key = modName + "_" + key;
			if (!ModConfigAPI.Initialize())
			{
				return false;
			}
			bool result;
			try
			{
				ModConfigAPI.modBehaviourType.GetMethod("AddBoolDropdownList", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[]
				{
					modName,
					key,
					description,
					defaultValue
				});
				Debug.Log(string.Concat(new string[]
				{
					"[",
					ModConfigAPI.TAG,
					"] 成功添加布尔下拉列表: ",
					modName,
					".",
					key
				}));
				result = true;
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[",
					ModConfigAPI.TAG,
					"] 添加布尔下拉列表失败 ",
					modName,
					".",
					key,
					": ",
					ex.Message
				}));
				result = false;
			}
			return result;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00005714 File Offset: 0x00003914
		public static T SafeLoad<[Nullable(2)] T>(string mod_name, string key, T defaultValue = default(T))
		{
			key = mod_name + "_" + key;
			if (!ModConfigAPI.Initialize())
			{
				return defaultValue;
			}
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogWarning("[" + ModConfigAPI.TAG + "] 配置键不能为空");
				return defaultValue;
			}
			T result;
			try
			{
				MethodInfo method = ModConfigAPI.optionsManagerType.GetMethod("Load", BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					Debug.LogError("[" + ModConfigAPI.TAG + "] 未找到 OptionsManager_Mod.Load 方法");
					result = defaultValue;
				}
				else
				{
					object obj = method.MakeGenericMethod(new Type[]
					{
						typeof(T)
					}).Invoke(null, new object[]
					{
						key,
						defaultValue
					});
					Debug.Log(string.Format("[{0}] 成功加载配置: {1} = {2}", ModConfigAPI.TAG, key, obj));
					result = (T)((object)obj);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[",
					ModConfigAPI.TAG,
					"] 加载配置失败 ",
					key,
					": ",
					ex.Message
				}));
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000583C File Offset: 0x00003A3C
		public static bool SafeSave<[Nullable(2)] T>(string mod_name, string key, T value)
		{
			key = mod_name + "_" + key;
			if (!ModConfigAPI.Initialize())
			{
				return false;
			}
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogWarning("[" + ModConfigAPI.TAG + "] 配置键不能为空");
				return false;
			}
			bool result;
			try
			{
				MethodInfo method = ModConfigAPI.optionsManagerType.GetMethod("Save", BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					Debug.LogError("[" + ModConfigAPI.TAG + "] 未找到 OptionsManager_Mod.Save 方法");
					result = false;
				}
				else
				{
					method.MakeGenericMethod(new Type[]
					{
						typeof(T)
					}).Invoke(null, new object[]
					{
						key,
						value
					});
					Debug.Log(string.Format("[{0}] 成功保存配置: {1} = {2}", ModConfigAPI.TAG, key, value));
					result = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[",
					ModConfigAPI.TAG,
					"] 保存配置失败 ",
					key,
					": ",
					ex.Message
				}));
				result = false;
			}
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00005964 File Offset: 0x00003B64
		public static bool IsAvailable()
		{
			return ModConfigAPI.Initialize();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000596C File Offset: 0x00003B6C
		public static string GetVersionInfo()
		{
			if (!ModConfigAPI.Initialize())
			{
				return "ModConfig 未加载 | ModConfig not loaded";
			}
			string result;
			try
			{
				FieldInfo field = ModConfigAPI.modBehaviourType.GetField("VERSION", BindingFlags.Static | BindingFlags.Public);
				if (field != null && field.FieldType == typeof(int))
				{
					int num = (int)field.GetValue(null);
					string arg = (num == 1) ? "兼容" : "不兼容";
					result = string.Format("ModConfig v{0} (API v{1}, {2})", num, 1, arg);
				}
				else
				{
					PropertyInfo property = ModConfigAPI.modBehaviourType.GetProperty("VERSION", BindingFlags.Static | BindingFlags.Public);
					if (property != null)
					{
						object value = property.GetValue(null);
						result = (((value != null) ? value.ToString() : null) ?? "未知版本 | Unknown version");
					}
					else
					{
						result = "ModConfig 已加载（版本信息不可用） | ModConfig loaded (version info unavailable)";
					}
				}
			}
			catch
			{
				result = "ModConfig 已加载（版本检查失败） | ModConfig loaded (version check failed)";
			}
			return result;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00005A54 File Offset: 0x00003C54
		public static bool IsVersionCompatible()
		{
			return ModConfigAPI.Initialize() && ModConfigAPI.isVersionCompatible;
		}

		// Token: 0x04000037 RID: 55
		public static string ModConfigName = "ModConfig";

		// Token: 0x04000038 RID: 56
		private const int ModConfigVersion = 1;

		// Token: 0x04000039 RID: 57
		private static string TAG = string.Format("ModConfig_v{0}", 1);

		// Token: 0x0400003A RID: 58
		private static Type modBehaviourType;

		// Token: 0x0400003B RID: 59
		private static Type optionsManagerType;

		// Token: 0x0400003C RID: 60
		public static bool isInitialized = false;

		// Token: 0x0400003D RID: 61
		private static bool versionChecked = false;

		// Token: 0x0400003E RID: 62
		private static bool isVersionCompatible = false;
	}
}
