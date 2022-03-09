using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum SessionState
    {
        Normal,
        Game,
    }

    public class SessionStateComponent:Entity, IAwake
    {
        public SessionState state;
    }
}
