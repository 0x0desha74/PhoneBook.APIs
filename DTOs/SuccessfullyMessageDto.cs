namespace PhoneBook.APIs.DTOs
{
    public class SuccessfullyMessageDto
    {
        public string Message { get; set; }

        public SuccessfullyMessageDto(string message)
        {
            Message = message;
        }
    }
}
