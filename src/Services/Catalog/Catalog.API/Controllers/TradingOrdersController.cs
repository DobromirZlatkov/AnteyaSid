namespace AnteyaSidOnContainers.Services.Catalog.API.Controllers
{
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/v1/[controller]/[action]/")]
    public class TradingOrdersController : ControllerBase
    {
        private readonly ITradingOrderService _tradingOrderService;

        public TradingOrdersController(
          ITradingOrderService tradingOrderService
          )
        {
            _tradingOrderService = tradingOrderService ?? throw new ArgumentNullException(nameof(tradingOrderService));
        }

        [HttpPost]
        public async Task<IActionResult> SyncOrders([FromBody] SyncOrdersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tradingOrderService.SyncOrders(model);

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = await _tradingOrderService.GetOrders();
            return Ok(orders);
        }
    }
}
