using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories.OrderItems;
using CRMSystem.Application.Absrtacts.Repositories.OrderOverheads;
using CRMSystem.Application.Absrtacts.Repositories.Orders;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Message;
using CRMSystem.Application.Dtos.OrderCustomer;
using CRMSystem.Application.Dtos.OrderFighter;
using CRMSystem.Application.Dtos.OrderItem;
using CRMSystem.Application.Dtos.Statistic;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IOrderItemWriteRepository _orderItemWriteRepository;
        private readonly IOrderItemReadRepository _orderItemReadRepository;
        private readonly IMediaService _mediaService;
        private readonly IOrderOverheadWriteRepository _orderOverheadWriteRepository;
        private readonly IMapper _mapper;
        private readonly IFighterService _fighterService;

        private readonly IWhatsAppMessageService _whatsAppMessageService;
        private readonly IAdminNotificationService _notificationService;

        public OrderService(
            IOrderReadRepository orderReadRepository,
            IOrderWriteRepository orderWriteRepository,
            IOrderItemWriteRepository orderItemWriteRepository,
            IOrderOverheadWriteRepository orderOverheadWriteRepository,
            IMapper mapper,
            IFighterService fighterService,
            IWhatsAppMessageService whatsAppMessageService,
            IMediaService mediaService,
   
            IOrderItemReadRepository orderItemReadRepository,
            IAdminNotificationService notificationService)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _orderItemWriteRepository = orderItemWriteRepository;
            _orderOverheadWriteRepository = orderOverheadWriteRepository;
            _mapper = mapper;
            _fighterService = fighterService;
            _whatsAppMessageService = whatsAppMessageService;
            _mediaService = mediaService;
     
            _orderItemReadRepository = orderItemReadRepository;
          _notificationService = notificationService;
        }


        public async Task<OrderDto> CreateOrderAsync(string adminId,CreateOrderDto dto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                AdminId = adminId,
                SectionId = Guid.Parse(dto.SectionId),
                Description = dto.Description,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow,
                EmployeeConfirm = true,
                OrderLimitTime = DateTime.SpecifyKind(
               DateTime.ParseExact(dto.OrderLimitTime, "dd.MM.yyyy", CultureInfo.InvariantCulture),
               DateTimeKind.Utc
           ),
                OrderDeliveryTime = DateTime.UtcNow.AddDays(3),
            };

            await _orderWriteRepository.AddAsync(order);

            // Order Items əlavə et
            foreach (var item in dto.Items)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse(item.ProductId),
                    OrderId = order.Id,
                    RequiredQuantity = item.RequiredQuantity,
            
                };
                await _orderItemWriteRepository.AddAsync(orderItem);
            }

     

            await _orderWriteRepository.CommitAsync();
            await _notificationService.CreateAsync(
                 type: "order_created",
                 orderId: order.Id.ToString(),
                 categoryId: null,
                 productId: null
                 );
            var fighters = await _fighterService.GetAllFightersAsync();

            foreach (var fighter in fighters)
            {
                if (!string.IsNullOrWhiteSpace(fighter.PhoneNumber))
                {
                    var cleanedPhone = fighter.PhoneNumber.Trim();

                    // Əgər 0 ilə başlayırsa, onu +994 ilə əvəzlə
                    if (cleanedPhone.StartsWith("0"))
                        cleanedPhone = "+994" + cleanedPhone.Substring(1);
                    else if (!cleanedPhone.StartsWith("+994"))
                        continue; // Yanlış formatlı nömrələr keçilsin

                    await _whatsAppMessageService.SendMessageAsync(new WhatsAppMessageDto
                    {
                        PhoneNumber = cleanedPhone,
                        Message = "Yeni sifariş daxil olub. Zəhmət olmasa profilinə daxil ol və sifarişi təchiz et."
                    });
                }
            }



            return _mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderReadRepository.GetAllAsync(
          func: o => !o.IsDeleted,
          include: q => q
              .Include(o => o.Section)
                  .ThenInclude(s => s.Department)
                      .ThenInclude(d => d.Company)
              .Include(o => o.Items.Where(z => !z.IsDeleted))
                  .ThenInclude(oi => oi.Vendor)
              .Include(o => o.Items)
                  .ThenInclude(oi => oi.Product)
              .Include(o => o.Overhead)
              .Include(o => o.Admin)
              .Include(o => o.Fighter)
      );

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<(int TotalConfirmedOrders, decimal PercentGrowth)> GetFighterConfirmedOrdersStatisticsAsync(string companyId)
        {
            if (!Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("Company ID formatı düzgün deyil!");

            var today = DateTime.UtcNow.Date;

            var endCurrentPeriod = today;
            var startCurrentPeriod = endCurrentPeriod.AddMonths(-1);

            var endPreviousPeriod = startCurrentPeriod;
            var startPreviousPeriod = endPreviousPeriod.AddMonths(-1);

            var orders = await _orderReadRepository.GetAllAsync(
                o => !o.IsDeleted &&
                     o.FighterConfirm &&
                     o.Section.Department.CompanyId == companyGuid,
                include: q => q.Include(o => o.Section).ThenInclude(s => s.Department)
            );

            var totalConfirmedOrders = orders.Count;

            var ordersInCurrentPeriod = orders.Count(o =>
                o.CreatedDate >= startCurrentPeriod && o.CreatedDate < endCurrentPeriod
            );

            var ordersInPreviousPeriod = orders.Count(o =>
                o.CreatedDate >= startPreviousPeriod && o.CreatedDate < endPreviousPeriod
            );

            decimal percentGrowth = 0;
            if (ordersInPreviousPeriod > 0)
            {
                percentGrowth = ((decimal)(ordersInCurrentPeriod - ordersInPreviousPeriod) / ordersInPreviousPeriod) * 100;
            }
            else if (ordersInCurrentPeriod > 0)
            {
                percentGrowth = 100;
            }

            return (totalConfirmedOrders, percentGrowth);
        }


        public async Task<OrderDto> CreateOrderFighterAsync(string adminId, OrderFighterDto dto)
        {
            if (!Guid.TryParse(dto.OrderId, out var orderGuid))
                throw new GlobalAppException("Order ID formatı düzgün deyil!");

            var order = await _orderReadRepository.GetAsync(o => o.Id == orderGuid && !o.IsDeleted);

            if (order == null)
                throw new GlobalAppException("Sifariş tapılmadı!");

            foreach (var itemDto in dto.OrderItems)
            {
                var existingOrderItem = await _orderItemReadRepository.GetByIdAsync(itemDto.OrderItemId);
                if (existingOrderItem != null && existingOrderItem.OrderId == order.Id)
                {
                    existingOrderItem.Price = itemDto.Price;
                    if (!Guid.TryParse(itemDto.VendorId, out var vendorId))
                        throw new GlobalAppException("Order ID formatı düzgün deyil!");
                    existingOrderItem.VendorId= vendorId;
                    existingOrderItem.SuppliedQuantity = itemDto.SuppliedQuantity;
                    if (itemDto.SuppliedQuantity > 0)
                    {
                        existingOrderItem.OrderItemDeliveryTime = DateTime.UtcNow;
                    }
                    existingOrderItem.LastUpdatedDate = DateTime.UtcNow;

                    await _orderItemWriteRepository.UpdateAsync(existingOrderItem);
                }
            }

            // Faylları yaddaşa yaz və OrderOverhead kimi saxla
            if (dto.OrderOverhead != null && dto.OrderOverhead.Any())
            {
                foreach (var file in dto.OrderOverhead)
                {
                    var savedFileName = await _mediaService.UploadFile(file, "overhead_files");

                    var orderOverhead = new OrderOverhead
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        FileName = savedFileName,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _orderOverheadWriteRepository.AddAsync(orderOverhead);
                }
            }

            // Order-un FighterConfirm sahəsini true, OrderDeliveryTime-i indi vaxta təyin et
            order.FighterConfirm = true;
            order.OrderDeliveryTime = DateTime.UtcNow;
            order.LastUpdatedDate = DateTime.UtcNow;
            order.FighterId = adminId;

            await _orderWriteRepository.UpdateAsync(order);

            // Commit hamısını
            await _orderItemWriteRepository.CommitAsync();
            await _orderOverheadWriteRepository.CommitAsync();
            await _orderWriteRepository.CommitAsync();
            await _notificationService.CreateAsync(
           type: "order_fighter_confirmed",
           orderId: order.Id.ToString(),
           categoryId: null,
           productId: null
          );

            // WhatsApp mesaj göndərmə (istəyə görə)
            //var customers = await _customerService.GetAllCustomersAsync();
            //foreach (var customer in customers)
            //{
            //    if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
            //    {
            //        var cleanedPhone = customer.PhoneNumber.Trim();

            //        if (cleanedPhone.StartsWith("0"))
            //            cleanedPhone = "+994" + cleanedPhone.Substring(1);
            //        else if (!cleanedPhone.StartsWith("+994"))
            //            continue;

            //        await _whatsAppMessageService.SendMessageAsync(new WhatsAppMessageDto
            //        {
            //            PhoneNumber = cleanedPhone,
            //            Message = $" {order.Id} nömrəli sifarişiniz təsdiq olundu."
            //        });
            //    }
            //}

            return _mapper.Map<OrderDto>(order);
        }
        public async Task ConfirmOrderDeliveryAsync(string orderId)
        {
            if (!Guid.TryParse(orderId, out var orderGuid))
                throw new GlobalAppException("Order ID formatı düzgün deyil!");

            // Sifarişi tap
            var order = await _orderReadRepository.GetAsync(o => o.Id == orderGuid && !o.IsDeleted);
            if (order == null)
                throw new GlobalAppException("Sifariş tapılmadı!");

            // Şərti yoxla
            if (order.EmployeeConfirm && order.FighterConfirm)
            {
                order.EmployeeDelivery = true;
                order.LastUpdatedDate = DateTime.UtcNow;

                await _orderWriteRepository.UpdateAsync(order);
                await _orderWriteRepository.CommitAsync();
                await _notificationService.CreateAsync(
                type: "order_delivery_confirmed",
                orderId: order.Id.ToString(),
                categoryId: null,
                productId: null
                );

            }
            else
            {
                throw new GlobalAppException("Order EmployeeConfirm və FighterConfirm true deyil, təhvili aktivləşdirmək mümkün deyil.");
            }


        }

        public async Task<List<OrderDto>> GetOrdersByAdminIdAsync(string adminId)
        {


            var orders = await _orderReadRepository.GetAllAsync(
        func: o => !o.IsDeleted,
        include: q => q
            .Include(o => o.Section)
                .ThenInclude(s => s.Department)
                    .ThenInclude(d => d.Company)
            .Include(o => o.Items.Where(z => !z.IsDeleted))
                .ThenInclude(oi => oi.Vendor)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Overhead)
            .Include(o => o.Admin)
            .Include(o => o.Fighter)
    );


            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task DeleteOrderByAdminAsync(string orderId, string adminId)
        {
            if (!Guid.TryParse(orderId, out var orderGuid))
                throw new GlobalAppException("Order ID formatı düzgün deyil!");

            // Sifarişi tap və həm orderId, həm də adminId uyğunluğunu yoxla
            var order = await _orderReadRepository.GetAsync(
                o => o.Id == orderGuid && !o.IsDeleted && (o.AdminId == adminId));

            if (order == null)
                throw new GlobalAppException("Belə bir sifariş tapılmadı və ya icazəniz yoxdur!");

            // Silinməyi işarələ
            order.IsDeleted = true;
            order.DeletedDate = DateTime.UtcNow;

            await _orderWriteRepository.UpdateAsync(order);
            await _orderWriteRepository.CommitAsync();
        }

        public async Task DeleteOrderByFighterAsync(string orderId)
        {
            if (!Guid.TryParse(orderId, out var orderGuid))
                throw new GlobalAppException("Order ID formatı düzgün deyil!");

            // Sifarişi tap və həm orderId, həm də adminId uyğunluğunu yoxla
            var order = await _orderReadRepository.GetAsync(
                o => o.Id == orderGuid && !o.IsDeleted);

            if (order == null)
                throw new GlobalAppException("Belə bir sifariş tapılmadı və ya icazəniz yoxdur!");

            // Silinməyi işarələ
            order.IsDeleted = true;
            order.DeletedDate = DateTime.UtcNow;

            await _orderWriteRepository.UpdateAsync(order);
            await _orderWriteRepository.CommitAsync();
        }
        public async Task<List<MonthlyOrderStatusDto>> GetMonthlyOrderStatusCountsAsync(int year, string companyId)
        {
            if (!Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("Company ID formatı düzgün deyil!");

            var orders = await _orderReadRepository.GetAllAsync(
                o => !o.IsDeleted &&
                     o.CreatedDate.Year == year &&
                     o.Section.Department.CompanyId == companyGuid,
                include: q => q.Include(o => o.Section).ThenInclude(s => s.Department)
            );

            var grouped = orders
                .GroupBy(o => o.CreatedDate.Month)
                .Select(g => new MonthlyOrderStatusDto
                {
                    Month = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(g.Key),
                    CompletedCount = g.Count(o => o.EmployeeConfirm && o.FighterConfirm && o.EmployeeDelivery),
                    CanceledCount = g.Count(o => o.IsDeleted),
                    PendingCount = g.Count(o => o.EmployeeConfirm && !o.FighterConfirm && !o.EmployeeDelivery)
                })
                .ToList();

            // Ayların hamısını qaytarmaq üçün sıfırla doldururuq
            var result = new List<MonthlyOrderStatusDto>();
            for (int i = 1; i <= 12; i++)
            {
                var name = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(i);
                var monthData = grouped.FirstOrDefault(m => m.Month == name);

                result.Add(new MonthlyOrderStatusDto
                {
                    Month = name,
                    CompletedCount = monthData?.CompletedCount ?? 0,
                    CanceledCount = monthData?.CanceledCount ?? 0,
                    PendingCount = monthData?.PendingCount ?? 0
                });
            }

            return result;
        }


        public async Task<Dictionary<string, decimal>> GetMonthlyOrderAmountsByYearAsync(int year, string companyId)
        {
            if (!Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("Company ID formatı düzgün deyil!");

            var orders = await _orderReadRepository.GetAllAsync(
                o => !o.IsDeleted &&
                     o.FighterConfirm &&
                     o.CreatedDate.Year == year &&
                     o.Section.Department.CompanyId == companyGuid,
                include: q => q
                    .Include(o => o.Section)
                        .ThenInclude(s => s.Department)
                    .Include(o => o.Items)
            );

            var monthlyAmounts = orders
                .GroupBy(o => o.CreatedDate.Month)
                .ToDictionary(
                    g => CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(g.Key),
                    g => g.SelectMany(o => o.Items).Sum(oi => oi.Price ?? 0m)
                );

            // Boş aylar üçün sıfırla doldur
            for (int month = 1; month <= 12; month++)
            {
                var name = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(month);
                if (!monthlyAmounts.ContainsKey(name))
                {
                    monthlyAmounts[name] = 0;
                }
            }

            return monthlyAmounts
                .OrderBy(kvp => DateTime.ParseExact(kvp.Key, "MMMM", new CultureInfo("az")))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public async Task<Dictionary<string, int>> GetMonthlyOrderCountsByYearAsync(int year, string companyId)
        {
            if (!Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("Company ID formatı düzgün deyil!");

            var orders = await _orderReadRepository.GetAllAsync(
                o => !o.IsDeleted &&
                     o.FighterConfirm &&
                     o.CreatedDate.Year == year &&
                     o.Section.Department.CompanyId == companyGuid,
                include: q => q.Include(o => o.Section).ThenInclude(s => s.Department)
            );

            var monthlyCounts = orders
                .GroupBy(o => o.CreatedDate.Month)
                .ToDictionary(
                    g => CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(g.Key),
                    g => g.Count()
                );

            for (int month = 1; month <= 12; month++)
            {
                var name = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(month);
                if (!monthlyCounts.ContainsKey(name))
                {
                    monthlyCounts[name] = 0;
                }
            }

            return monthlyCounts
                .OrderBy(kvp => DateTime.ParseExact(kvp.Key, "MMMM", new CultureInfo("az")))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }



        public async Task<List<OrderDto>> GetOrdersByVendorIdAsync(string vendorId)
        {
            if (!Guid.TryParse(vendorId, out var vendorGuid))
                throw new GlobalAppException("Vendor ID formatı düzgün deyil!");

            var orders = await _orderReadRepository.GetAllAsync(
                func: o => !o.IsDeleted && o.FighterConfirm && o.Items.Any(item => item.VendorId == vendorGuid && !item.IsDeleted),
                include: q => q
                    .Include(o => o.Section)
                        .ThenInclude(s => s.Department)
                            .ThenInclude(d => d.Company)
                    .Include(o => o.Items.Where(i => !i.IsDeleted))
                        .ThenInclude(i => i.Vendor)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                    .Include(o => o.Overhead)
                    .Include(o => o.Admin)
                    .Include(o => o.Fighter)
            );

            return _mapper.Map<List<OrderDto>>(orders);
        }
        public async Task<int> GetFighterConfirmedOrderCountAsync(string fighterId, string companyId)
        {
            if (!Guid.TryParse(fighterId, out var fighterGuid) || !Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var count = await _orderReadRepository.GetAllAsync(
                o => o.FighterId == fighterId &&
                     o.FighterConfirm &&
                     !o.IsDeleted &&
                     o.Section.Department.CompanyId == companyGuid,
                include: q => q.Include(o => o.Section).ThenInclude(s => s.Department)
            );

            return count.Count;
        }


        public async Task<List<MonthlyFighterOrderStatusDto>> GetFighterMonthlyCompletedAndIncompleteOrdersAsync(string fighterId, int year, string companyId)
        {
            if (!Guid.TryParse(fighterId, out var fighterGuid) || !Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var orders = await _orderReadRepository.GetAllAsync(
                o => o.FighterId == fighterId &&
                     o.FighterConfirm &&
                     !o.IsDeleted &&
                     o.CreatedDate.Year == year &&
                     o.Section.Department.CompanyId == companyGuid,
                include: q => q
                    .Include(o => o.Section)
                        .ThenInclude(s => s.Department)
                    .Include(o => o.Items.Where(i => !i.IsDeleted))
            );

            var monthlyStats = orders
                .GroupBy(o => o.CreatedDate.Month)
                .Select(g =>
                {
                    int completed = 0;
                    int incomplete = 0;

                    foreach (var order in g)
                    {
                        bool allItemsSupplied = order.Items.All(i =>
                            i.RequiredQuantity == i.SuppliedQuantity
                        );

                        if (allItemsSupplied)
                            completed++;
                        else
                            incomplete++;
                    }

                    return new MonthlyFighterOrderStatusDto
                    {
                        Month = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(g.Key),
                        Completed = completed,
                        Incomplete = incomplete
                    };
                }).ToList();

            // Bütün aylar üçün nəticəni doldur
            var result = new List<MonthlyFighterOrderStatusDto>();
            for (int month = 1; month <= 12; month++)
            {
                var name = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(month);
                var existing = monthlyStats.FirstOrDefault(x => x.Month == name);
                result.Add(new MonthlyFighterOrderStatusDto
                {
                    Month = name,
                    Completed = existing?.Completed ?? 0,
                    Incomplete = existing?.Incomplete ?? 0
                });
            }

            return result;
        }


        public async Task<(decimal CompletedPercent, decimal CanceledPercent, decimal WaitingPercent)> GetFighterOrderStatusPercentagesAsync(string fighterId, string companyId)
        {
            if (!Guid.TryParse(fighterId, out var fighterGuid) || !Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var orders = await _orderReadRepository.GetAllAsync(
                o => o.FighterId == fighterId &&
                     o.Section.Department.CompanyId == companyGuid,
                include: q => q.Include(o => o.Section).ThenInclude(s => s.Department)
            );

            if (orders.Count == 0)
                return (0, 0, 0);

            var totalOrders = orders.Count;

            var completed = orders.Count(
                o => o.EmployeeConfirm && o.FighterConfirm && o.EmployeeDelivery && !o.IsDeleted
            );

            var canceled = orders.Count(
                o => o.IsDeleted
            );

            var waiting = orders.Count(
                o => o.EmployeeConfirm && !o.FighterConfirm && !o.EmployeeDelivery && !o.IsDeleted
            );

            return (
                CompletedPercent: (decimal)completed / totalOrders * 100,
                CanceledPercent: (decimal)canceled / totalOrders * 100,
                WaitingPercent: (decimal)waiting / totalOrders * 100
            );
        }


        public async Task<decimal> GetTotalSuppliedQuantityByProductIdAsync(string productId, string companyId)
        {
            if (!Guid.TryParse(productId, out var productGuid) || !Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var orders = await _orderReadRepository.GetAllAsync(
                o => o.EmployeeConfirm && o.FighterConfirm && o.EmployeeDelivery && !o.IsDeleted &&
                     o.Section.Department.CompanyId == companyGuid,
                include: q => q
                    .Include(o => o.Section)
                        .ThenInclude(s => s.Department)
                    .Include(o => o.Items.Where(i => !i.IsDeleted && i.ProductId == productGuid))
            );

            var totalQuantity = orders
                .SelectMany(o => o.Items)
                .Sum(i => i.SuppliedQuantity ?? 0m);

            return totalQuantity;
        }
        public async Task<Dictionary<string, decimal>> GetMonthlySuppliedQuantitiesByProductAsync(string productId, int year, string companyId)
        {
            if (!Guid.TryParse(productId, out var productGuid) || !Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var orders = await _orderReadRepository.GetAllAsync(
                o => o.EmployeeConfirm && o.FighterConfirm && o.EmployeeDelivery && !o.IsDeleted &&
                     o.CreatedDate.Year == year &&
                     o.Section.Department.CompanyId == companyGuid,
                include: q => q
                    .Include(o => o.Section)
                        .ThenInclude(s => s.Department)
                    .Include(o => o.Items.Where(i => !i.IsDeleted && i.ProductId == productGuid))
            );

            var monthlyQuantities = orders
                .GroupBy(o => o.CreatedDate.Month)
                .ToDictionary(
                    g => CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(g.Key),
                    g => g.SelectMany(o => o.Items).Sum(i => i.SuppliedQuantity ?? 0m)
                );

            // Ayların siyahısını tamamla
            for (int month = 1; month <= 12; month++)
            {
                var monthName = CultureInfo.GetCultureInfo("az").DateTimeFormat.GetMonthName(month);
                if (!monthlyQuantities.ContainsKey(monthName))
                    monthlyQuantities[monthName] = 0;
            }

            return monthlyQuantities
                .OrderBy(kvp => DateTime.ParseExact(kvp.Key, "MMMM", new CultureInfo("az")))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }


        public async Task<OrderDto> GetOrderByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                throw new GlobalAppException("ID formatı düzgün deyil!");

            var order = await _orderReadRepository.GetAsync(
                o => o.Id == guid && !o.IsDeleted,
                include: q => q
                .Include(o => o.Section)
                .ThenInclude(s => s.Department)
                    .ThenInclude(d => d.Company)
            .Include(o => o.Items.Where(z=>!z.IsDeleted))
                .ThenInclude(oi => oi.Vendor)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Overhead)
            .Include(o => o.Admin)
            .Include(o => o.Fighter)

            );

            if (order == null)
                throw new GlobalAppException("Sifariş tapılmadı!");

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<(decimal DeletedPercent, decimal CompletedPercent, decimal WaitingPercent)> GetOrderStatusPercentagesAsync(string companyId)
        {
            if (!Guid.TryParse(companyId, out var companyGuid))
                throw new GlobalAppException("Company ID formatı düzgün deyil!");

            var orders = await _orderReadRepository.GetAllAsync(
                o => o.Section.Department.CompanyId == companyGuid,
                include: q => q.Include(o => o.Section).ThenInclude(s => s.Department)
            );

            var totalOrdersCount = orders.Count;

            if (totalOrdersCount == 0)
                return (0, 0, 0);

            var deletedCount = orders.Count(o => o.IsDeleted);

            var completedCount = orders.Count(o =>
                o.EmployeeConfirm && o.FighterConfirm && o.EmployeeDelivery && !o.IsDeleted);

            var waitingCount = orders.Count(o =>
                o.EmployeeConfirm && !o.FighterConfirm && !o.EmployeeDelivery && !o.IsDeleted);

            decimal deletedPercent = ((decimal)deletedCount / totalOrdersCount) * 100;
            decimal completedPercent = ((decimal)completedCount / totalOrdersCount) * 100;
            decimal waitingPercent = ((decimal)waitingCount / totalOrdersCount) * 100;

            return (deletedPercent, completedPercent, waitingPercent);
        }

    }

}
