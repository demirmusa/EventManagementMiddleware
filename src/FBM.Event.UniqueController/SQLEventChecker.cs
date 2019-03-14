﻿using FBM.Event.UniqueController.Data.dbEntities;
using FBM.Event.UniqueController.Dto;
using FBM.Event.UniqueController.Exceptions;
using FBM.Event.UniqueController.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBM.Event.UniqueController
{
    /// <summary>
    /// this is an event checker which use sql server to check event
    /// </summary>
    public class SQLEventChecker : IEventChecker
    {
        UniqueControllerDbContext _context;
        public SQLEventChecker(UniqueControllerDbContext context)
        {
            _context = context;
        }

        public async Task<List<FBMEventInfo>> GetAllRegisteredEvents()
        {
            return await _context.FBMEventInfos.ToListAsync();
        }
        public async Task<FBMEventInfo> CheckOrAddFBMEventInfo<T>(FBMEventInfoRequestDto<T> data) where T : new()
        {
            var propertiesJson = GeneretePropertiesJson(data.Event);

            //if event exist
            if (await _context.FBMEventInfos.AnyAsync(x => x.EventName == data.EventName))
            {
                var dbResult = await _context.FBMEventInfos.FirstOrDefaultAsync(x => x.EventName == data.EventName);
                if (dbResult.EventPropertiesJson != propertiesJson)//if event has different properties .Throw Exception
                    throw new FBMEventInvalidPropertyException($"{data.EventName} named event has different properties." +
                        $" Registered properties: {dbResult.EventPropertiesJson}");

                return dbResult;// its ok. there is no problem. Return event info
            }
            else // event not exist. Create it
            {
                var eventInfo = new FBMEventInfo
                {
                    CreationTime = DateTime.Now,
                    //TODO: Change it
                    CreatorClientName = "",
                    EventName = data.EventName,
                    EventPropertiesJson = propertiesJson
                };
                await _context.FBMEventInfos.AddAsync(eventInfo);
                return eventInfo;
            }
        }
        public string GeneretePropertiesJson<T>(T data) where T : new()
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
