using AutoMapper;
using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.DTOs;
using BTKRoadmapperAPI.Entities;
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

        public async Task<User> GetUserByMail(string mail)
        {
            var userList = await _userRepository.GetAllAsync();
            if (userList.Any(x => x.Email == mail))
            {
                var user = userList.Where(x => x.Email == mail).FirstOrDefault();
                return user;
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> AddUser(UserDTO userDTO)
        {
            var userList = await _userRepository.GetAllAsync();
            if (userList.Any(x => x.Email == userDTO.Email))
            {
                var user = userList.Where(x => x.Email == userDTO.Email).FirstOrDefault();
                user.Preferences?.Add(_mapper.Map<UserPreference>(userDTO.UserPreferences));
                await _userRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                var PreferencesList = new List<UserPreference>();
                PreferencesList.Add(_mapper.Map<UserPreference>(userDTO.UserPreferences));
                var newUser = new User()
                {
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                    Role = userDTO.Role,
                    Preferences = PreferencesList
                };
            }
            return true;
        }
    }
}
