using System.ComponentModel.DataAnnotations.Schema;

namespace Producer.Data
{
    public class DefaultEntity<T> where T : struct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
    }
}
