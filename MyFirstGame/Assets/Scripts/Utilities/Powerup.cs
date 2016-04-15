namespace Assets.Scripts.Utilities
{
    public class Powerup
    {
        public PowerupTypes Type;
        public int PermanentCount;
        public int TempCount;

        public Powerup(PowerupTypes type, int permanentCount, int tempCount)
        {
            Type = type;
            PermanentCount = permanentCount;
            TempCount = tempCount;
        }
    }
}
