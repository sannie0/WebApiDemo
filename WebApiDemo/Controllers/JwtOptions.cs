namespace WebApiDemo.Controllers;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;

    public int ExpiresHours { get; set; }
    
    public int UserId { get; set; }
}