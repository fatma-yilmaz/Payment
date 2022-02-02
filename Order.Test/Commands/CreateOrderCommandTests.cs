//using Autofac.Extras.Moq;
//using AutoFixture;
//using AutoFixture.Xunit2;
//using Moq;
//using Order.Api.Dal.Interfaces;
//using Order.Api.Data.Commands;
//using Order.Api.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace Order.Test.Commands
//{
//    public class CreateOrderCommandTests
//    {
//        private Fixture _fixture;
//        public CreateOrderCommandTests()
//        {
//            _fixture = new Fixture();
//            //this done for creating circular reference object
//            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
//                .ForEach(b => _fixture.Behaviors.Remove(b));
//            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
//        }

//        [Theory, AutoData]
//        public async Task Succcess_Case(CancellationToken cancellationToken)
//        {
//            //Arrange
//            using var mock = AutoMock.GetStrict();
//            var handler = mock.Create<CreateOrderCommandHandler>();
//            var request = _fixture.Create<CreateOrderCommand>();
//            var orderId = Guid.NewGuid();

//            mock.Mock<IOrderRepository>().Setup(x => x.Create(It.IsAny<OrderEntity>())).ReturnsAsync(orderId);

//            //Act
//            var response = await handler.Handle(request, cancellationToken);

//            //Asset
//            Assert.NotNull(response);
//            Assert.True(response.IsSuccess);
//            Assert.Equal(response.OrderId, orderId);

//            mock.Mock<IOrderRepository>().Verify(x => x.Create(It.IsAny<OrderEntity>()), Times.Once);
//        }
//    }
//}
