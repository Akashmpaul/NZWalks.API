using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //WITHOUT REPOSITORY PATTERN
    public class RegionsController : ControllerBase
    {
       private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public IMapper Mapper { get; }

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
           this.dbContext = dbContext;
           this.regionRepository = regionRepository;
            Mapper = mapper;
        }

       //GET ALL REGIONS
       [HttpGet]
        [Authorize]
       public async Task<IActionResult> GetAll()
       {
          
            //GET DATA FROM DATABASE - DOMAIN MODELS
            var regionsDomain = await regionRepository.GetAllAsync();

            //MAP DOMAIN MODELS TO DTOS

            var regionDto = Mapper.Map<List<RegionDto>>(regionsDomain);


            //RETURN DTOs
            return Ok(regionDto);

       }

       //GET REGIONS BY ID OR GET SINGLE REGION

       [HttpGet]
       [Route("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
       {
           //GET REGION DOMAIN MODEL FROM DATABASE
           var regionDomain = await regionRepository.GetByIdAsync(id);
           if (regionDomain == null)
           {
               return NotFound();
           }

            //MAP/CONVERT REGION MODEL TO REGION DTO
            var regionDto = Mapper.Map<RegionDto>(regionDomain);

            //RETURN DTO
            return Ok(regionDto);
       }

       [HttpPost]
       [ValidateModel]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] AddRegionDto addRegionDto)
       {
           
                //Map or convert dto to domain model
                var regionDomainModel = Mapper.Map<Region>(addRegionDto);

                //use domain model to create region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);


                //Map domain model back to dto
                var regionDto = Mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
         
       }

       //Update region
       //PUT : 

       [HttpPut]
       [Route("{id:guid}")]
       [ValidateModel]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto) 
       {
           
                //Map dto to domain model
                var regionDomainModel = Mapper.Map<Region>(updateRegionDto);

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //convert domain model to dto

                var regionDto = Mapper.Map<RegionDto>(regionDomainModel);

                return Ok(regionDto);
       }

       //Delete region
       [HttpDelete]
       [Route("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
       {
           //Check id id exists
           var regionDomainModel = await regionRepository.DeleteAsync(id);
           if(regionDomainModel == null) 
           {
               return NotFound();
           }

            //Map or convert region domain model to dto
            var regionDto = Mapper.Map<RegionDto>(regionDomainModel);

           return Ok(regionDto);
       }
    }
}
