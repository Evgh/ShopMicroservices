using Api.Contracts.Requests;
using Api.Contracts.Responces;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        IMapper _mapper;
        IShopService _shopService;

        public ShopsController(IMapper mapper, IShopService shopService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shopService = shopService ?? throw new ArgumentNullException(nameof(shopService));
        }

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] int page = 0)
        {
            List<ShopEntity> data = await _shopService.GetShops(page);
            List<ShopResponce> mappedData = data.Select(element => _mapper.Map<ShopResponce>(element)).ToList();

            return Ok(mappedData);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            ShopEntity shop = await _shopService.GetShopById(id);

            if(shop != null)
                return Ok(_mapper.Map<ShopResponce>(shop));

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateShopRequest shopRequest)
        {
            if (ModelState.IsValid)
            {
                ShopEntity shopToCreate = _mapper.Map<ShopEntity>(shopRequest);
                ShopEntity createdShop = await _shopService.Create(shopToCreate);

                return Created(createdShop.Id, _mapper.Map<ShopResponce>(createdShop)); 
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] ShopRequest shopRequest)
        {
            if (ModelState.IsValid)
            {
                ShopEntity shopToUpdate = _mapper.Map<ShopEntity>(shopRequest);

                if (await _shopService.Update(shopToUpdate))
                    return NoContent();

                return NotFound();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (await _shopService.Delete(id))
                return NoContent();

            return NotFound();
        }
    }
}
