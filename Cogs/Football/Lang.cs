using LCChaosMod.Utils;

namespace LCChaosMod.Cogs.Football
{
    internal static class Lang
    {
        public static void Init()
        {
            Loc.Register("event.football",      "Football time!",   "Час футболу!");
            Loc.Register("ui.football_duration","Duration (seconds)","Тривалість (секунди)");
        }
    }
}
