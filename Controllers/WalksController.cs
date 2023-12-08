using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //Create Walk
        //POST

        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] AddWalkDto addWalkDto)
        {
            
                //Map dto to domaion model
                var walkDomainModel = mapper.Map<Walk>(addWalkDto);

                await walkRepository.CreateAsync(walkDomainModel);

                //Map domain model to dto
                var walkDto = mapper.Map<WalkDto>(walkDomainModel);
                return Ok(walkDto);
        }

        //Get all walks
        //GET

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending)
        {

            //Get data from database
            var walkDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true);

            //Map domain model to dto
            var walkDto = mapper.Map<List<WalkDto>> (walkDomainModel);
            return Ok(walkDto);
        }

        //Get single walk
        [HttpGet]
        [Route("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDoaminModel = await walkRepository.GetByIdAsync(id);
            if(walkDoaminModel == null)
            {
                return NotFound();
            }
            //Map domain model to dto
            var walkDto = mapper.Map<WalkDto> (walkDoaminModel);
            return Ok(walkDto);
        }

        //Update walk
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            
                //Map updateWalkdto to domain model

                var walkDomainModel = mapper.Map<Walk>(updateWalkDto);

                walkDomainModel = await walkRepository.UpdateWalkAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                //Map domain model to DTO
                var walkDto = mapper.Map<WalkDto>(walkDomainModel);
                return Ok(walkDto);
        }

        //Delete Walk
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var existingWalk = await walkRepository.DeleteWalkAsync(id);
            if(existingWalk == null)
            {
                return NotFound();
            }

            //Map domain model to dto
            var walkDto = mapper.Map<WalkDto>(existingWalk);
            return Ok(walkDto);
        }
    }
}
