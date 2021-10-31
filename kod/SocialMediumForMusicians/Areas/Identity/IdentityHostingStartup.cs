using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;

[assembly: HostingStartup(typeof(SocialMediumForMusicians.Areas.Identity.IdentityHostingStartup))]
namespace SocialMediumForMusicians.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}