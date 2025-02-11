namespace Xanh_Dau.Helpers;

public static class StringHelper
{
    public static string MaskEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            return email;

        var parts = email.Split('@');
        if (parts[0].Length < 3) return email;

        var visiblePart = parts[0].Substring(0, 2); // Lấy 2 ký tự đầu
        var maskedPart = new string('*', parts[0].Length - 2); // Phần bị che
        return $"{visiblePart}{maskedPart}@{parts[1]}";
    }

    public static string MaskPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 3)
            return phoneNumber;

        var visiblePart = phoneNumber.Substring(phoneNumber.Length - 2); // Lấy 2 số cuối
        var maskedPart = new string('*', phoneNumber.Length - 2); // Phần bị che
        return $"{maskedPart}{visiblePart}";
    }
}