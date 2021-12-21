using AutoMapper;
using System;
using TinCore.Application.ViewModels;
using TinCore.Domain.Commands;
using TinCore.Domain.Models;

namespace TinCore.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<LocationViewModel, NewLocationCommand>();
            CreateMap<LocationViewModel, UpdateLocationCommand>();

            // Relation mapping
            //CreateMap<RelationViewModel, Relation>();

            // Entity mapping
            CreateMap<BusinessContactViewModel, EntityContact>();

            // tracking mapping
            //CreateMap<TrackingViewModel, Tracking>();

            // profile models
            CreateMap<ProfileAttributeViewModel, ProfileAttribute>();
            CreateMap<ProfileReviewViewModel, ProfileReview>();
            CreateMap<ProfileSectionViewModel, ProfileSection>();

            // create entity command
            CreateMap<EntityViewModel, NewEntityCommand>();
            CreateMap<NewEntityCommand, TinEntity>();
            //update entity command
            CreateMap<EntityViewModel, UpdateEntityCommand>();
            CreateMap<UpdateEntityCommand, TinEntity>();
            //delete entity command
            CreateMap<EntityViewModel, RemoveEntityCommand>();
            CreateMap<RemoveEntityCommand, TinEntity>();
        }
    }
}
