using DataExporter.Services;
using FluentValidation;
using System.Globalization;

public class PostPoliciesValidator : AbstractValidator<DataExporter.Dtos.CreatePolicyDto>
{
    public PostPoliciesValidator()
    {
        RuleFor(policy => policy.PolicyNumber)
            .NotEmpty()
            .MaximumLength(20);
        RuleFor(policy => policy.Premium)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(policy => policy.StartDate)
            .NotEmpty()
            .Must(s => DateTime.TryParseExact(
                s,
                PolicyServiceHelper.DateFormat, 
                CultureInfo.InvariantCulture, // We don't need to deal with any cultures; this is a fixed format.
                DateTimeStyles.None,
                out _)); //And discard. We just want to validate.
    }
}