using Facebook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facebook.Data.SeedConfigurations
{
    public class UserSeeding : IEntityTypeConfiguration<User>
    {


            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.ToTable("User");
                builder.HasData(
                        new User
                        {
                            UserName = "Youssef",
                            Email = "youssef@email.com",
                            Token = ""

                        },
                        new User
                        {
                            UserName = "Pamela",
                            Email = "pamela@email.com",
                            Token = ""
                            

                        },
                        new User
                        {
                            UserName = "Inyaki",
                            Email = "inyaki@email.com",
                            Token = ""

                        }
                    );
            }



        }
    }

