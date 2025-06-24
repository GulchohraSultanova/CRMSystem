using CRMSystem.Domain.Entities.Common;
using System;

namespace CRMSystem.Domain.Entities
{
    public class AdminNotification : BaseEntity
    {
        /// <summary>
        /// The “type” of this notification (e.g. "CategoryCreateRequest", "CategoryDeleteRequest",
        /// "ProductUpdateRequest", etc.).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// If this notification is about a Category, this holds the Category's ID.
        /// Otherwise null.
        /// </summary>
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }

        /// <summary>
        /// If this notification is about a Product, this holds the Product's ID.
        /// Otherwise null.
        /// </summary>
        
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }

        /// <summary>
        /// Whether the admin has already seen (or handled) this notification.
        /// </summary>
        public bool IsRead { get; set; } = false;
    }
}
