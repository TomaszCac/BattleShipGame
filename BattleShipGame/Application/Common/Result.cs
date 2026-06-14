namespace BattleShipGame.Application.Common
{
    /// <summary>
    /// Class for establishing error results
    /// </summary>
    /// <typeparam name="T">Result type to create</typeparam>
    public class Result<T>
    {
        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }
        public T? Errors { get; set; }
    }
}
