using System;

namespace CSVParser.Core.TrackFiles.TrackBunches.Automatonymous
{
    public class StateMachine<TState>
        where TState : StateMachineState
    {
        public TState State { get; private set; }

        //protected void Initially(Action<StateMachineCommand<TState>> mutator)
        //{
        //    var cmd = new StateMachineCommand<TState>();
        //    mutator?.Invoke(cmd);
        //    State = cmd.Process();
        //}
        
        //protected void Initially(TState state)
        //{
        //    throw new NotImplementedException();
        //    var cmd = new StateMachineCommand<TState>();
        //    mutator?.Invoke(cmd);
        //    State = cmd.Process();
        //}

        protected void During(TState state)
        {
            throw new NotImplementedException();
        }

        public IContinueCommand<TState> When(TrackStatus status)
        {
            throw new NotImplementedException();
        }

    }

    public interface IContinueCommand<TState>
        where TState : StateMachineState
    {
        public TState TransitionTo(TState state);
    }

    public class StateMachineCommand<TState>
        where TState : StateMachineState
    {
        public StateMachineCommand<TState> When(Func<bool> when)
        {
            throw new NotImplementedException();
        }

        public StateMachineCommand<TState> TransitionTo(Func<TState> transitionTo)
        {
            throw new NotImplementedException();
        }

        public TState Process()
        {
            throw new NotImplementedException();
        }
    }

    public class StateMachineState
    {
        // ReSharper disable once InconsistentNaming
        public interface State { }
    }
}