using System.Collections.Generic;

namespace Assets.Scripts.Utilities
{
    public static class Mappings
    {
        public static List<KeyValuePair<PowerupTypes, string>> PowerupSprites
        {
            get
            {
                List<KeyValuePair<PowerupTypes, string>> retVal = new List<KeyValuePair<PowerupTypes, string>>();

                retVal.Add(new KeyValuePair<PowerupTypes, string>(PowerupTypes.Jump, "JumpPickup"));
                retVal.Add(new KeyValuePair<PowerupTypes, string>(PowerupTypes.Speed, "SpeedPickup"));

                return retVal;
            }
        }
    }
}