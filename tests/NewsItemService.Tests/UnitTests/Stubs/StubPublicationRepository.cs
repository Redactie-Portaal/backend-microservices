using NewsItemService.Entities;
using NewsItemService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsItemService.Tests.UnitTests.Stubs
{
    internal class StubPublicationRepository : IPublicationRepository
    {
        public Task<Dictionary<bool, Publication>> GetPublicationById(int id)
        {
            Publication publication = new Publication();

            if (id == 1)
            {
                publication.Id = 1;
                publication.Name = "Twitter";
                publication.Description = "Social media";
                publication.Icon = "empty.png";
            }
            else
            {
                return Task.FromResult(new Dictionary<bool, Publication>() { { false, null } });
            }

            return Task.FromResult(new Dictionary<bool, Publication>() { { true, publication } });
        }
    }
}
