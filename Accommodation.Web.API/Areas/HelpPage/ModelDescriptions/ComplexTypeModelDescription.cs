using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Accommodation.Web.API.Areas.HelpPage.ModelDescriptions
{
    [ExcludeFromCodeCoverage]
    public class ComplexTypeModelDescription : ModelDescription
    {
        public ComplexTypeModelDescription()
        {
            Properties = new Collection<ParameterDescription>();
        }

        public Collection<ParameterDescription> Properties { get; private set; }
    }
}