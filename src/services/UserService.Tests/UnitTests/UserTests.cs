using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;
using Xunit;

namespace UserService.Tests.UnitTests
{
    public class UserTests: IDisposable
    {

        public UserTests()
        {
            
        }

        [Fact]
        public async Task TestUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, }
            };

            Assert.Single(users);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
