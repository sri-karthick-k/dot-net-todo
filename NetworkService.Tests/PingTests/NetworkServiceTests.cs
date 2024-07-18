using Xunit;
using NetworkUtility.Ping;
using FluentAssertions;
using FluentAssertions.Extensions;
using System.Net.NetworkInformation;

namespace NetworkUtility.Tests.PingTests
{


    public class NetworkServiceTests
    {
        private NetworkService _pingService;

        public NetworkServiceTests() 
        { 
            _pingService = new NetworkService();
        }
        [Fact]
        public void NetworkService_SendPing_ReturnString()
        {
            var result = _pingService.SendPing();

            result.Should().NotBeNull();
            result.Should().Be("Success: Sent Ping!");
            result.Should().Contain("Success", Exactly.Once());
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(10, 50, 60)]
        public void NetworkService_PingTimeOut_ReturnInt(int a, int b, int expected)
        {
            

            int result = _pingService.PingTimeOut(a, b);

            result.Should().Be(expected);
            result.Should().NotBeInRange(-1000, 0);
            result.Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public void NetworkService_GetLastPing_ReturnDate()
        {
            var date = _pingService.GetLastPing();
            date.Should().BeAfter(1.January(2020));
            date.Should().BeBefore(1.January(2025));
        }

        [Fact]
        public void NetworkService_GetPingOptions_ReturnPingOptions()
        {
            var expected = new PingOptions()
            {
                Ttl = 1,
                DontFragment = true,
            };
            var result = _pingService.GetPingOptions();
            result.Should().NotBeNull();
            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expected);
            result.Ttl.Should().Be(1);
        }

        [Fact]
        public void NetworkService_GetLastPings_ReturnIEnumerablePingOptions()
        {
            var expected = new PingOptions()
            {
                Ttl = 1,
                DontFragment = true,
            };

            var result = _pingService.GetLastPings();
            result.Should().NotBeNull();
            result.Should().BeOfType<List<PingOptions>>();
            result.Should().ContainEquivalentOf(expected);
            result.Should().AllBeEquivalentTo(expected);
        }
    }
}
