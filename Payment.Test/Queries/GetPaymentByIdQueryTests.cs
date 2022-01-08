using Autofac.Extras.Moq;
using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using Payment.Api.Core.Validators;
using Payment.Api.Dal.Interfaces;
using Payment.Api.Data.HttpClients;
using Payment.Api.Data.Queries;
using Payment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Payment.Test.Queries
{
    public class GetPaymentByIdQueryTests
    {
        private Fixture _fixture;
        public GetPaymentByIdQueryTests()
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
            var handler = mock.Create<GetPaymentByIdQueryHandler>();
            var request = _fixture.Create<GetPaymentByIdQuery>();
            var paymentEntity = _fixture.Create<PaymentEntity>();
            var orderHttpClientResponse = _fixture.Create<GetOrderResponse>();
            var orderDto = _fixture.Create<OrderDto>();
            var queryResponse = _fixture.Create<GetPaymentByIdQueryResponse>();
            queryResponse.Order = orderDto;
            paymentEntity.Id = request.Id;

            mock.Mock<IPaymentRepository>().Setup(x => x.GetById(request.Id)).ReturnsAsync(paymentEntity);
            mock.Mock<OrderHttpClient>().Setup(x => x.GetOrder(paymentEntity.OrderId,cancellationToken)).ReturnsAsync(orderHttpClientResponse);
            mock.Mock<IMapper>().Setup(x => x.Map<OrderDto>(It.IsAny<GetOrderResponse>())).Returns(orderDto);
            mock.Mock<IMapper>().Setup(x => x.Map<GetPaymentByIdQueryResponse>(It.IsAny<PaymentEntity>())).Returns(queryResponse);

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.NotNull(response);
            Assert.Equal(response.Order, orderDto);

            mock.Mock<IPaymentRepository>().Verify(x => x.GetById(request.Id),Times.Once);
            mock.Mock<OrderHttpClient>().Verify(x => x.GetOrder(paymentEntity.OrderId, cancellationToken), Times.Once);
            mock.Mock<IMapper>().Verify(x => x.Map<OrderDto>(It.IsAny<GetOrderResponse>()),Times.Once);
            mock.Mock<IMapper>().Verify(x => x.Map<GetPaymentByIdQueryResponse>(It.IsAny<PaymentEntity>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task Should_Return_Null_When_Payment_Not_Found(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var handler = mock.Create<GetPaymentByIdQueryHandler>();
            var request = _fixture.Create<GetPaymentByIdQuery>();
            var paymentEntity = (PaymentEntity)(null);

            mock.Mock<IPaymentRepository>().Setup(x => x.GetById(request.Id)).ReturnsAsync(paymentEntity);

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.Null(response);

            mock.Mock<IPaymentRepository>().Verify(x => x.GetById(request.Id), Times.Once);
        }

        [Theory, AutoData]
        public async Task Order_Should_Be_Empty_When_OrderId_Is_Null(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var handler = mock.Create<GetPaymentByIdQueryHandler>();
            var request = _fixture.Create<GetPaymentByIdQuery>();
            var order = new OrderDto { };
            var queryResponse = _fixture.Create<GetPaymentByIdQueryResponse>();
            var paymentEntity = _fixture.Create<PaymentEntity>();
            paymentEntity.OrderId = null;

            mock.Mock<IPaymentRepository>().Setup(x => x.GetById(request.Id)).ReturnsAsync(paymentEntity);
            mock.Mock<IMapper>().Setup(x => x.Map<GetPaymentByIdQueryResponse>(It.IsAny<PaymentEntity>())).Returns(queryResponse);

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.NotNull(response);
            Assert.Equal(response.Order.Id,order.Id);
            Assert.Equal(response.Order.ConsumerFullName, order.ConsumerFullName);
            Assert.Equal(response.Order.ConsumerAddress, order.ConsumerAddress);

            mock.Mock<IPaymentRepository>().Verify(x => x.GetById(request.Id), Times.Once);
            mock.Mock<IMapper>().Verify(x => x.Map<GetPaymentByIdQueryResponse>(It.IsAny<PaymentEntity>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task Order_Should_Be_Empty_When_OrderId_Is_Empty_Guid(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var handler = mock.Create<GetPaymentByIdQueryHandler>();
            var request = _fixture.Create<GetPaymentByIdQuery>();
            var order = new OrderDto { };
            var queryResponse = _fixture.Create<GetPaymentByIdQueryResponse>();
            var paymentEntity = _fixture.Create<PaymentEntity>();
            paymentEntity.OrderId = Guid.Empty;

            mock.Mock<IPaymentRepository>().Setup(x => x.GetById(request.Id)).ReturnsAsync(paymentEntity);
            mock.Mock<IMapper>().Setup(x => x.Map<GetPaymentByIdQueryResponse>(It.IsAny<PaymentEntity>())).Returns(queryResponse);

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.NotNull(response);
            Assert.Equal(response.Order.Id, order.Id);
            Assert.Equal(response.Order.ConsumerFullName, order.ConsumerFullName);
            Assert.Equal(response.Order.ConsumerAddress, order.ConsumerAddress);

            mock.Mock<IPaymentRepository>().Verify(x => x.GetById(request.Id), Times.Once);
            mock.Mock<IMapper>().Verify(x => x.Map<GetPaymentByIdQueryResponse>(It.IsAny<PaymentEntity>()), Times.Once);
        }
    }
}
