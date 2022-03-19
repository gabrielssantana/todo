using System.ComponentModel.DataAnnotations;

namespace MyTodoBaltaIo.ViewModels
{
    public class CreateTodoViewModel
    {
        [Required] 
        public string Title { get; set; }
    }
}