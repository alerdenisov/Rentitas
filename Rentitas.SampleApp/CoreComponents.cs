using System;

namespace Rentitas.SampleApp
{
    public class PauseComponent : ICorePool
    {
         
    }

    public class SettingsComponent : ICorePool
    {
        public string GameName;
        public Version Version;
        public string Author;
        public bool IsDebug;
    }

    public class TimerComponent : ICorePool
    {
        public long Time;
    }

    public enum States
    {
        Initialize,
        Loading,
        Menu,
        InGame,
        Over
    }

    public class GameStateComponent : ICorePool
    {
        public States Current;
    }
}