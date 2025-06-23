namespace MedicineWebsite.Infrastructure.Services;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    Task<bool> SendEmailConfirmationAsync(string to, string confirmationLink);
    Task<bool> SendPasswordResetAsync(string to, string resetLink);
    Task<bool> SendOrderConfirmationAsync(string to, string orderDetails);
}

public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<PaymentResult> RefundPaymentAsync(string transactionId, decimal amount);
    Task<PaymentStatus> GetPaymentStatusAsync(string transactionId);
}

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName, string folder = "medicines");
    Task<bool> DeleteImageAsync(string publicId);
    Task<string> GetOptimizedImageUrlAsync(string publicId, int width = 300, int height = 300);
}

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string CustomerEmail { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, string> Metadata { get; set; } = new();
}

public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string PaymentUrl { get; set; } = string.Empty;
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Succeeded,
    Failed,
    Cancelled,
    Refunded
}
