using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    

    [ProtoContract]
	public partial class StartSceneConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int Process { get; set; }
		[ProtoMember(3)]
		public int Zone { get; set; }
		[ProtoMember(4)]
		public string SceneType { get; set; }
		[ProtoMember(5)]
		public string Name { get; set; }
		[ProtoMember(6)]
		public int OuterPort { get; set; }

	}
}
