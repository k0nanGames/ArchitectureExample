namespace Example.Core
{
    public class BaseChain : IGameChain
    {
        private IGameChain _nextChain;
        
        public virtual BaseChainsData Handle(BaseChainsData data)
        {
            return HandleNext(data);
        }

        public virtual IGameChain SetNext(IGameChain next)
        {
            _nextChain = next;
            return next;
        }
        
        protected BaseChainsData HandleNext(BaseChainsData data)
        {
            return _nextChain?.Handle(data);
        }
    }
}