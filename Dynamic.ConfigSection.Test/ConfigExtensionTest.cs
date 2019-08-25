using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using Hocon.Extensions.Configuration;
using Dynamic.ConfigSection;
namespace Dynamic.ConfigSection.Test
{
    public class ConfigExtentsionTest
    {
        [Fact]
        public void GetSectionAsDynamic()
        {
            var configBuilder = new ConfigurationBuilder();
            var config = configBuilder.AddHoconFile("akka.default.hocon").Build();
            var akkaConfig = config.GetSectionAsDynamic("akka");
            Assert.Equal("INFO", akkaConfig.akka.loglevel  );
        }
    }
}
