using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;
using OBSSystem.API.DTOs;
using OBSSystem.Application.DTOs.Create;

public class UserService
{
    private readonly OBSContext _context;

    public UserService(OBSContext context)
    {
        _context = context;
    }

    public User CreateAdmin(CreateAdminDto adminDto)
    {
        if (_context.Users.Any(u => u.Email == adminDto.Email))
        {
            throw new InvalidOperationException("A user with the same email already exists.");
        }

        var admin = new User
        {
            Name = adminDto.Name,
            Email = adminDto.Email,
            Password = PasswordHasher.HashPassword(adminDto.Password),
            Role = "Admin"
        };

        _context.Users.Add(admin);
        _context.SaveChanges();

        return admin;
    }

    public Teacher CreateTeacher(CreateTeacherDto teacherDto)
    {
        if (_context.Users.Any(u => u.Email == teacherDto.Email))
        {
            throw new InvalidOperationException("A user with the same email already exists.");
        }

        var teacher = new Teacher
        {
            Name = teacherDto.Name,
            Email = teacherDto.Email,
            Password = PasswordHasher.HashPassword(teacherDto.Password),
            Role = "Teacher",
            Department = teacherDto.Department
        };

        _context.Users.Add(teacher);
        _context.SaveChanges();

        return teacher;
    }

    public Student CreateStudent(CreateStudentDto studentDto)
    {
        if (_context.Users.Any(u => u.Email == studentDto.Email))
        {
            throw new InvalidOperationException("A user with the same email already exists.");
        }

        var student = new Student
        {
            Name = studentDto.Name,
            Email = studentDto.Email,
            Password = PasswordHasher.HashPassword(studentDto.Password),
            Role = "Student",
            GradeLevel = studentDto.GradeLevel
        };

        _context.Users.Add(student);
        _context.SaveChanges();

        return student;
    }
}
