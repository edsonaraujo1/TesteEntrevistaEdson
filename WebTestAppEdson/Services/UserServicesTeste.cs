using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Interfaces;
using WebApi.Models;
using WebAPI.Repositories;
using Xunit;

namespace WebTestAppEdson.Services
{
    public class UserServicesTeste
    {
        private SeguroController userServices;

        public UserServicesTeste()
        {
            userServices = new SeguroController(new Mock<ISeguroRepository>().Object, new Mock<IMapper>().Object);
        }

        [Fact]
        public void Post_SendingValidID()
        {
            object result = userServices.CadastrarSeguro(new Seguro { id = Convert.ToInt32(Guid.NewGuid()) });
            Assert.False(Convert.ToBoolean(result));
        }
    }
}
