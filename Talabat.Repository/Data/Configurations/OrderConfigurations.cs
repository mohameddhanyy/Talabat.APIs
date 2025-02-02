﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());
            
            builder.Property(st => st.Status)
                   .HasConversion(
                           OStatus => OStatus.ToString(),
                           OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
                           );

            builder.HasMany(O => O.Items)
                .WithOne();

            builder.HasOne(D => D.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(P => P.Subtotal)
                .HasColumnType("decimal(18,2)");

            
        }
    }
}
