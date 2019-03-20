using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using NUnitTestProject1.dto;
using System;
using System.Threading.Tasks;
using EventManager.Core.interfaces;
using EventManager.DefaultManager.Extensions;
using EventManager.EventChecker.Extensions;
namespace Tests
{
    public class EventPublishTest
    {
        IEMPublisher _eventPublisher;
        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddEMDefaultManager(
                   checkerOptions =>
                       checkerOptions.UseSQL(sql => sql.UseSqlServer("Data Source=LAPTOP-3O58F4FN;database=eventDB;trusted_connection=yes;"))
                );

            var provider = services.BuildServiceProvider();

            provider.InitializeEMDefaultManager("localhost");

            _eventPublisher = provider.GetService<IEMPublisher>();

        }

        [Test]
        public void PublishCreatedEventAsync()
        {
            try
            {
                Random rnd = new Random();
                var task = Task.Run(async () =>
                  {
                      await
                      _eventPublisher.PublishAsync(new TestEvent2
                      {
                          userid = rnd.Next(0, int.MaxValue),
                          userTc = "tc:" + rnd.Next(0, int.MaxValue)
                      });
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
        public void PublishDeletedEventAsync()
        {
            try
            {
                Random rnd = new Random();
                var task = Task.Run(async () =>
                {
                    await
                    _eventPublisher.PublishAsync(new MyEventClass
                    {
                        id = rnd.Next(0, int.MaxValue),
                        email = "email" + rnd.Next(0, int.MaxValue),
                        username = "user" + rnd.Next(0, int.MaxValue)
                    });
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
        public void PublishCreatedEvent()
        {
            try
            {
                Random rnd = new Random();

                _eventPublisher.Publish(new TestEvent2
                {
                    userid = rnd.Next(0, int.MaxValue),
                    userTc = "tc:" + rnd.Next(0, int.MaxValue)
                });

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
                _eventPublisher.Publish(new MyEventClass
                {
                    id = rnd.Next(0, int.MaxValue),
                    email = "email" + rnd.Next(0, int.MaxValue),
                    username = "user" + rnd.Next(0, int.MaxValue)
                });

            }
            catch (Exception e)
            {
                Assert.That(false, e.Message);

            }
            Assert.Pass();
        }
    }
}