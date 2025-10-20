using DataExporter.Dtos;
using DataExporter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
                // I HATE this. If I were to work on this further, I'd use proper error handling and return specific errors based on what went wrong in the service.
                // Especially because it might not necessarily be a bad request, something could go wrong on our end.
                return BadRequest("Could not create policy."); 
            }
            return CreatedAtAction(nameof(GetPolicy), new { policyId = policy.Id }, policy); //201 instead of 200 for creation.
        }


        [HttpGet("")]
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

            return Ok(policy);
        }


        [HttpGet("export")] // Get instead of post because there's no body.
        public async Task<IActionResult> ExportData([FromQuery, BindRequired]DateTime startDate, [FromQuery, BindRequired] DateTime endDate)
        {
            if(endDate< startDate)
            {
                return BadRequest("End date must be greater than or equal to start date.");
            }

            var exportData = await _policyService.ExportPoliciesAsync(startDate, endDate);
            return Ok(exportData);
        }
    }
}
