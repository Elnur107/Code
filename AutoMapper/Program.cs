using System;
using System.Reflection;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public class UserDto
{
    public string Name { set; get; }
    public int Age { get; set; }
}

public interface IMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
}

public class Mapper<TSource, TDestination> : IMapper<TSource, TDestination>
{
    public TDestination Map(TSource source)
    {
        TDestination destination = Activator.CreateInstance<TDestination>();

        PropertyInfo[] sourceProperties = typeof(TSource).GetProperties();
        PropertyInfo[] destinationProperties = typeof(TDestination).GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            foreach (var destinationProperty in destinationProperties)
            {
                if (sourceProperty.Name == destinationProperty.Name &&
                    sourceProperty.PropertyType == destinationProperty.PropertyType &&
                    destinationProperty.CanWrite)
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                    break;
                }
            }
        }

        return destination;
    }
}

class Program
{
    static void Main()
    {
        User user = new User { Id = 1, Name = "Esger", Age = 20 };

        IMapper<User, UserDto> mapper = new Mapper<User, UserDto>();
        UserDto userDto = mapper.Map(user);

        Console.WriteLine($"UserDto: Name={userDto.Name}, Age={userDto.Age}");
    }
}
