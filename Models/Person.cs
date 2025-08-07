using System.ComponentModel.DataAnnotations;

namespace Pozdravlyator.Models;

public class Person
{
    public int Id { get; set; }

    public string Name { get; set; }

    [DataType(DataType.Date)]
    public DateTime BirthDay { get; set; }

    [RegularExpression(@".*\.(jpg|jpeg|png|bmp)$", ErrorMessage = "Недопустимый формат файла")]
    public string? PicturePath { get; set; }
}
