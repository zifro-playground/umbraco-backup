using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

// Is used to validate model properties that must be true
public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is bool && (bool)value)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(String.Format(ErrorMessageString, validationContext.DisplayName));
    }

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    {
        var rule = new ModelClientValidationRule
        {
            ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
            ValidationType = "shouldbetrue"
        };

        yield return rule;
    }
}