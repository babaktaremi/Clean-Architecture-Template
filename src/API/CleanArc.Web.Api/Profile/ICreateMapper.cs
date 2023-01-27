namespace CleanArc.Web.Api.Profile;

public interface ICreateMapper<TSource>
{
    void Map(AutoMapper.Profile profile)
    {
        profile.CreateMap(typeof(TSource), GetType()).ReverseMap();
    }
}