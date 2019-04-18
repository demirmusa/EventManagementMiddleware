using EventManager.EventChecker.SQL.Data;
using EventManager.EventChecker.SQL.Data.DbEntities;
using EventManager.Shared.Dto;
using EventManager.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.EventChecker
{
    /// <summary>
    /// this is an event checker which use sql server to check event
    /// </summary>
    public class SQLEventChecker : IEventInfoLoader

    {
        EventCheckerDbContext _context;
        public SQLEventChecker(
            EventCheckerDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<List<EMEventInfoDto>> GetAllRegisteredEventsAsync()
        {
            return await _context.EMEventInfos.Select(x => new EMEventInfoDto { EventName = x.EventName, EventPropertiesJson = x.EventPropertiesJson }).ToListAsync();
        }
        public async Task<EMEventInfoDto> GetOrAddEMEventInfoAsync<T>(EMEvent<T> data, string propertiesJson) where T : IEMEvent
        {
            //if event exist
            if (await _context.EMEventInfos.AnyAsync(x => x.EventName == data.EventName))
            {
                var dbResult = await _context.EMEventInfos.FirstOrDefaultAsync(x => x.EventName == data.EventName);

                return new EMEventInfoDto
                {
                    EventName = dbResult.EventName,
                    EventPropertiesJson = dbResult.EventPropertiesJson
                };//Return event info
            }
            else // event not exist. Create it
            {
                var eventInfo = new EMEventInfo
                {
                    CreationTime = DateTime.Now,
                    //TODO: Change it
                    CreatorClientName = "",
                    EventName = data.EventName,
                    EventPropertiesJson = propertiesJson
                };
                await _context.EMEventInfos.AddAsync(eventInfo);
                await _context.SaveChangesAsync();
                return new EMEventInfoDto
                {
                    EventName = data.EventName,
                    EventPropertiesJson = propertiesJson
                };
            }
        }


        public List<EMEventInfoDto> GetAllRegisteredEvents()
        {
            return _context.EMEventInfos.Select(x => new EMEventInfoDto { EventName = x.EventName, EventPropertiesJson = x.EventPropertiesJson }).ToList();
        }
        public EMEventInfoDto GetOrAddEMEventInfo<T>(EMEvent<T> data, string propertiesJson) where T : IEMEvent
        {

            //if event exist
            if (_context.EMEventInfos.Any(x => x.EventName == data.EventName))
            {
                var dbResult = _context.EMEventInfos.FirstOrDefault(x => x.EventName == data.EventName);

                return new EMEventInfoDto
                {
                    EventName = dbResult.EventName,
                    EventPropertiesJson = dbResult.EventPropertiesJson
                };
            }
            else // event not exist. Create it
            {
                var eventInfo = new EMEventInfo
                {
                    CreationTime = DateTime.Now,
                    //TODO: Change it
                    CreatorClientName = "",
                    EventName = data.EventName,
                    EventPropertiesJson = propertiesJson
                };
                _context.EMEventInfos.Add(eventInfo);
                _context.SaveChanges();
                return new EMEventInfoDto
                {
                    EventName = data.EventName,
                    EventPropertiesJson = propertiesJson
                };
            }
        }





    }
}
