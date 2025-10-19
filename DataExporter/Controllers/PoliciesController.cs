using DataExporter.Dtos;
using DataExporter.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataExporter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoliciesController : ControllerBase
    {
        private PolicyService _policyService;

        public PoliciesController(PolicyService policyService) 
        { 
            _policyService = policyService;
        }

        // IActionResult is used here and I'm following suit.
        // In a real project, I'd consider and suggest using ActionResult<T> for better type safety.

        [HttpPost]
        public async Task<IActionResult> PostPolicies([FromBody]CreatePolicyDto createPolicyDto)
        {         
            var policy = await _policyService.CreatePolicyAsync(createPolicyDto);
            if (policy == null)
            {
                return BadRequest("Invalid policy data.");
            }
            return Ok(policy);
        }


        [HttpGet("AllPolicies")]
        public async Task<IActionResult> GetPolicies()
        {
            var policies = await _policyService.ReadPoliciesAsync();
            return Ok(policies);
        }

        [HttpGet("{policyId}")]
        public async Task<IActionResult> GetPolicy(int policyId)
        {
            var policy = await _policyService.ReadPolicyAsync(policyId);
            if (policy == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(policy);
            }
        }


        [HttpPost("export")]
        public async Task<IActionResult> ExportData([FromQuery]DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok();
        }
    }
}
