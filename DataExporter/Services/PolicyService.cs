using DataExporter.Dtos;
using DataExporter.Model;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System.Globalization;


namespace DataExporter.Services
{
    public class PolicyService
    {
        private ExporterDbContext _dbContext;
        private IValidator<CreatePolicyDto> _validator;

        public PolicyService(ExporterDbContext dbContext, IValidator<CreatePolicyDto> validator)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
            _validator = validator;
        }

        /// <summary>
        /// Creates a new policy from the DTO.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>Returns a ReadPolicyDto representing the new policy, if succeded. Returns null, otherwise.</returns>
        public async Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto)
        {
            var results = await _validator.ValidateAsync(createPolicyDto);

            if (!results.IsValid)
            {
                return null;
            }

            var entity = new Policy()
            {
                //We can tell the PostPoliciesValidator already checked for nulls and format, so we can use the ! operator here.
                PolicyNumber = createPolicyDto.PolicyNumber!,
                Premium = createPolicyDto.Premium,
                StartDate = DateTime.ParseExact(createPolicyDto.StartDate!, PolicyServiceHelper.DateFormat, CultureInfo.InvariantCulture) //We don't need to use TryParse here because the validator already checked the format.
            };

            _dbContext.Policies.Add(entity);

            try 
            { 
                await _dbContext.SaveChangesAsync();
            }

            catch (Exception)
            {
                return null; //As discussed in the controller, this is horrible error handling. In a real project, I'd implement proper error handling based on the exception type.
            }
            

            return new ReadPolicyDto()
            {
                Id = entity.Id,
                PolicyNumber = entity.PolicyNumber,
                Premium = entity.Premium,
                StartDate = entity.StartDate
            };
        }

        /// <summary>
        /// Retrives all policies.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a list of ReadPoliciesDto.</returns>
        public async Task<IList<ReadPolicyDto>> ReadPoliciesAsync()
        {
            return await _dbContext.Policies.Select(p => new ReadPolicyDto
            {
                Id = p.Id,
                PolicyNumber = p.PolicyNumber,
                Premium = p.Premium,
                StartDate = p.StartDate
            }).ToListAsync();
        }

        /// <summary>
        /// Retrieves a policy by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a ReadPolicyDto.</returns>
        public async Task<ReadPolicyDto?> ReadPolicyAsync(int id)
        {
            // If SingleAsync doesn't find an entity, it throws an exception. FindAsync returns null instead, which maps cleanly to 404.
            // Worth mentioning that FindAsync is for PK only, for non-pk lookups, use FirstOrDefaultAsync.
            var policy = await _dbContext.Policies.FindAsync(id); 
            if (policy == null)
            {
                return null;
            }

            var policyDto = new ReadPolicyDto()
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate
            };

            return policyDto;
        }

        /// <summary>
        /// Exports policies within a date range.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Returns a list of ExportDto within the given time range.</returns>

        public async Task<IList<ExportDto>> ExportPoliciesAsync(DateTime startDate, DateTime endDate)
        {
                return await _dbContext.Policies
                    .Where(p => p.StartDate >= startDate && p.StartDate <= endDate)
                    .Select(policy => new ExportDto
                    {
                        PolicyNumber = policy.PolicyNumber,
                        Premium = policy.Premium,
                        StartDate = policy.StartDate,
                        Notes = policy.Notes.Select(note => note.Text).ToList()
                    }).ToListAsync();

            //For a larger results set, would be worth to add pagination here.
            }
    }
}
