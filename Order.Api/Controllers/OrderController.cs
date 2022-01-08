using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Api.Data.Commands;
using Order.Api.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetOrderByIdQuery(id);
            var response = (await _mediator.Send(query));
            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderCommand requestModel)
        {
            var response = await _mediator.Send(requestModel);
            return Ok(response);
        }
    }
}
