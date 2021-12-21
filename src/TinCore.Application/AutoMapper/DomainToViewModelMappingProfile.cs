using AutoMapper;
using TinCore.Application.ViewModels;
using TinCore.Domain.Models;

namespace TinCore.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            // location
            //CreateMap<Location, LocationViewModel>();

            // entity 
            CreateMap<TinEntity, EntityViewModel>();

            // entity contact
            CreateMap<EntityContact, BusinessContactViewModel>();

            // relation
            //CreateMap<Relation, RelationViewModel>();
            

            // profile models
            CreateMap<ProfileAttribute, ProfileAttributeViewModel>();
            CreateMap<ProfileReview, ProfileReviewViewModel>();
            CreateMap<ProfileSection, ProfileSectionViewModel>();
        }
    }
}
