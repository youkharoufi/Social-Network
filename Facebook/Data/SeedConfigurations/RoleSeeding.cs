using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facebook.Data.SeedConfigurations
{


        public class RoleSeeding : IEntityTypeConfiguration<IdentityRole>
        {

            public async void Configure(EntityTypeBuilder<IdentityRole> builder)
            {
                builder
                   .HasData(
                       new IdentityRole()
                       {
                           Name = "Admin",
                           NormalizedName = "ADMIN"

                       },
                       new IdentityRole
                       {
                           Name = "Member",
                           NormalizedName = "MEMBER",

                       }
                   );

            }
        }

}
