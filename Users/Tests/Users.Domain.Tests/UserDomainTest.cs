// using Users.Domain.Tests.Services;
// using Users.Domain.Users;
// using Users.Domain.Users.Exception;
// using Users.Domain.Users.Types;
//
// public class UserDomainTest
// {
//     IUserRepository _userRepository = new UserRepository();
//
//     [Fact]
//     public async Task create_user_by_username_success()
//     {
//         Username username = "test123";
//         var user = await User.CreateAsync(username, _userRepository);
//         Assert.NotNull(user);
//         Assert.IsType<User>(user);
//         Assert.Equal(username, user.Username);
//     }
//     
//     [Fact]
//     public async Task create_user_by_email_success()
//     {
//         Email email = "email@test.com";
//         var user = await User.CreateAsync(email, _userRepository);
//         Assert.NotNull(user);
//         Assert.IsType<User>(user);
//         Assert.Equal(email, user.Email);
//     }
//     
//     [Fact]
//     public async Task create_user_by_phoneNumber_success()
//     {
//         PhoneNumber number = "09111111112";
//         var user = await User.CreateAsync(number, _userRepository);
//         Assert.NotNull(user);
//         Assert.IsType<User>(user);
//         Assert.Equal(number, user.PhoneNumber);
//     }
//     
//     [Fact]
//     public async Task create_user_by_nationalCode_success()
//     {
//         NationalCode nationalCode = "1230058915";
//         var user = await User.CreateAsync(nationalCode, _userRepository);
//         Assert.NotNull(user);
//         Assert.IsType<User>(user);
//         Assert.Equal(nationalCode, user.NationalCode);
//     }
//     
//     [Fact]
//     public async Task create_user_by_username_duplicate_fail()
//     {
//         Username username = "testaaa";
//         var user = await User.CreateAsync(username, _userRepository);
//         await _userRepository.CreateAsync(user);
//         await Assert.ThrowsAsync<UserDuplicateIdentifierException>(async () =>
//         {
//             await User.CreateAsync(username, _userRepository);   
//         });
//     }
//
//     [Fact]
//     public async Task create_user_by_email_duplicate_fail()
//     {
//         Email email = "test@gmail.com";
//         var user = await User.CreateAsync(email, _userRepository);
//         await _userRepository.CreateAsync(user);
//         await Assert.ThrowsAsync<UserDuplicateIdentifierException>(async () =>
//         {
//             await User.CreateAsync(email, _userRepository);
//         });
//     }
//     
//     [Fact]
//     public async Task create_user_by_phoneNumber_duplicate_fail()
//     {
//         PhoneNumber phoneNumber = "09111111111";
//         var user = await User.CreateAsync(phoneNumber, _userRepository);
//         await _userRepository.CreateAsync(user);
//         await Assert.ThrowsAsync<UserDuplicateIdentifierException>(async () => { await User.CreateAsync(phoneNumber, _userRepository); });
//     }
//     
//     [Fact]
//     public async Task create_user_by_nationalCode_duplicate_fail()
//     {
//         NationalCode nationalCode = "1111251452";
//         var user = await User.CreateAsync(nationalCode, _userRepository);
//         await _userRepository.CreateAsync(user);
//         await Assert.ThrowsAsync<UserDuplicateIdentifierException>(async () => { await User.CreateAsync(nationalCode, _userRepository); });
//     }
//
//
// }