namespace BTKRoadmapperAPI.Middlewares
{
    public class ClientBreakFlow : Exception
    {
        public ClientBreakFlow(string message) : base(message)
        {
        }
    }
}
