using AutoMapper;

namespace CleanArc.Infrastructure.Identity.Profiles;

public interface ICreateMapper<TSource>
{
    void Map(Profile profile)
    {
        profile.CreateMap(typeof(TSource), GetType()).ReverseMap();
    }
}