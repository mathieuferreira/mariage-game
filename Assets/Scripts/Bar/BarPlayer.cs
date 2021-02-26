namespace Bar
{
    public class BarPlayer : BaseRPGPlayer
    {
        private const int MaxConsumable = 3;
    
        private BarConsumableList consumableList = new BarConsumableList(MaxConsumable);
    
        protected override void Awake()
        {
            base.Awake();
            LockMove();
        }

        public BarConsumableList GetConsumableList()
        {
            return consumableList;
        }
    }
}
