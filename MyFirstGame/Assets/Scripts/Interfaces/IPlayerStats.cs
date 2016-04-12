using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interfaces
{
    public interface IPlayerStats
    {
        int PermanentSpeedCount { get; set; }
        int TempSpeedCount { get; set; }

        int PermanentJumpCount { get; set; }
        int TempJumpCount { get; set; }
    }
}
