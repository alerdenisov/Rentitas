namespace Rentitas
{
    public struct TriggerOnEvent
    {

        public IMatcher Trigger;
        public GroupEventType EventType;

        public TriggerOnEvent(IMatcher trigger, GroupEventType eventType)
        {
            this.Trigger = trigger;
            this.EventType = eventType;
        }
    }
}