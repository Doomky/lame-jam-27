using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Game : Game<Game.StateMachine, Game.Action>
    {
        public enum Action
        {
            
        }

        public new class StateMachine : GameStateMachine<Action>
        {
            public StateMachine() : base(null)
            {
            }

            protected override bool TryGetNextState(Action action, out GameState nextState)
            {
                switch (action)
                {
                    default:
                        nextState = null;
                        return false;
                }
            }
        }
    }
}
