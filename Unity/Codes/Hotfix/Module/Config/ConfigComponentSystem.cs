using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ET
{
	[ObjectSystem]
    public class ConfigAwakeSystem : AwakeSystem<ConfigComponent>
    {
        public override void Awake(ConfigComponent self)
        {
	        ConfigComponent.Instance = self;
        }
    }
    
    [ObjectSystem]
    public class ConfigDestroySystem : DestroySystem<ConfigComponent>
    {
	    public override void Destroy(ConfigComponent self)
	    {
		    ConfigComponent.Instance = null;
	    }
    }
    
    public static class ConfigComponentSystem
	{
		public static void LoadOneConfig(this ConfigComponent self, Type configType)
		{
			byte[] oneConfigBytes = self.ConfigLoader.GetOneConfigBytes(configType.FullName);

			object category = ProtobufHelper.FromBytes(configType, oneConfigBytes, 0, oneConfigBytes.Length);

			self.AllConfig[configType] = category;
		}
		
		public static void Load(this ConfigComponent self)
		{
			self.AllConfig.Clear();
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));
			
			Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();
			self.ConfigLoader.GetAllConfigBytes(configBytes);

			foreach (Type type in types)
			{
				self.LoadOneInThread(type, configBytes);
			}
		}
		
		public static async ETTask LoadAsync(this ConfigComponent self)
		{
			//self.AllConfig.Clear();
			//HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));

			//Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();
			//self.ConfigLoader.GetAllConfigBytes(configBytes);

			//using (ListComponent<Task> listTasks = ListComponent<Task>.Create())
			//{
			//	foreach (Type type in types)
			//	{
			//		Task task = Task.Run(() => self.LoadOneInThread(type, configBytes));
			//		listTasks.Add(task);
			//	}

			//	await Task.WhenAll(listTasks.ToArray());
			//}


			//������ñ������ʹ��
			foreach (string file in Directory.GetFiles($"../Server/Configs", "*.csv"))
			{
				string className = Path.GetFileNameWithoutExtension(file);
				string content = File.ReadAllText(file);
                string methodName = "ReturnDictionary";

				Type t = Game.EventSystem.GetType(className);
				MethodInfo method = t.GetMethod(methodName, System.Reflection.BindingFlags.IgnoreCase
						| System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public
						| System.Reflection.BindingFlags.Static);

				var mainClass = Game.EventSystem.GetType("Configs");
				var props = mainClass.GetField(className + "Dict");
				var finalType = Convert.ChangeType(method.Invoke(null, new object[1] { content }), props.FieldType);
				props.SetValue(props, finalType);


				//List<string> csvKeys = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
				//for (int i = 1; i < csvKeys.Count; i++)
				//{
				//	string item = csvKeys[i].Split(',')[0];
				//	if (!item.EndsWith(".csv")) continue;

				//	//localPath = FrameworkConfig.Instance.UsePersistantPath ? AssetPathManager.Instance.GetPersistentDataPath(item) : AssetPathManager.Instance.GetStreamAssetDataPath(item);
				//	//DocumentAccessor.LoadAsset(localPath, (string e) =>
				//	//{
				//	//	var contens = item.Split('/');
				//	//	string className = (contens.Length > 1 ? contens[1] : contens[0]).Split('.')[0];
				//	//	string strMethod = "ReturnDictionary";

				//	//	//��������£�ͨ�����ڷ����ȡ����Ӧ�༰��̬����
				//	//	if (FrameworkConfig.Instance.scriptEnvironment != RunEnvironment.ILRuntime)
				//	//		Assets.Scripts.DotNetScriptCall.SetConfigInstall(className, strMethod, e);
				//	//	//�ȸ�����£�ͨ���ȸ��ӿڻ�ȡ���ڲ����õ��༰����
				//	//	else
				//	//		Assets.Scripts.ILRScriptCall.SetConfigInstall(className, strMethod, e);

				//	//	Log.Info(string.Format("���õ��������-> {0}", item));
				//	//});
				//}
			}

			//����Configs.Install����
			var configClass = Game.EventSystem.GetType("Configs");
			var configMethod = configClass.GetMethod("Install");
			configMethod.Invoke(null, null);

			await ETTask.CompletedTask;
		}

		private static void LoadOneInThread(this ConfigComponent self, Type configType, Dictionary<string, byte[]> configBytes)
		{
			byte[] oneConfigBytes = configBytes[configType.Name];

			object category = ProtobufHelper.FromBytes(configType, oneConfigBytes, 0, oneConfigBytes.Length);

			lock (self)
			{
				self.AllConfig[configType] = category;	
			}
		}
	}
}