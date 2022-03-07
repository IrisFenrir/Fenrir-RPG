namespace IrisFenrir.AI.FSM
{
    public class NotCondition<T> : FSMCondition<T>
    {
        private FSMCondition<T> m_condition;

        public NotCondition(FSMCondition<T> con)
        {
            m_condition = con;
        }

        public override bool Condition(T owner)
        {
            return !m_condition.Condition(owner);
        }
    }
}
