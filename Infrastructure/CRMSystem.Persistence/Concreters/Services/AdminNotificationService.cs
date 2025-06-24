using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Application.Absrtacts.Repositories.AdminNotifications;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Notification;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class AdminNotificationService : IAdminNotificationService
    {
        private readonly IAdminNotificationReadRepository _readRepo;
        private readonly IAdminNotificationWriteRepository _writeRepo;
        private readonly IMapper _mapper;

        public AdminNotificationService(
            IAdminNotificationReadRepository readRepo,
            IAdminNotificationWriteRepository writeRepo,
            IMapper mapper)
        {
            _readRepo = readRepo;
            _writeRepo = writeRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new AdminNotification. Exactly one of categoryId/productId must be non-null.
        /// </summary>
        public async Task CreateAsync(string type, string? categoryId, string? productId,string? orderId)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty", nameof(type));

            var entity = new AdminNotification
            {
                Type = type,
                CategoryId = string.IsNullOrWhiteSpace(categoryId)
                                    ? (Guid?)null
                                    : Guid.Parse(categoryId),
                ProductId = string.IsNullOrWhiteSpace(productId)
                                    ? (Guid?)null
                                    : Guid.Parse(productId),
                OrderId = string.IsNullOrWhiteSpace(orderId)
                                    ? (Guid?)null
                                    : Guid.Parse(orderId),
                IsRead = false,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            };

            await _writeRepo.AddAsync(entity);
            await _writeRepo.CommitAsync();
        }

        public async Task<List<AdminNotificationDto>> GetAllAsync()
        {
            var list = await _readRepo.GetAllAsync(
                n => !n.IsDeleted,
                include: q => q.Include(x => x.Category)
                               .Include(x => x.Product).Include(x=>x.Order),
                
                orderBy: q => q.OrderByDescending(x => x.CreatedDate)
            );

            return _mapper.Map<List<AdminNotificationDto>>(list);
        }

        public async Task<List<AdminNotificationDto>> GetUnreadAsync()
        {
            var list = await _readRepo.GetAllAsync(
                n => !n.IsDeleted && !n.IsRead,
                include: q => q.Include(x => x.Category)
                               .Include(x => x.Product).Include(x => x.Order),
                orderBy: q => q.OrderByDescending(x => x.CreatedDate)
            );

            return _mapper.Map<List<AdminNotificationDto>>(list);
        }

        public async Task<AdminNotificationDto> GetByIdAsync(string id)
        {
            var entity = await _readRepo.GetAsync(
                n => n.Id.ToString() == id && !n.IsDeleted,
                include: q => q.Include(x => x.Category)
                               .Include(x => x.Product).Include(x => x.Order)
            );

            if (entity == null)
                throw new GlobalAppException("Notification not found!");

            return _mapper.Map<AdminNotificationDto>(entity);
        }

        public async Task MarkAsReadAsync(string id)
        {
            var entity = await _readRepo.GetAsync(n => n.Id.ToString() == id && !n.IsDeleted);
            if (entity == null)
                throw new GlobalAppException("Notification not found!");

            entity.IsRead = true;
            entity.LastUpdatedDate = DateTime.Now;

            await _writeRepo.UpdateAsync(entity);
            await _writeRepo.CommitAsync();
        }
    }
}
