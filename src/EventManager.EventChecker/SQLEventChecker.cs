using EventManager.EventChecker.Data.dbEntities;
using EventManager.EventChecker.Exceptions;
using EventManager.EventChecker.interfaces;
using EventManager.Shared.Dto;
using EventManager.Shared.interfaces;
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
    public class SQLEventChecker : IEventChecker
    {
        EventCheckerDbContext _context;
        public SQLEventChecker(EventCheckerDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<List<EMEventInfoDto>> GetAllRegisteredEventsAsync()
        {
            return await _context.EMEventInfos.Select(x => new EMEventInfoDto { EventName = x.EventName, EventPropertiesJson = x.EventPropertiesJson }).ToListAsync();
        }
        public List<EMEventInfoDto> GetAllRegisteredEvents()
        {
            return _context.EMEventInfos.Select(x => new EMEventInfoDto { EventName = x.EventName, EventPropertiesJson = x.EventPropertiesJson }).ToList();
        }
        public async Task<EMEventInfoDto> CheckOrAddEMEventInfo<T>(EMEvent<T> data) where T : IEMEvent
        {
            var propertiesJson = GeneretePropertiesJson(data.EventData);

            //if event exist
            if (await _context.EMEventInfos.AnyAsync(x => x.EventName == data.EventName))
            {
                var dbResult = await _context.EMEventInfos.FirstOrDefaultAsync(x => x.EventName == data.EventName);
                if (dbResult.EventPropertiesJson != propertiesJson)//if event has different properties .Throw Exception
                    throw new EMEventInvalidPropertyException($"{data.EventName} named event has different properties." +
                        $" Registered properties: {dbResult.EventPropertiesJson}");

                return new EMEventInfoDto
                {
                    EventName = dbResult.EventName,
                    EventPropertiesJson = dbResult.EventPropertiesJson
                };// its ok. there is no problem. Return event info
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
        public string GeneretePropertiesJson<T>(T data) where T : IEMEvent
        {
            Dictionary<string, string> propTypeNameDic = new Dictionary<string, string>();
            //get all properties of event 
            var properties = data.GetType().GetProperties();
            foreach (var p in properties)
                propTypeNameDic.Add(p.Name, p.PropertyType.Name);

            return Newtonsoft.Json.JsonConvert.SerializeObject(propTypeNameDic);
        }
    }
}
