using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.XPath;

namespace Cyh.WebServices.AppStartup
{
    public static partial class CommonExtends
    {
        /// <summary>
        /// Inject human-friendly descriptions for Operations, Parameters and Schemas based on XML Comment files
        /// </summary>
        /// <param name="swaggerGenOptions"></param>
        /// <param name="xmlDocFactory">A factory method that returns XML Comments as an XPathDocument</param>
        /// <param name="includeControllerXmlComments">
        /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
        /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
        /// </param>
        public static void MyIncludeXmlComments(
            this SwaggerGenOptions swaggerGenOptions,
            IEnumerable<string> xmlPaths,
            bool includeControllerXmlComments = false) {
            if (xmlPaths.IsNullOrEmpty())
                return;

            foreach (string xmlPath in xmlPaths) {
                if (File.Exists(xmlPath))
                    swaggerGenOptions.IncludeXmlComments(xmlPath, includeControllerXmlComments);
            }
        }
    }
}
