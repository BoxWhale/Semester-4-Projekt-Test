namespace Assets.Scripts.Interfaces
{
    public interface IElectricalComponent
    {
        //Less important for now
        string ComponentName  { get; }
        string ComponentDescription { get; }
        
        bool IsFaulted { get; }
        
        
        int ID { get; }
        float ResistanceOhms { get; }
        float MaxVoltageVolts { get; }
        float CurrentVoltageAmps { get; }
        void TickEvaluation();
    }
}