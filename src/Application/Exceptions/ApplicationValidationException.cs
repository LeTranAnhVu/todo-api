namespace Application.Exceptions;

public class ApplicationValidationException(string message) : Exception(message);