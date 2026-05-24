namespace BattleShipGame.Application.Common
{
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
