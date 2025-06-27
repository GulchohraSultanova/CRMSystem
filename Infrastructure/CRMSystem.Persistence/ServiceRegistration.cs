using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Persistence.Concreters.Services;
using CRMSystem.Infrastructure.Concreters.Services;

using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.Vendors;
using CRMSystem.Application.Absrtacts.Repositories.Categories;
using CRMSystem.Application.Absrtacts.Repositories.Companies;
using CRMSystem.Application.Absrtacts.Repositories.Departments;
using CRMSystem.Application.Absrtacts.Repositories.Jobs;
using CRMSystem.Application.Absrtacts.Repositories.Orders;
using CRMSystem.Application.Absrtacts.Repositories.OrderItems;
using CRMSystem.Application.Absrtacts.Repositories.OrderOverheads;
using CRMSystem.Application.Absrtacts.Repositories.Products;
using CRMSystem.Application.Absrtacts.Repositories.Sections;

using CRMSystem.Persistence.Concreters.Repositories.Vendors;
using CRMSystem.Persistence.Concreters.Repositories.Categories;
using CRMSystem.Persistence.Concreters.Repositories.Companies;
using CRMSystem.Persistence.Concreters.Repositories.Departments;
using CRMSystem.Persistence.Concreters.Repositories.Jobs;
using CRMSystem.Persistence.Concreters.Repositories.Orders;
using CRMSystem.Persistence.Concreters.Repositories.OrderItems;
using CRMSystem.Persistence.Concreters.Repositories.OrderOverheads;
using CRMSystem.Persistence.Concreters.Repositories.Products;
using CRMSystem.Persistence.Concreters.Repositories.Sections;
using CRMSystem.Application.Absrtacts.Repositories.PendingCategories;
using CRMSystem.Persistence.Concreters.Repositories.PendingCategories;
using CRMSystem.Persistence.Concreters.Repositories.PendingProducts;
using CRMSystem.Application.Absrtacts.Repositories.PendingProducts;
using CRMSystem.Application.Absrtacts.Repositories.AdminNotifications;
using CRMSystem.Persistence.Concreters.Repositories.AdminNotifications;
using CRMSystem.Application.Absrtacts.Repositories.SectionCustomers;
using CRMSystem.Persistence.Concreters.Repositories.SectionCustomers;
using CRMSystem.Application.Absrtacts.Repositories.Calculator;
using CRMSystem.Persistence.Concreters.Repositories.Calculator;

namespace CRMSystem.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IFighterService, FighterService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAdminNotificationService, AdminNotificationService>();
            services.AddScoped<IVendorService, VendorService>();

            services.AddScoped<IVendorReadRepository, VendorReadRepository>();
            services.AddScoped<IVendorWriteRepository, VendorWriteRepository>();
            services.AddScoped<ISectionCustomerReadRepository, SectionCustomerReadRepository>();
            services.AddScoped<ISectionCustomerWriteRepository, SectionCustomerWriteRepository>();
            services.AddScoped<IAdminNotificationReadRepository,AdminNotificationReadRepository>();
            services.AddScoped<IAdminNotificationWriteRepository,AdminNotificationWriteRepository>();

            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

            services.AddScoped<ICompanyReadRepository, CompanyReadRepository>();
            services.AddScoped<ICompanyWriteRepository, CompanyWriteRepository>();

            services.AddScoped<IMonthlyCalculationReadRepository,MonthlyCalculationReadRepository>();
            services.AddScoped<IMonthlyCalculationWriteRepository, MonthlyCalculationWriteRepository>();
            services.AddScoped<IMonthlyCalculationService, MonthlyCalculationService>();



            services.AddScoped<IDepartmentReadRepository, DepartmentReadRepository>();
            services.AddScoped<IDepartmentWriteRepository, DepartmentWriteRepository>();
            services.AddScoped<IPendingCategoryReadRepository, PendingCategoryReadRepository>();
            services.AddScoped<IPendingCategoryWriteRepository, PendingCategoryWriteRepository>();
            services.AddScoped<IPendingProductReadRepository, PendingProductReadRepository>();
            services.AddScoped<IPendingProductWriteRepository, PendingProductWriteRepository>();


            services.AddScoped<IJobReadRepository, JobReadRepository>();
            services.AddScoped<IJobWriteRepository, JobWriteRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            services.AddScoped<IOrderItemReadRepository, OrderItemReadRepository>();
            services.AddScoped<IOrderItemWriteRepository, OrderItemWriteRepository>();

            services.AddScoped<IOrderOverheadReadRepository, OrderOverheadReadRepository>();
            services.AddScoped<IOrderOverheadWriteRepository, OrderOverheadWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<ISectionReadRepository, SectionReadRepository>();
            services.AddScoped<ISectionWriteRepository, SectionWriteRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddHttpClient<IWhatsAppMessageService, WhatsAppMessageService>();
        }
    }
}