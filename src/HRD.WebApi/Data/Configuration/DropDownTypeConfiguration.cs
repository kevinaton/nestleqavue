using HRD.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HRD.WebApi.Data.Configuration
{
    public class DropDownTypeConfiguration : IEntityTypeConfiguration<DropDownType>
    {
        public void Configure(EntityTypeBuilder<DropDownType> builder)
        {
            builder.HasData(
                new DropDownType { Id = 25, Name = "Dummy" }
                );
        }
    }
}
