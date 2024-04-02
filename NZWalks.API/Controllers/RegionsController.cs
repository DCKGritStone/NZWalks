using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    // https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // GET ALL REGIONS
        // GET: https://localhost:poetnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            /*var regions = new List<Region>
            {
                new Region
                {
                    Id=Guid.NewGuid(),
                    Name="Auckland Region",
                    Code="AKL",
                    ReigionImageUrl="https://www.google.com/imgres?imgurl=https%3A%2F%2Fwww.newzealand.com%2Fassets%2FTourism-NZ%2FAuckland%2F98618569ff%2Fimg-1536065871-6217-4403-p-10D1D0BD-B88E-AAB3-AE3F2E903EF65717-2544003__aWxvdmVrZWxseQo_CropResizeWzE5MDAsMTAwMCw3NSwianBnIl0.jpg&tbnid=EMe48uzxTFsJnM&vet=12ahUKEwioqLH_-YGFAxU2oGMGHWhKAmgQMygFegQIARB7..i&imgrefurl=https%3A%2F%2Fwww.newzealand.com%2Fint%2Fauckland%2F&docid=DQy3NuWSTmwQxM&w=1900&h=1000&q=auckland%20region&ved=2ahUKEwioqLH_-YGFAxU2oGMGHWhKAmgQMygFegQIARB7"
                },
                new Region
                {
                    Id=Guid.NewGuid(),
                    Name="Wellington Region",
                    Code="WLG",
                    ReigionImageUrl="https://www.google.com/imgres?imgurl=https%3A%2F%2Fwww.live-work.immigration.govt.nz%2Fsites%2Fdefault%2Ffiles%2Fstyles%2Fscale_width_media_medium_%2Fpublic%2F2020-06%2FCable-car.jpg%3Fitok%3Dn2rcfKps&tbnid=ZHWlgcbJkwyVMM&vet=12ahUKEwidhrbI-oGFAxWbhGMGHUCvC1cQMygCegQIARB1..i&imgrefurl=https%3A%2F%2Fwww.live-work.immigration.govt.nz%2Fchoose-new-zealand%2Fregions-cities%2Fwellington&docid=7hMisUaurZ4mkM&w=804&h=453&q=wellington%20region&ved=2ahUKEwidhrbI-oGFAxWbhGMGHUCvC1cQMygCegQIARB1"
                }

            };*/

            //Get Data From Database - Domain models
           var regionsDomain= dbContext.Regions.ToList();

            //Map Domain Models to DTOs

            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    ReigionImageUrl = regionDomain.RegionImageUrl
                }); 

            }
            //Return DTOs
            return Ok(regionsDto);
        }

        // GET SINGLE REGION (Get Region By ID)
        // GET: https://localhost:poetnumber/api/regions/{id}
        [HttpGet]

        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id) 
        {
            //  var region = dbContext.Regions.Find(id);

            //Get Region Domain Model From Database
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id );

            if (regionDomain== null)
            {
                return NotFound();
            }
            //Map/Convert Region Domain Model to Region DTO
          var  regionsDto=new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                ReigionImageUrl = regionDomain.RegionImageUrl
            };

            //Return DTO back to client

            return Ok(regionDomain);
        }

        //POST to create new region
        //POST : https://loaclhost:portnumber/api/regions

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map/Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.ReigionImageUrl
            };

            //Use Domain Model to create Region
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                ReigionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new {id=regionDomainModel.Id},regionDomainModel);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, UpdateRegionRequestDto updateRegionRequestDto)
        {

            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();  

            }

            regionDomain.Code = updateRegionRequestDto.Code;
            regionDomain.Name = updateRegionRequestDto.Name;
            regionDomain.RegionImageUrl = updateRegionRequestDto.ReigionImageUrl;

            dbContext.SaveChanges();

            return Ok(regionDomain);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {

            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();

            }
            dbContext.Remove(regionDomain);
            dbContext.SaveChanges();

            return Ok(regionDomain);
        }
    }
}
