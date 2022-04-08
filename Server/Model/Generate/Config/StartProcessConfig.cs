using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    

    [ProtoContract]
	public partial class StartProcessConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int MachineId { get; set; }
		[ProtoMember(3)]
		public int InnerPort { get; set; }
		[ProtoMember(4)]
		public string AppName { get; set; }

	}
}
