namespace OBSSystem.Application.Exceptions;

public class StudentNotFoundException : Exception
{
    public StudentNotFoundException() : base("Student not found!") {}
    
    public StudentNotFoundException(string message) : base(message) {}
}