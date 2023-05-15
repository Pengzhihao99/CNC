using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using MessageCore.Domain.Common.Enum;

namespace MessageCore.Application.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var subscriber = new Subscriber("jeff", "jeff@163.com", "", "jeff.test", SubscriberType.Client, "TSST", true);

            var field = "Email";

            var email = subscriber.GetType().GetProperty(field).GetValue(subscriber);

            var field2 = "Phone";

            var email2 = subscriber.GetType().GetProperty(field2).GetValue(subscriber);
        }
    }
}