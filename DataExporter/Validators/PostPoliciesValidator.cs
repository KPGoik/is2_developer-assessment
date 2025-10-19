using FluentValidation;

public class PostPoliciesValidator : AbstractValidator<DataExporter.Dtos.CreatePolicyDto>
{
    public PostPoliciesValidator()
    {
        RuleFor(policy => policy.PolicyNumber)
            .NotEmpty()
            .MaximumLength(20);
        RuleFor(policy => policy.Premium)
            .GreaterThan(0);
            
    }
}