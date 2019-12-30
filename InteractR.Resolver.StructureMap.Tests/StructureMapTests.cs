using System;
using System.Threading;
using System.Threading.Tasks;
using InteractR.Interactor;
using InteractR.Resolver.StructureMap.Tests.Mocks;
using NSubstitute;
using NUnit.Framework;
using StructureMap;

namespace InteractR.Resolver.StructureMap.Tests
{
    [TestFixture]
    public class StructureMapTests
    {
        private IInteractorHub _interactorHub;
        private IInteractor<MockUseCase, IMockOutputPort> _useCaseInteractor;

        private IMiddleware<MockUseCase, IMockOutputPort> _middleware1;
        private IMiddleware<MockUseCase, IMockOutputPort> _middleware2;

        [SetUp]
        public void Setup()
        {
            _useCaseInteractor = Substitute.For<IInteractor<MockUseCase, IMockOutputPort>>();

            _middleware1 = Substitute.For<IMiddleware<MockUseCase, IMockOutputPort>>();
            _middleware1.Execute(
                    Arg.Any<MockUseCase>(),
                    Arg.Any<IMockOutputPort>(),
                    d => Task.FromResult(new UseCaseResult(true)),
                    Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(x => new UseCaseResult(true))
                .AndDoes(x => x.Arg<Func<MockUseCase, Task<UseCaseResult>>>().Invoke(x.Arg<MockUseCase>()));

            _middleware2 = Substitute.For<IMiddleware<MockUseCase, IMockOutputPort>>();
            _middleware2.Execute(
                    Arg.Any<MockUseCase>(),
                    Arg.Any<IMockOutputPort>(),
                    d => Task.FromResult(new UseCaseResult(true)),
                    Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(x => new UseCaseResult(true))
                .AndDoes(x => x.Arg<Func<MockUseCase, Task<UseCaseResult>>>().Invoke(x.Arg<MockUseCase>()));

            var container = new Container(c =>
            {
                c.For<IInteractor<MockUseCase, IMockOutputPort>>().Use(_useCaseInteractor);
                c.For<IMiddleware<MockUseCase, IMockOutputPort>>().Use(_middleware1);
                c.For<IMiddleware<MockUseCase, IMockOutputPort>>().Use(_middleware2);
            });

            _interactorHub = new Hub(new StructureMapResolver(container));
        }

        [Test]
        public async Task Test_StructureMap_Resolver()
        {
            await _interactorHub.Execute(new MockUseCase(), (IMockOutputPort)new MockOutputPort());
            await _useCaseInteractor.Received().Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Test_Pipeline()
        {
            await _interactorHub.Execute(new MockUseCase(), (IMockOutputPort)new MockOutputPort());
            await _middleware2.ReceivedWithAnyArgs().Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<Func<MockUseCase, Task<UseCaseResult>>>(),
                Arg.Any<CancellationToken>());
        }
    }
}
