﻿using AutoMapper;
using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.DTOs;
using BTKRoadmapperAPI.Entities;
using k8s.KubeConfigModels;
using System.Collections.Generic;

namespace BTKRoadmapperAPI.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<UserDTO>> GetUserByMail(string mail)
        {
            var userList = await _userRepository.GetAllAsync();
            if (userList.Any(x => x.Email == mail))
            {
                var user = userList.Where(x => x.Email == mail).FirstOrDefault();
                return Response<UserDTO>.Success(_mapper.Map<UserDTO>(user),200);
            }
            else
            {
                return null;
            }

        }
        public async Task<Response<bool>> UpdateUser(UserDTO userDTO)
        {
            var userList = await _userRepository.GetAllAsync();
            var newUser = _mapper.Map<Entities.User>(userDTO);
            if (userList.Any(x => x.Email == userDTO.Email))
            {
                var user = userList.Where(x => x.Email == userDTO.Email).FirstOrDefault();
                user = newUser;
                await _userRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();

                return Response<bool>.Success(true, 201);
            }
            else
            {
                return Response<bool>.Success(false, 400);
            }

        }

        public async Task<bool> AddUser(UserDTO userDTO)
        {

                var user = _mapper.Map<Entities.User>(userDTO);
                var userPreference = _mapper.Map<UserPreference>(userDTO.UserPreferences);
                userPreference.User = user;
                user.Preferences?.Add(userPreference);
                await _userRepository.AddAsync(user);
                await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
