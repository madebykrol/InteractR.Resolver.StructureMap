using System.Threading;
using System.Threading.Tasks;

using InteractorHub.Resolvers.AutoFac;
using InteractR;
using InteractR.Interactor;
using InteractR.Resolver.AutoFac.Tests.Mocks;
using NSubstitute;
using NUnit.Framework;
using StructureMap;

namespace InteractorHub.Tests.Resolvers.AutoFac
{
    [TestFixture]
    public class AutoFacTests
    {
        private IInteractorHub _interactorHub;
        private IInteractor<MockUseCase, IMockOutputPort> _useCaseInteractor;

        [SetUp]
        public void Setup()
        {
            
            _useCaseInteractor = Substitute.For<IInteractor<MockUseCase, IMockOutputPort>>();

            var container = new Container(c =>
            {
                c.For<IInteractor<MockUseCase, IMockOutputPort>>().Use(_useCaseInteractor);
            });
            
            _interactorHub = new Hub(new StructureMapResolver(container));
        }

        [Test]
        public async Task Test_AutoFac_Resolver()
        {
            await _interactorHub.Execute(new MockUseCase(), (IMockOutputPort)new MockOutputPort());
            await _useCaseInteractor.Received().Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<CancellationToken>());
        }

    }
}
