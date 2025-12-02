using Application.Core.DTO.Admin;
using Application.Core.DTO.Message;
using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Core.Model.Admin;
using Application.Core.Model.Like;
using Application.Core.Model.Message;
using Application.Core.Model.User;
using Application.Core.UseCase.Admin;
using Application.Core.UseCase.Like;
using Application.Core.UseCase.Login;
using Application.Core.UseCase.Logout;
using Application.Core.UseCase.Message;
using Application.Core.UseCase.User;
using Application.Core.Validator;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infraestructure.IOC;

internal static class DependencyInjectionUseCase
{
    internal static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<InsertUserModel, Result<UserLoginDto>>, InsertUserUseCase>();
        services.AddScoped<IRequestHandler<LoginModel, Result<UserLoginDto>>, LoginUseCase>();
        services.AddScoped<IRequestHandler<RefreshTokenModel, Result<UserLoginDto>>, RefreshTokenUseCase>();
        services.AddScoped<IRequestHandler<GetAllUserModel, Result<IEnumerable<UserDto>>>, GetAllUserUseCase>();
        services.AddScoped<IRequestHandler<GetUserByUserNameModel, Result<UserDto>>, GetUserByUserNameUseCase>();
        services.AddScoped<IRequestHandler<GetUserByIdModel, Result<UserDto?>>, GetUserByIdUseCase>();
        services.AddScoped<IRequestHandler<UpdateUserModel, Result<bool>>, UpdateUserUseCase>();
        services.AddScoped<IRequestHandler<PhotoUploadModel, Result<PhotoUserDto>>, UploadPhotoUserUseCase>();
        services.AddScoped<IRequestHandler<GetUserPhotoByIdModel, Result<IEnumerable<PhotoUserDto>?>>, GetPhotosByIdUseCase>();
        services.AddScoped<IRequestHandler<UpdatePhotoMainModel, Result<bool>>, UpdatePhotoMainUseCase>();
        services.AddScoped<IRequestHandler<DeletePhotoModel, Result<bool>>, DeletePhotoUseCase>();
        services.AddScoped<IRequestHandler<UpdateUserActivityModel, Result<bool>>, UpdateUserActivityUseCase>();
        services.AddScoped<IRequestHandler<ToggleLikeModel, Result<bool>>, ToggleLikeUseCase>();
        services.AddScoped<IRequestHandler<UserIdLikeModel, Result<IReadOnlyList<string>>>, UserIdLikeUseCase>();
        services.AddScoped<IRequestHandler<GetUserLikesModel, Result<IEnumerable<UserDto>>>, GetUserLikesUseCase>();
        services.AddScoped<IRequestHandler<CreateMessageModel, Result<MessageDto>>, CreateMessageUseCase>();
        services.AddScoped<IRequestHandler<GetMessageModel, Result<IEnumerable<MessageDto>>>, GetMessagesUseCase>();
        services.AddScoped<IRequestHandler<GetMessageThreadModel, Result<IEnumerable<MessageDto>>>, GetMessageThreadUseCase>();
        services.AddScoped<IRequestHandler<DeleteMessageModel, Result<bool>>, DeleteMessageUseCase>();
        services.AddScoped<IRequestHandler<GetUsersRolesModel, Result<IEnumerable<UsersRolesDto>>>, GetUsersRolesUseCase>();
        services.AddScoped<IRequestHandler<EditUserRolesModel, Result<IEnumerable<string>>>, EditUserRolesUseCase>();
        services.AddScoped<IRequestHandler<AddGroupModel, Result<bool>>, AddGroupUseCase>();
        services.AddScoped<IRequestHandler<RemoveGroupModel, Result<bool>>, RemoveGroupUseCase>();
        services.AddScoped<IRequestHandler<GetGroupModel, Result<GroupDto?>>, GetGroupUseCase>();
        services.AddScoped<IRequestHandler<LogoutModel, Result<bool>>, LogoutUseCase>();

        services.AddValidators();

        return services;
    }

    internal static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<InsertUserValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        services.AddValidatorsFromAssemblyContaining<EditUserRolesValidator>();

        return services;
    }
}
