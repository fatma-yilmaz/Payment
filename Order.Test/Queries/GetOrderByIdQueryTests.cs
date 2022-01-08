using Autofac.Extras.Moq;
using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using Order.Api.Dal.Interfaces;
using Order.Api.Data.Queries;
using Order.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Order.Test.Queries
{
    public class GetOrderByIdQueryTests
    {
        private Fixture _fixture;
        public GetOrderByIdQueryTests()
        {
            _fixture = new Fixture();
            //this done for creating circular reference object
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Theory, AutoData]
        public async Task Succcess_Case(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var handler = mock.Create<GetOrderByIdQueryHandler>();
            var request = _fixture.Create<GetOrderByIdQuery>();
            var orderEntity = _fixture.Create<OrderEntity>();
            var mappedResponse = _fixture.Create<GetOrderByIdQueryResponse>();
            var orderId = Guid.NewGuid();

            mock.Mock<IOrderRepository>().Setup(x => x.GetById(request.Id)).ReturnsAsync(orderEntity);
            mock.Mock<IMapper>().Setup(x => x.Map<GetOrderByIdQueryResponse>(It.IsAny<OrderEntity>())).Returns(mappedResponse);

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.NotNull(response);
            Assert.True(response.Id != Guid.Empty);

            mock.Mock<IOrderRepository>().Verify(x => x.GetById(request.Id), Times.Once);
            mock.Mock<IMapper>().Verify(x => x.Map<GetOrderByIdQueryResponse>(It.IsAny<OrderEntity>()),Times.Once);
        }

        [Theory, AutoData]
        public async Task Should_Return_Null_When_Order_Not_Found(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var handler = mock.Create<GetOrderByIdQueryHandler>();
            var request = _fixture.Create<GetOrderByIdQuery>();
            var orderEntity = (OrderEntity)null;

            mock.Mock<IOrderRepository>().Setup(x => x.GetById(request.Id)).ReturnsAsync(orderEntity);
            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.Null(response);

            mock.Mock<IOrderRepository>().Verify(x => x.GetById(request.Id), Times.Once);
        }
    }
}
