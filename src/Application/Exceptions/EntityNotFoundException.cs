namespace Application.Exceptions;

public class EntityNotFoundException(string entity) : Exception(entity + " Not Found");