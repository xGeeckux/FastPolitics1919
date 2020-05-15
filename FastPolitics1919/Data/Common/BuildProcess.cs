using FastPolitics1919.Common;
using System;

namespace FastPolitics1919.Data.Common
{
    public class BuildProcess
    {
        //- Round Infos
        public Round StartRound { get; set; }
        public Round EndRound { get; set; }
        public Round RemainingRounds => new Round(Math.Abs(EndRound.Number - StartRound.Number));

        //- ProcessObject
        public GameObject GameObject { get; set; }

        //- ProcessInfos
        public double Status => (double)Engine.Game.Current.Number / EndRound.Number;

        public bool IsDone { get; set; }

        //- Constructor
        public BuildProcess()
        {
            StartRound = Engine.Game.Current;
        }

        //- Constructor Effect
        public virtual void OnBuild()
        {

        }

        //- Abord
        public virtual void Abord()
        {

        }

        //- Instant Effect
        public virtual void Done()
        {
            IsDone = true;
            GameObject.MyProcess = null;
        }

        //- Secound Constructor
        protected void Load(GameObject game_object)
        {
            if (!game_object.IsProcessable)
                throw new Exception("Das übergeben GameObject ist nicht Process-bar.");
            GameObject = game_object;
            GameObject.MyProcess = this;
            EndRound = new Round(StartRound.Number + GameObject.ProcessRound);
            Log.Write("Neue Process gestartet, für (" + game_object.ID + " " + game_object.Name + ")" + ". Endet in Runde: " + EndRound.Number);
            Engine.Game.EveryProcess.Add(this, EndRound.Number);
            OnBuild();
        }

        //- SetTime

    }
}
