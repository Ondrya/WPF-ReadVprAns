using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadVprAns
{
    /// <summary>
    /// Ответ пользователя
    /// </summary>
    public class Answer
    {
        public Answer(string id, string content)
        {
            Id = id;
            Content = content;
        }

        public string Id { get; set; }
        /// <summary>
        /// Строка с выбранными ответами из файла .ans
        /// </summary>
        public string Content { get; set; }

        public override string ToString()
        {
            return Id;
        }
    }
}
