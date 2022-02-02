using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.Services;
using Payment.Api.Services.Models;
using System;
using System.Threading.Tasks;

namespace Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService) 
        {
            _paymentService = paymentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _paymentService.GetById(new GetPaymentByIdServiceRequest { Id = id}, new System.Threading.CancellationToken());
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePaymentServiceRequest createPaymentRequest)
        {
            var response = await _paymentService.Create(createPaymentRequest,new System.Threading.CancellationToken());
            return Ok(response);
        }

    }
}
