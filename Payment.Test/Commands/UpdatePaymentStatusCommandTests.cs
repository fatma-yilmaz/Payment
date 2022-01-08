using Autofac.Extras.Moq;
using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using Payment.Api.Dal.Interfaces;
using Payment.Api.Data.Commands;
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
    public class UpdatePaymentStatusCommandTests
    {
        private Fixture _fixture;
        public UpdatePaymentStatusCommandTests()
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
            var handler = mock.Create<UpdatePaymentStatusCommandHandler>();
            var request = _fixture.Create<UpdatePaymentStatusCommand>();
            var paymentEntity = _fixture.Create<PaymentEntity>();

            mock.Mock<IPaymentRepository>().Setup(x => x.GetById(request.PaymentId)).ReturnsAsync(paymentEntity);
            mock.Mock<IPaymentRepository>().Setup(x => x.Update(It.IsAny<PaymentEntity>())).ReturnsAsync(true);

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);

            mock.Mock<IPaymentRepository>().Verify(x => x.GetById(request.PaymentId), Times.Once);
            mock.Mock<IPaymentRepository>().Verify(x => x.Update(It.IsAny<PaymentEntity>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task Should_Return_IsSuccess_False_When_Payment_NotFound(CancellationToken cancellationToken)
        {
            //Arrange
            using var mock = AutoMock.GetStrict();
            var handler = mock.Create<UpdatePaymentStatusCommandHandler>();
            var request = _fixture.Create<UpdatePaymentStatusCommand>();
            var paymentEntity = (PaymentEntity)null;

            mock.Mock<IPaymentRepository>().Setup(x => x.GetById(request.PaymentId)).ReturnsAsync(paymentEntity);

            //Act
            var response = await handler.Handle(request, cancellationToken);

            //Asset
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Message, "payment not found".ToString());

            mock.Mock<IPaymentRepository>().Verify(x => x.GetById(request.PaymentId), Times.Once);
        }
    }
}
