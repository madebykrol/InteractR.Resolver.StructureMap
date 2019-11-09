using InteractR.Interactor;
using StructureMap;

namespace InteractR.Resolver.StructureMap
{
    public class StructureMapResolver : IResolver
    {
        private readonly IContainer _container;
        public StructureMapResolver(IContainer container)
        {
            _container = container;
        }

        private T Resolve<T>()
        {
            return _container.GetInstance<T>();
        }
        public IInteractor<TUseCase, TOutputPort> ResolveInteractor<TUseCase, TOutputPort>(TUseCase useCase) where TUseCase : IUseCase<TOutputPort>
        {
            return Resolve<IInteractor<TUseCase, TOutputPort>>();
        }
    }
}
