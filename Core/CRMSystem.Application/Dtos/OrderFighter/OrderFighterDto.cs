using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Application.Dtos.OrderFighter
{
    public class OrderFighterDto
    {
        public string OrderId { get; set; }
        public List<IFormFile> OrderOverhead { get; set; }

        // JSON string kimi saxlanır, FromForm-da da burdan alınır
        [FromForm(Name = "orderItemsJson")]
        public string OrderItemsJson { get; set; } = "[]";

        /// <summary>
        /// Binder və validator buranı görməsin.
        /// Burada JSON string avtomatik List<OrderItemFighterDto>-ə çevrilir.
        /// </summary>
        [NotMapped]
        [BindNever]
        [ValidateNever]
        public List<OrderItemFighterDto> OrderItems
        {
            get
            {
                try
                {
                    var token = JToken.Parse(OrderItemsJson);
                    if (token.Type == JTokenType.Array)
                        return token.ToObject<List<OrderItemFighterDto>>();
                    if (token.Type == JTokenType.Object)
                        return new List<OrderItemFighterDto>
                        {
                            token.ToObject<OrderItemFighterDto>()
                        };
                }
                catch
                {
                    // Parse error olduqda boş siyahı qaytarır
                }
                return new List<OrderItemFighterDto>();
            }
        }
    }
}
