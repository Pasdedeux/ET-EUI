﻿//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Net;
//using System;
//using MongoDB.Bson.Serialization.Attributes;
//using ProtoBuf;
//using ET;

////namespace ET
////{
//    public partial class StartSceneConfigCategory
//    {
//        public MultiMap<int, StartSceneConfig> Gates = new MultiMap<int, StartSceneConfig>();
        
//        //public MultiMap<int, StartSceneConfig> ProcessScenes = new MultiMap<int, StartSceneConfig>();
        
//        public Dictionary<long, Dictionary<string, StartSceneConfig>> ZoneScenesByName = new Dictionary<long, Dictionary<string, StartSceneConfig>>();

//        public StartSceneConfig LocationConfig;
        
//        public List<StartSceneConfig> Robots = new List<StartSceneConfig>();
        
//        public List<StartSceneConfig> GetByProcess(int process)
//        {
//            return this.ProcessScenes[process];
//        }
        
//        public StartSceneConfig GetBySceneName(int zone, string name)
//        {
//            return this.ZoneScenesByName[zone][name];
//        }
        
//        public override void AfterEndInit()
//        {
//            foreach (StartSceneConfig startSceneConfig in Configs.StartSceneConfigDict.Values)
//            {
//                //this.ProcessScenes.Add(startSceneConfig.BeginInit, startSceneConfig);
                
//                if (!this.ZoneScenesByName.ContainsKey(startSceneConfig.Zone))
//                {
//                    this.ZoneScenesByName.Add(startSceneConfig.Zone, new Dictionary<string, StartSceneConfig>());
//                }
//                this.ZoneScenesByName[startSceneConfig.Zone].Add(startSceneConfig.Name, startSceneConfig);
                
//                switch (startSceneConfig.Type)
//                {
//                    case SceneType.Gate:
//                        this.Gates.Add(startSceneConfig.Zone, startSceneConfig);
//                        break;
//                    case SceneType.Location:
//                        this.LocationConfig = startSceneConfig;
//                        break;
//                    case SceneType.Robot:
//                        this.Robots.Add(startSceneConfig);
//                        break;
//                }
//            }
//        }
//    }

//    [ProtoContract]
//    [Config]
//    public partial class StartSceneConfigCategory : ProtoObject
//    {
//        public static StartSceneConfigCategory Instance;

//        [ProtoIgnore]
//        [BsonIgnore]
//        private Dictionary<int, StartSceneConfig> dict = new Dictionary<int, StartSceneConfig>();

//        [BsonElement]
//        [ProtoMember(1)]
//        private List<StartSceneConfig> list = new List<StartSceneConfig>();

//        public StartSceneConfigCategory()
//        {
//            Instance = this;
//        }

//        public override void EndInit()
//        {
//            foreach (StartSceneConfig config in list)
//            {
//                this.dict.Add(config.Id, config);
//            }
//            this.AfterEndInit();
//        }

//        public StartSceneConfig Get(int id)
//        {
//            this.dict.TryGetValue(id, out StartSceneConfig item);

//            if (item == null)
//            {
//                throw new Exception($"配置找不到，配置表名: {nameof(StartSceneConfig)}，配置id: {id}");
//            }

//            return item;
//        }

//        public bool Contain(int id)
//        {
//            return this.dict.ContainsKey(id);
//        }

//        public Dictionary<int, StartSceneConfig> GetAll()
//        {
//            return this.dict;
//        }

//        public StartSceneConfig GetOne()
//        {
//            if (this.dict == null || this.dict.Count <= 0)
//            {
//                return null;
//            }
//            return this.dict.Values.GetEnumerator().Current;
//        }
//    }

////}