using System.Collections.Generic;
using System.Linq;

namespace ET
{
	public static class RealmGateAddressHelper
	{
		public static StartSceneConfig GetGate(int zone)
		{
			//TODO
			var zoneGates = Configs.StartSceneConfigDict.Values.Where(e => e.Zone == zone);// StartSceneConfigCategory.Instance.Gates[zone];
			
			int n = RandomHelper.RandomNumber(0, zoneGates.Count());

			return zoneGates.ElementAt(n);
		}
	}
}
