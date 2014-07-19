using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using System.Configuration;

namespace Windcheater.Dependency
{
    using Services;

    public class WindcheaterNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<StravaService>()
                .ToSelf()
                .WithConstructorArgument("stravaApiKey", ConfigurationManager.AppSettings["StravaApiKey"]);

            Bind<IWeatherService>()
                .To<MetOfficeWeatherService>()
                .WithConstructorArgument("azureDataMarketUsername", ConfigurationManager.AppSettings["AzureDataMarketUsername"])
                .WithConstructorArgument("azureDataMarketApiKey", ConfigurationManager.AppSettings["AzureDataMarketApiKey"]);

        }
    }
}
