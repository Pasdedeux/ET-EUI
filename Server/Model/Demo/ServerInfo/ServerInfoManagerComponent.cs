/*======================================
* 项目名称 ：ET.Demo.ServerInfo
* 项目描述 ：
* 类 名 称 ：ServerInfoManagerComponent
* 类 描 述 ：
* 命名空间 ：ET.Demo.ServerInfo
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/9 14:53:02
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Derek Liu 2022. All rights reserved.
*******************************************************************
======================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ServerInfoManagerComponent : Entity, IAwake, IDestroy, ILoad
    {
        public List<ServerInfo> ServerInfos = new List<ServerInfo>();
    }
}
