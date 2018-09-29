using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ObITwahn.Services.Meeting.Model;

namespace ObITwahn.Trinity.Services.Web.Data
{
    internal class AggregateIncludesInitializer
    {
        private readonly ILookup<Type, string> _includeProperties;

        public AggregateIncludesInitializer()
        {
            var items = new[]
            {
                new {Type = typeof(Meeting), Path = nameof(Meeting.Participants)},
                new {Type = typeof(Meeting), Path = $"{nameof(Meeting.Participants)}.{nameof(EmployeeMeeting.Employee)}"}
               
                //new {Type = typeof(Facility), Path = $"{nameof(Facility.Parts)}.{nameof(FacilityPart.Type)}"},
                //new {Type = typeof(Facility), Path = $"{nameof(Facility.Parts)}.{nameof(FacilityPart.Deductibles)}"},
                //new
                //{
                //    Type = typeof(Facility),
                //    Path =
                //        $"{nameof(Facility.Parts)}.{nameof(FacilityPart.Deductibles)}.{nameof(Deductible.FacilityPart)}"
                //},
                //new {Type = typeof(Facility), Path = $"{nameof(Facility.Parts)}.{nameof(FacilityPart.Facility)}"},
                //new {Type = typeof(Property), Path = nameof(Property.Facilities)},
                //new {Type = typeof(Property), Path = nameof(Property.Segments)},
                //new {Type = typeof(Property), Path = nameof(Property.FunctionalZoningCategory)},
                //new {Type = typeof(Property), Path = nameof(Property.Type)},
                //new {Type = typeof(Property), Path = nameof(Property.TaxNotice)},
                //new {Type = typeof(ZoneMap), Path = nameof(ZoneMap.Group)},
                //new {Type = typeof(ZoneMap), Path = $"{nameof(ZoneMap.Group)}.{nameof(ZoneGroup.Zones)}"},
                //new
                //{
                //    Type = typeof(ZoneMap),
                //    Path = $"{nameof(ZoneMap.Group)}.{nameof(ZoneGroup.Zones)}.{nameof(Zone.Group)}"
                //},
                //new
                //{
                //    Type = typeof(ZoneMap),
                //    Path = $"{nameof(ZoneMap.Group)}.{nameof(ZoneGroup.Zones)}.{nameof(Zone.Data)}"
                //},
                //new {Type = typeof(ZoneMap), Path = nameof(ZoneMap.ZoneData)},
                //new {Type = typeof(ZoneMap), Path = $"{nameof(ZoneMap.ZoneData)}.{nameof(ZoneData.Map)}"},
                //new {Type = typeof(ZoneMap), Path = $"{nameof(ZoneMap.ZoneData)}.{nameof(ZoneData.Zone)}"},
                ////new {Type = typeof(ZoneMap), Path = $"{nameof(ZoneMap.ZoneData)}.{nameof(ZoneData.)}"}
            };

            _includeProperties = items.ToLookup(x => x.Type, x => x.Path);

        }

        public IEnumerable<string> GetIncludes<T>()
        {
            var aggregateRoot = typeof(T);

            if (_includeProperties.Contains(aggregateRoot))
            {
                return _includeProperties[aggregateRoot];
            }

            return Enumerable.Empty<string>();
        }

        public IQueryable<TEntity> IncludePropertiesWhenNeeded<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            var includes = GetIncludes<TEntity>();

            if (!EnumerableExtensions.Any(includes))
            {
                return query;
            }

            foreach (var includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }


            return query;
            //return includes.Aggregate(query, (current, includeProperty) => current);
        }
    }
}