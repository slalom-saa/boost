using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Slalom.Boost.AutoMapper.Mappers
{
    public interface IConditionalObjectMapper
    {
        string ProfileName { get; }
        ICollection<Func<TypePair, bool>> Conventions { get; }
		bool IsMatch(TypePair context);
    }

    public class ConditionalObjectMapper : IConditionalObjectMapper
    {
        public string ProfileName { get; }

        public ConditionalObjectMapper(string profileName)
        {
            this.ProfileName = profileName;
        }

        public bool IsMatch(TypePair typePair)
        {
            return this.Conventions.All(c => c(typePair));
        }

        public ICollection<Func<TypePair, bool>> Conventions { get; } = new Collection<Func<TypePair, bool>>();
    }

    public static class ConventionGeneratorExtensions
    {
        public static IConditionalObjectMapper Where(this IConditionalObjectMapper self, Func<Type, Type, bool> condition)
        {
            self.Conventions.Add(rc => condition(rc.SourceType, rc.DestinationType));
            return self;
        }

    }

}