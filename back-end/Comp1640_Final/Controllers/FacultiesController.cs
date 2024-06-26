﻿using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultiesController : ControllerBase
    {
        public readonly IFacultyService _service;

        public FacultiesController(IFacultyService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetFalcuties()
        {
            var faculties = _service.GetFaculties();
            return Ok(faculties);
        }

		[HttpGet("contributions/by-year")]
		public  async Task<IActionResult> GetContributionsByYear()
		{
			var contributions = _service.GetContributionsByYear();
			return Ok(contributions);
		}

		[HttpGet("contributors/by-year")]
		public async Task<IActionResult> GetContributorsByYear()
		{
			var contributors = _service.GetContributorsByYear();
			return Ok(contributors);
		}
		[HttpGet("contributions/percentage")]
		public async Task<IActionResult> GetPercentageContributions()
		{
			var contributions = _service.GetPercentageContributions();
			return Ok(contributions);
		}


	}
}
