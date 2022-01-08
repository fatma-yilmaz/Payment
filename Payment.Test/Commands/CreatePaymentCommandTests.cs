using Autofac.Extras.Moq;
using AutoFixture;
using AutoFixture.Xunit2;
using MediatR;
using Moq;
using Payment.Api.Dal.Interfaces;
using Payment.Api.Data.Commands;
using Payment.Api.Data.HttpClients;
using Payment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Payment.Test.Commands
{
    public class CreatePaymentCommandTests
    {
        private Fixture _fixture;
        public CreatePaymentCommandTests()
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
            var handler = mock.Create<CreatePaymentCommandHandler>();
            var request = _fixture.Create<CreatePaymentCommand>();
            var paymentEntity = new PaymentEntity()
                                {
                                    Amount = request.Amount,
                                    CurrencyCode = request.CurrencyCode,
                                    Status = "Created",
                                    CreationDate = DateTime.Now
            };
            var paymentId = Guid.NewGuid();
            var orderHttpClientResponse = new CreateOrderResponse { isSuccess = true, orderId = Guid.NewGuid()};


            mock.Mock<IPaymentRepository>().Setup(x => x.Create(It.IsAny<PaymentEntity>())).ReturnsAsync(paymentId);
            mock.Mock<OrderHttpClient>().Setup(x => x.CreateOrder(request.Order.ConsumerFullName, request.Order.ConsumerAddress, cancellationToken)).ReturnsAsync(orderHttpClientResponse);
            mock.Mock<IMediator>()
                    .Setup(x => x.Send(It.Is<UpdatePaymentOrderStatusCommand>(m => m.PaymentId == paymentId && m.OrderId == orderHttpClientResponse.orderId), cancellationToken))
                    .ReturnsAsync(new UpdatePaymentOrderStatusCommandResponse { IsSuccess=true ,Message=""});

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.Equal(response.PaymenId, paymentId);

            mock.Mock<IPaymentRepository>().Verify(x => x.Create(It.IsAny<PaymentEntity>()), Times.Once);
            mock.Mock<OrderHttpClient>().Verify(x => x.CreateOrder(request.Order.ConsumerFullName, request.Order.ConsumerAddress, cancellationToken),Times.Once);
            mock.Mock<IMediator>().Verify(x => x.Send(It.Is<UpdatePaymentOrderStatusCommand>(m => m.PaymentId == paymentId && m.OrderId == orderHttpClientResponse.orderId), cancellationToken),Times.Once);
        }

        [Theory, AutoData]
        public async Task Should_Return_IsSuccess_False_When_UpdatePaymentResponse_Is_False(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var handler = mock.Create<CreatePaymentCommandHandler>();
            var request = _fixture.Create<CreatePaymentCommand>();
            var paymentEntity = new PaymentEntity()
            {
                Amount = request.Amount,
                CurrencyCode = request.CurrencyCode,
                Status = "Created",
                CreationDate = DateTime.Now
            };
            var paymentId = Guid.NewGuid();
            var orderHttpClientResponse = new CreateOrderResponse { isSuccess = true, orderId = Guid.NewGuid() };


            mock.Mock<IPaymentRepository>().Setup(x => x.Create(It.IsAny<PaymentEntity>())).ReturnsAsync(paymentId);
            mock.Mock<OrderHttpClient>().Setup(x => x.CreateOrder(request.Order.ConsumerFullName, request.Order.ConsumerAddress, cancellationToken)).ReturnsAsync(orderHttpClientResponse);
            mock.Mock<IMediator>()
                    .Setup(x => x.Send(It.Is<UpdatePaymentOrderStatusCommand>(m => m.PaymentId == paymentId && m.OrderId == orderHttpClientResponse.orderId), cancellationToken))
                    .ReturnsAsync(new UpdatePaymentOrderStatusCommandResponse { IsSuccess = false, Message = "" });

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);

            mock.Mock<IPaymentRepository>().Verify(x => x.Create(It.IsAny<PaymentEntity>()), Times.Once);
            mock.Mock<OrderHttpClient>().Verify(x => x.CreateOrder(request.Order.ConsumerFullName, request.Order.ConsumerAddress, cancellationToken), Times.Once);
            mock.Mock<IMediator>().Verify(x => x.Send(It.Is<UpdatePaymentOrderStatusCommand>(m => m.PaymentId == paymentId && m.OrderId == orderHttpClientResponse.orderId), cancellationToken), Times.Once);
        }
        [Theory, AutoData]
        public async Task Should_Return_IsSuccess_False_When_CreateOrderResponse_Is_False(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var handler = mock.Create<CreatePaymentCommandHandler>();
            var request = _fixture.Create<CreatePaymentCommand>();
            var paymentEntity = new PaymentEntity()
            {
                Amount = request.Amount,
                CurrencyCode = request.CurrencyCode,
                Status = "Created",
                CreationDate = DateTime.Now
            };
            var paymentId = Guid.NewGuid();
            var orderHttpClientResponse = new CreateOrderResponse { isSuccess = false, orderId = Guid.Empty };


            mock.Mock<IPaymentRepository>().Setup(x => x.Create(It.IsAny<PaymentEntity>())).ReturnsAsync(paymentId);
            mock.Mock<OrderHttpClient>().Setup(x => x.CreateOrder(request.Order.ConsumerFullName, request.Order.ConsumerAddress, cancellationToken)).ReturnsAsync(orderHttpClientResponse);
            mock.Mock<IMediator>()
                    .Setup(x => x.Send(It.Is<UpdatePaymentStatusCommand>(m => m.PaymentId == paymentId && m.Status == "Failed"), cancellationToken))
                    .ReturnsAsync(new UpdatePaymentStatusCommandResponse { IsSuccess = true, Message = "" });

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);

            mock.Mock<IPaymentRepository>().Verify(x => x.Create(It.IsAny<PaymentEntity>()), Times.Once);
            mock.Mock<OrderHttpClient>().Verify(x => x.CreateOrder(request.Order.ConsumerFullName, request.Order.ConsumerAddress, cancellationToken), Times.Once);
            mock.Mock<IMediator>().Verify(x => x.Send(It.Is<UpdatePaymentStatusCommand>(m => m.PaymentId == paymentId && m.Status == "Failed"), cancellationToken), Times.Once);
        }

    }
}
