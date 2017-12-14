using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HackerNews
{
    public class Validator
    {
        public bool Validate(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                model, context, results);

            return !results.Any();
        }
    }
}
