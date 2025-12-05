namespace EmailSecurityApi.Models
{
   public class EmailRequest
{
    public string Email { get; set; } = string.Empty;
}

public class EmailResponse
{
    public string Email { get; set; } = string.Empty;
    public bool Secured { get; set; }
}

public class EmailBatchRequest
{
    public List<string> Emails { get; set; } = new();
}

public class EmailCheckResult
{
    public string Email { get; set; } = string.Empty;
    public bool Secured { get; set; }
}

public class EmailBatchResponse
{
    public List<EmailCheckResult> Results { get; set; } = new();
}

}
