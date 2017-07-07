using System;
using System.Web;
using Microsoft.Practices.Unity;
using Accommodation.Commons;
using Accommodation.Interfaces;
using Accommodation.Interfaces.Maps;
using Accommodation.Interfaces.Repositories;
using Accommodation.Interfaces.Services;
using Accommodation.Maps;
using Accommodation.Models.Context;
using Accommodation.Models.Interfaces;
using Accommodation.Repositories;
using Accommodation.Service;
using Unity.WebApi;
using Accommodation.Services;
using Alexandra.Core.Business;

namespace Accommodation.Unity.Injection
{

    public static class UnityConfig
    {

        private static UnityContainer container;

        public static UnityContainer Container { get { return container; } }

        public static UnityDependencyResolver RegisterComponents()
        {
            try
            {
                container = new UnityContainer();

                container.RegisterType<IAccommodationContext, AccommodationContext>();

                #region Ticket Proxies
                //container.RegisterType<ITicketProxy, EmployeeVacationRequestTicketProxy>(typeof(EmployeeVacationRequestTicketProxy).Name);
                //container.RegisterType<ITicketProxy, BillingReportTicketProxy>(typeof(BillingReportTicketProxy).Name);
                #endregion

                #region Maps
                container.RegisterType<ILodgingMap, LodgingMap>();
                container.RegisterType<IServiceMap, ServiceMap>();
                container.RegisterType<ITourismMap, TourismMap>();
                container.RegisterType<ILocationMap, LocationMap>();
                container.RegisterType<IPhotoMap, PhotoMap>();
                container.RegisterType <IAdvertisingMap, AdvertisingMap>();
                #endregion

                #region Services
                container.RegisterType<ILodgingService, LodgingService>();
                container.RegisterType<ILocationService, LocationService>();
                container.RegisterType<IEmailService, EmailService>();
                container.RegisterType<IServiceService, ServiceService>();
                container.RegisterType<ITourismService, TourismService>();
                container.RegisterType <IPhotoService, PhotoService>();
                container.RegisterType<IAdvertisingService, AdvertisingService>();
                container.RegisterType<ICurrentUserService, CurrentUserService>();

                #endregion

                #region Repositories
                container.RegisterType<ILodgingRepository, LodgingRepository>();
                container.RegisterType<IServiceRepository, ServiceRepository>();
                container.RegisterType<ITourismRepository, TourismRepository>();
                container.RegisterType<ILocationRepository, LocationRepository>();
                container.RegisterType<IAdvertisingRepository, AdvertisingRepository>();
                container.RegisterType<IPhotoRepository, PhotoRepository>();
                #endregion

                return new UnityDependencyResolver(container);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public static T ResolveDependency<T>()
        {
            try
            {
                if (container == null)
                {
                    container = new UnityContainer();
                    RegisterComponents();
                }
                return container.Resolve<T>();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

    }
}
