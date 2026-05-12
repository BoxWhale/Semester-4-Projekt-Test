namespace Assets.Scripts.Interfaces
{
    public interface ITerminal
    {
        //Less important for now
        string TerminalName  { get; }
        string TerminalDescription { get; }
        
        int TerminalID { get; }
        int OwnerID { get; }
        
        
    }
}