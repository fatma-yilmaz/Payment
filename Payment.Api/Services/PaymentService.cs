using AutoMapper;
using Payment.Api.Core.Enums;
using Payment.Api.Core.Exceptions;
using Payment.Api.Dal.Configuration;
using Payment.Api.Entities;
using Payment.Api.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CreatePaymentServiceResponse> Create(CreatePaymentServiceRequest request, CancellationToken cancellationToken)
        {
            var orderId = await _unitOfWork.Orders.Create(new OrderEntity() { ConsumerFullName = request.Order.ConsumerFullName, ConsumerAddress = request.Order.ConsumerAddress }, cancellationToken);
            var paymentId = await _unitOfWork.Payments.Create(new PaymentEntity()
            {
                Amount = request.Amount,
                CurrencyCode = request.CurrencyCode,
                Status = (int)PaymentStatus.Completed,
                CreationDate = DateTime.Now,
                OrderId = orderId
            }, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreatePaymentServiceResponse
            {
                PaymenId = paymentId
            };
        }

        public async Task<GetPaymentByIdServiceResponse> GetById(GetPaymentByIdServiceRequest request, CancellationToken cancellationToken)
        {
            var payment = await _unitOfWork.Payments.GetById(request.Id, cancellationToken);
            if (payment == null) throw new PaymentException("Payment Not Found!", System.Net.HttpStatusCode.NotFound);

            var response = _mapper.Map<GetPaymentByIdServiceResponse>(payment);
            response.Order = await _unitOfWork.Orders.GetById(payment.OrderId, cancellationToken);

            return response;
        }
    }
}
