using Autofac.Extras.Moq;
using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using Payment.Api.Core.Exceptions;
using Payment.Api.Dal.Configuration;
using Payment.Api.Entities;
using Payment.Api.Services;
using Payment.Api.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Payment.Test.Services
{
    public class PaymentServiceTests
    {
        private Fixture _fixture;
        public PaymentServiceTests()
        {
            _fixture = new Fixture();
            //this done for creating circular reference object
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Theory, AutoData]
        public async Task Create_Succcess(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var unitOfWorkMock = mock.Mock<IUnitOfWork>();
            var mapperMock = mock.Mock<IMapper>();
            var paymentService = new PaymentService(unitOfWorkMock.Object, mapperMock.Object);
            var request = _fixture.Create<CreatePaymentServiceRequest>();
            var orderId = _fixture.Create<Guid>();
            var paymentId = _fixture.Create<Guid>();

            unitOfWorkMock.Setup(x => x.Orders.Create(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(orderId);
            unitOfWorkMock.Setup(x => x.Payments.Create(It.IsAny<PaymentEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(paymentId);
            unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            //Act
            var response = await paymentService.Create(request, cancellationToken);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.PaymenId, paymentId);

            unitOfWorkMock.Verify(x => x.Orders.Create(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            mock.Mock<IUnitOfWork>().Verify(x => x.Payments.Create(It.IsAny<PaymentEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            mock.Mock<IUnitOfWork>().Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetById_Succcess(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var unitOfWorkMock = mock.Mock<IUnitOfWork>();
            var mapperMock = mock.Mock<IMapper>();
            var paymentService = new PaymentService(unitOfWorkMock.Object, mapperMock.Object);
            var request = _fixture.Create<GetPaymentByIdServiceRequest>();
            var getPaymentByIdServiceResponse = _fixture.Create<GetPaymentByIdServiceResponse>();
            var paymentEntity = _fixture.Create<PaymentEntity>();
            var orderEntity = _fixture.Create<OrderEntity>();

            unitOfWorkMock.Setup(x => x.Payments.GetById(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(paymentEntity);
            mapperMock.Setup(x => x.Map<GetPaymentByIdServiceResponse>(paymentEntity)).Returns(getPaymentByIdServiceResponse);
            unitOfWorkMock.Setup(x => x.Orders.GetById(paymentEntity.OrderId, It.IsAny<CancellationToken>())).ReturnsAsync(orderEntity);

            //Act
            var response = await paymentService.GetById(request, cancellationToken);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.Order, orderEntity);

            unitOfWorkMock.Verify(x => x.Payments.GetById(request.Id, It.IsAny<CancellationToken>()), Times.Once);
            mapperMock.Verify(x => x.Map<GetPaymentByIdServiceResponse>(paymentEntity), Times.Once);
            unitOfWorkMock.Verify(x => x.Orders.GetById(paymentEntity.OrderId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetById_Should_Throw_Exception_When_Payment_Not_Found(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var request = _fixture.Create<GetPaymentByIdServiceRequest>();
            var unitOfWorkMock = mock.Mock<IUnitOfWork>();
            var mapperMock = mock.Mock<IMapper>();
            var paymentService = new PaymentService(unitOfWorkMock.Object, mapperMock.Object);
            var paymentEntity = (PaymentEntity)(null);

            unitOfWorkMock.Setup(x => x.Payments.GetById(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(paymentEntity);

            //Act            
            var exception = await Assert.ThrowsAsync<PaymentException>(() => paymentService.GetById(request, cancellationToken));

            //Assert

            unitOfWorkMock.Verify(x => x.Payments.GetById(request.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
