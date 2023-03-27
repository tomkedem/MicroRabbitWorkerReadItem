using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domian.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;
        private readonly ITransferService _transferService;
        
        public ItemController(ILogger<ItemController> logger, ITransferService transferService)
        {
            _logger = logger;
            _transferService = transferService;
        }
        /// <summary>
        /// Get A list of all items
        /// </summary>
        //Get api/Item
        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetAllItems()
        {
            return Ok(_transferService.GetAllItems());
        }
        /// <summary>
        /// Get A list of items by IsDelete = false
        /// </summary>
        //Get api/allItem
        [HttpGet("/GetItems")]
        public ActionResult<IEnumerable<Item>> GetItems()
        {
            return Ok(_transferService.GetItems());
        }
        

    }
}