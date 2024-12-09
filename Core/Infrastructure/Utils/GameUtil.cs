using System;
using Core.Infrastructure.Enums;

namespace Core.Infrastructure.Utils
{
    public static class GameUtil
    {
        public static int GetStage(Stage stage)
        {
            var stageNumber = stage switch
            {
                Stage.First => 0,
                Stage.Second => 1,
                Stage.Third => 2,
                Stage.Fourth => 3,
                Stage.Fifth => 4,
                Stage.Bubble => 5,
                Stage.Transition => 6,
                _ => throw new ArgumentException()
            };

            return stageNumber;
        }
    }
}