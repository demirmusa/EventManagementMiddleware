using FBM.Event.Client.interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using FBM.Event.DefaultManager.Extensions;
using Microsoft.EntityFrameworkCore;
using FBM.Event.UniqueController.Extensions;
using NUnitTestProject1.dto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    public class EventPublishTest
    {
        IEventPublisher _eventPublisher;
        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddFBMEventClientWithDefaultManager(
               eventOptions =>
               {
                   eventOptions.CheckIsEventUnique = true;
                   eventOptions.CacheExpireTimeMinute = 30;
                   //eventOptions.RegisteredEventsMemoryCacheKey = "";
               },
               checkerOptions =>
               {
                   checkerOptions.UseSQL(sql => sql.UseSqlServer("Data Source=LAPTOP-3O58F4FN;database=eventDB;trusted_connection=yes;"));
               });

            var provider = services.BuildServiceProvider();
            provider.InitializeFBMEventClientDefaultManager("localhost");
            _eventPublisher = provider.GetService<IEventPublisher>();

        }

        [Test]
        public void PublishCreatedEvent()
        {
            try
            {
                Random rnd = new Random();
                var task = Task.Run(async () =>
                  {
                      await
                      _eventPublisher.PublishAsync(new TestEvent2 { userid = rnd.Next(0, int.MaxValue), userTc = "tc:" + rnd.Next(0, int.MaxValue) });
                  });
                task.Wait();
            }
            catch (Exception e)
            {
                Assert.That(false, e.Message);

            }
            Assert.Pass();
        }
        [Test]
        public void PublishDeletedEvent()
        {
            try
            {
                Random rnd = new Random();
                var task = Task.Run(async () =>
                {
                    await
                    _eventPublisher.PublishAsync(new MyEventClass { id = rnd.Next(0, int.MaxValue), email = "email" + rnd.Next(0, int.MaxValue), username = "user" + rnd.Next(0, int.MaxValue) });
                });
                task.Wait();
            }
            catch (Exception e)
            {
                Assert.That(false, e.Message);

            }
            Assert.Pass();
        }
    }
}