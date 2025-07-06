namespace LabourIdentity.DTOs
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public object Result { get; set; } = null;
    }
}
