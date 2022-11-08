using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ValetAPI.Auth;

namespace ValetAPI.Controllers.API;

/// <summary>
/// 
/// </summary>
[Route("auth")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="configuration"></param>
    public AuthController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    
    /// <summary>
    /// Login
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var token = GetToken(authClaims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        return Unauthorized();
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response {Status = "Error", Message = "User already exists!"});

        IdentityUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response
                    {Status = "Error", Message = "User creation failed! Please check user details and try again."});

        return Ok(new Response {Status = "Success", Message = "User created successfully!"});
    }

    /// <summary>
    /// Register a new admin
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response {Status = "Error", Message = "User already exists!"});

        IdentityUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response
                    {Status = "Error", Message = "User creation failed! Please check user details and try again."});

        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _userManager.AddToRoleAsync(user, UserRoles.User);
        return Ok(new Response {Status = "Success", Message = "User created successfully!"});
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}