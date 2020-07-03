namespace Bar
{
    public class BarPlayer : BaseRPGPlayer
    {
        private const int MaxConsumable = 3;
    
        private BarConsumableList consumableList;
    
        protected override void Awake()
        {
            base.Awake();
            LockMove();
            consumableList = new BarConsumableList(MaxConsumable);
        }

        public BarConsumableList GetConsumableList()
        {
            return consumableList;
        }
    }
}
