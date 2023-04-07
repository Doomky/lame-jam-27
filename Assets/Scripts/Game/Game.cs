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
            EnterInGame     = 1 << 0,
            ExitFromGame    = 1 << 1,
        }

        public new class StateMachine : GameStateMachine<Action>
        {
            private GameState _inGameState = new();
            
            private GameState _outGameState = new();

            public StateMachine() : base(null)
            {
            }

            protected override bool TryGetNextState(Action action, out GameState nextState)
            {
                switch (action)
                {
                    case Action.EnterInGame:
                        {
                            nextState = _inGameState;
                            return true;
                        }
                        
                    case Action.ExitFromGame:
                        {
                            nextState = _outGameState;
                            return true;
                        }

                    default:
                        {
                            nextState = null;
                            return false;
                        }
                }
            }
        }
    }
}
